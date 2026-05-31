using RabbitMQ.Client;
using System.Text;

namespace QuickPay.Services
{
    public class RabbitMQService
    {
        public async Task PublishMessage(string message)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            using var connection = await factory.CreateConnectionAsync();

            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: "transactionQueue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: "transactionQueue",
                body: body);
        }
    }
}