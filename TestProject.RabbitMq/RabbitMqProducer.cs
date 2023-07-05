using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace TestProject.RabbitMq
{
    public class RabbitMqProducer : IMessageProducer
    {
        public void SendMessage<T>(T message)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };
            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: "orders",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish("", "orders", false, null, body);
        }
    }
}
