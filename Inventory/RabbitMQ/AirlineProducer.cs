using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.RabbitMQ
{
    public class AirlineProducer
    {
        public static void Producer(string airlineName)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare("MQueue1",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            var message = new
            {
                Name = "Producer",
                message = airlineName,
            };
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            channel.BasicPublish("", "MQueue1", null, body);
        }
       
    }
}
