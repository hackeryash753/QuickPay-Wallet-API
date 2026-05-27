using QuickPay.Data;
using QuickPay.Models.Domain;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace QuickPay.Services
{
    public class ConsumerService : BackgroundService
    {
        private readonly IServiceScopeFactory scopeFactory;

        public ConsumerService(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            var connection =
                await factory.CreateConnectionAsync();

            var channel =
                await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: "transactionQueue",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer =
                new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();

                    var message =
                        Encoding.UTF8.GetString(body);

                    using var scope = scopeFactory.CreateScope();

                    var dbContext =
                        scope.ServiceProvider
                        .GetRequiredService<QuickPayDbContext>();

                    var notification = new Notification
                    {
                        Message = message,
                        CreatedAt = DateTime.UtcNow,
                        IsRead = false
                    };

                    dbContext.Notifications.Add(notification);

                    await dbContext.SaveChangesAsync();

                    
                    await channel.BasicAckAsync(
                        deliveryTag: ea.DeliveryTag,
                        multiple: false
                        );
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                    var failedBody = ea.Body.ToArray();

                    await channel.BasicPublishAsync(
                        exchange: "",
                        routingKey: "transactionQueue-dlq",
                        body: failedBody);

                    await channel.BasicAckAsync(
                        deliveryTag: ea.DeliveryTag,
                        multiple: false);
                }
            };

        }
    }
}
