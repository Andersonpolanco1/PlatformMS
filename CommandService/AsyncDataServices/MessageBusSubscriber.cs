using CommandService.EventProcesors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommandService.AsyncDataServices
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IConfiguration _conf;
        private readonly IEventProcessor _eventProcessor;
        private IConnection _connection;
        private IModel _channel;
        private string _queueName;

        public MessageBusSubscriber(IConfiguration conf, IEventProcessor eventProcessor)
        {
            _conf = conf;
            _eventProcessor = eventProcessor;
            Initialize();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += OnReceibed;
            _channel.BasicConsume(_queueName, true, consumer);
            return Task.CompletedTask;
        }

        private void OnReceibed(object sender, BasicDeliverEventArgs e)
        {
            Console.WriteLine("--> Event receibed");

            var body = e.Body;
            var notificationMsg = Encoding.UTF8.GetString(body.ToArray());
            _eventProcessor.ProcessEvent(notificationMsg);
        }

        private void Initialize()
        {
            var host = _conf.GetValue<string>("Rabbitmq:Host");
            var port = _conf.GetValue<int>("Rabbitmq:Port");
            var exchangeName = _conf.GetValue<string>("Rabbitmq:ExchangeName");

            var factory = new ConnectionFactory() { HostName = host, Port = port };

            _connection = factory.CreateConnection();
            _connection.ConnectionShutdown += OnConnectionShutdown;
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout);
            _queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(_queueName, exchangeName, string.Empty);

            Console.WriteLine("--> Listening on the Message Bus...");

        }

        private void OnConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ disconnected.");
        }

        public override void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
            base.Dispose();
        }
    }
}
