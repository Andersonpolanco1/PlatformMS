using Microsoft.Extensions.Configuration;
using PlatformService.DTOs;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;

namespace PlatformService.AsyncDataServices
{
    public class MessagebusClient : IMessageBusClient, IDisposable
    {
        private readonly IConfiguration _config;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessagebusClient(IConfiguration conf)
        {
            _config = conf;

            var factory = new ConnectionFactory() 
            { 
                HostName = _config.GetValue<string>("Rabbitmq:Host"), 
                Port = _config.GetValue<int>("Rabbitmq:Port")
            };

            try
            {
                _connection = factory.CreateConnection();
                _connection.ConnectionShutdown += OnConnectionShutDown;
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(_config.GetValue<string>("Rabbitmq:ExchangeName"), ExchangeType.Fanout);

                Console.WriteLine("--> Connected to message bus.");
            }
            catch (Exception)
            {
                Console.WriteLine("--> Could not connect to message bus.");
            }
        }

        private void OnConnectionShutDown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> Connection shutdown");
        }

        public void PublishNewPlatform(PlatformPublishedDto platform)
        {
            if (_connection.IsOpen)
            {
                Console.WriteLine("--> Connection state: open -> Sending message...");
                var message = JsonSerializer.Serialize(platform);
                SendMessage(message);
            }
            else
            {
                Console.WriteLine("--> Connection state: closed -> Could not send message.");
            }

        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message ?? string.Empty);
            _channel.BasicPublish(_config.GetValue<string>("Rabbitmq:ExchangeName"), "", null, body);
            Console.WriteLine($"--> We have send: {message}");
        }

        public void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
    }
}
