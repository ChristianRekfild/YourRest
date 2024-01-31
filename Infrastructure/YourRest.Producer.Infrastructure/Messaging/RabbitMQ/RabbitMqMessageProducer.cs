using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using YourRest.Domain.Events;

namespace YourRest.Producer.Infrastructure.Messaging.RabbitMQ
{
    public class RabbitMqMessageProducer
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqMessageProducer(string hostname, string queueName)
        {
            var factory = new ConnectionFactory() { HostName = hostname };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        public void Send(IDomainEvent domainEvent)
        {
            var message = JsonSerializer.Serialize(domainEvent);
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "",
                routingKey: "accommodation_created_queue",
                basicProperties: null,
                body: body);
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}

