using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PlatformService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlatformService.SyncDataServices.Http
{
    public class CommandDataClient : ICommandDataclient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _conf;
        private readonly ILogger<CommandDataClient> _logger;

        public CommandDataClient(HttpClient client, IConfiguration conf, ILogger<CommandDataClient> logger)
        {
            _httpClient = client;
            _conf = conf;
            _logger = logger;
        }

        public async Task SendPlatformToCommand(PlatformPublishedDto plat)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(plat),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync($"{_conf["CommandServiceUrl"]}", httpContent);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Successfully sent to commandService!");
            }
            else
            {
                _logger.LogError("Not send to command service!");
            }
        }
    }
}
