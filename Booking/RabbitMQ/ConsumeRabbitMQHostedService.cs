using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Booking.RabbitMQ
{
    public class ConsumeRabbitMQHostedService : BackgroundService
    {
        //private readonly AppDbContext _context;
        private readonly ILogger _logger;
        private IConnection _connection;
        private IModel _channel;

        public ConsumeRabbitMQHostedService(ILoggerFactory loggerFactory)
        {
            //_context = context;
            this._logger = loggerFactory.CreateLogger<ConsumeRabbitMQHostedService>();
            InitRabbitMQ();
        }

        private void InitRabbitMQ()
        {
            var factory = new ConnectionFactory
            {
                Uri = new System.Uri("amqp://guest:guest@localhost:5672")
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare("MQueue1",
               durable: true,
               exclusive: false,
               autoDelete: false,
               arguments: null);
        }

        protected override Task ExecuteAsync(System.Threading.CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                // received message  
                //if(message!=null)
                //{
                //    _context.PassMessage(message);
                //}
            };
            _channel.BasicConsume("MQueue1", true, consumer);
            return Task.CompletedTask;
        }

    }
}
