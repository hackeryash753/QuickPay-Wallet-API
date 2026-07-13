using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace QuickPay.Services
{
    public class RabbitMQService
    {
        private readonly IConfiguration configuration;

        public RabbitMQService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task PublishMessage<T>(T messageObject)
        {
            var rabbitHost =
                configuration["RabbitMQ:HostName"];

            var factory = new ConnectionFactory
            {
                HostName = rabbitHost
            };

            await using var connection =
                await factory.CreateConnectionAsync();

            await using var channel =
                await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: "transactionQueue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var message =
                JsonSerializer.Serialize(messageObject);

            var body =
                Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: "transactionQueue",
                body: body);

            Console.WriteLine(
                $"Message Published: {message}");
        }
    }
}