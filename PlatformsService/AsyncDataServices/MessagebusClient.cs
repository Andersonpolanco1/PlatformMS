using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<MessagebusClient> _logger;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessagebusClient(IConfiguration conf, ILogger<MessagebusClient> logger)
        {
            _config = conf;
            _logger = logger;

            try
            {
                var factory = new ConnectionFactory() 
                { 
                    HostName = _config.GetValue<string>("Rabbitmq:Host"), 
                    Port = _config.GetValue<int>("Rabbitmq:Port")
                };
                _connection = factory.CreateConnection();
                _connection.ConnectionShutdown += OnConnectionShutDown;
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(_config.GetValue<string>("Rabbitmq:ExchangeName"), ExchangeType.Fanout);

                _logger.LogInformation("--> Connected to message bus.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"--> Could not connect to message bus: {ex.Message}");
            }
        }

        private void OnConnectionShutDown(object sender, ShutdownEventArgs e)
        {
            _logger.LogInformation("--> Connection shutdown");
        }

        public void PublishNewPlatform(PlatformPublishedDto platform)
        {
            if (_connection != null && _connection.IsOpen)
            {
                _logger.LogInformation("--> Connection state: open -> Sending message to Message Bus...");
                var message = JsonSerializer.Serialize(platform);
                SendMessage(message);
            }
            else
            {
                _logger.LogError("--> Connection state: closed -> Could not send message to Message Bus.");
            }

        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message ?? string.Empty);
            _channel.BasicPublish(_config.GetValue<string>("Rabbitmq:ExchangeName"), "", null, body);
            _logger.LogInformation($"--> We have send: {message}");
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
