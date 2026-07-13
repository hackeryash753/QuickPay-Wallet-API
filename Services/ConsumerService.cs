using Microsoft.Extensions.Configuration;
using QuickPay.Data;
using QuickPay.Models.Domain;
using QuickPay.Models.DTO;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace QuickPay.Services
{
    public class ConsumerService : BackgroundService
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly IConfiguration configuration;

        public ConsumerService(
            IServiceScopeFactory scopeFactory,
            IConfiguration configuration)
        {
            this.scopeFactory = scopeFactory;
            this.configuration = configuration;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            Console.WriteLine("CONSUMER STARTED");

            var rabbitHost =
                configuration["RabbitMQ:HostName"] ?? "rabbitmq";

            var factory = new ConnectionFactory
            {
                HostName = rabbitHost
            };

            IConnection? connection = null;

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    connection =
                        await factory.CreateConnectionAsync();

                    Console.WriteLine(
                        $"Connected to RabbitMQ ({rabbitHost})");

                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        $"RabbitMQ not ready: {ex.Message}");

                    Console.WriteLine(
                        "Retrying in 5 seconds...");

                    await Task.Delay(
                        TimeSpan.FromSeconds(5),
                        stoppingToken);
                }
            }

            if (connection == null)
            {
                Console.WriteLine(
                    "Unable to connect to RabbitMQ");

                return;
            }

            await using var channel =
                await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: "transactionQueue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

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

                    var json =
                        Encoding.UTF8.GetString(body);

                    Console.WriteLine(
                        $"Message Received: {json}");

                    var notificationMessage =
                        JsonSerializer.Deserialize<NotificationMessageDto>(json);

                    if (notificationMessage == null)
                    {
                        throw new Exception(
                            "Failed to deserialize notification message");
                    }

                    using var scope =
                        scopeFactory.CreateScope();

                    var dbContext =
                        scope.ServiceProvider
                        .GetRequiredService<QuickPayDbContext>();

                    var notification =
                        new Notification
                        {
                            Message = notificationMessage.Message,
                            UserId = notificationMessage.UserId,
                            CreatedAt = DateTime.UtcNow,
                            IsRead = false
                        };

                    dbContext.Notifications.Add(notification);

                    await dbContext.SaveChangesAsync();

                    Console.WriteLine(
                        $"Notification saved for UserId: {notification.UserId}");

                    await channel.BasicAckAsync(
                        deliveryTag: ea.DeliveryTag,
                        multiple: false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        $"Consumer Error: {ex}");

                    await channel.BasicPublishAsync(
                        exchange: "",
                        routingKey: "transactionQueue-dlq",
                        body: ea.Body);

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

            try
            {
                await Task.Delay(
                    Timeout.Infinite,
                    stoppingToken);
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine(
                    "Consumer service shutting down...");
            }
        }
    }
}