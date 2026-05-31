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
            Console.WriteLine("CONSUMER STARTED");

            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            var connection =
                await factory.CreateConnectionAsync();

            var channel =
                await connection.CreateChannelAsync();

            // Main Queue
            await channel.QueueDeclareAsync(
                queue: "transactionQueue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            // Dead Letter Queue
            await channel.QueueDeclareAsync(
                queue: "transactionQueue-dlq",
                durable: true,
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

                    Console.WriteLine(
                        $"Message Received: {message}");

                    using var scope =
                        scopeFactory.CreateScope();

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

                    Console.WriteLine(
                        "Notification saved successfully");

                    await channel.BasicAckAsync(
                        deliveryTag: ea.DeliveryTag,
                        multiple: false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        $"Consumer Error: {ex.Message}");

                    var failedBody =
                        ea.Body.ToArray();

                    await channel.BasicPublishAsync(
                        exchange: "",
                        routingKey: "transactionQueue-dlq",
                        body: failedBody);

                    await channel.BasicAckAsync(
                        deliveryTag: ea.DeliveryTag,
                        multiple: false);
                }
            };

            await channel.BasicConsumeAsync(
                queue: "transactionQueue",
                autoAck: false,
                consumer: consumer);

            Console.WriteLine(
                "Consumer Registered Successfully");

            await Task.Delay(
                Timeout.Infinite,
                stoppingToken);
        }
    }
}