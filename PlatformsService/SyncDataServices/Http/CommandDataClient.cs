using Microsoft.Extensions.Configuration;
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

        public CommandDataClient(HttpClient client, IConfiguration conf)
        {
            _httpClient = client;
            _conf = conf;
        }

        public async Task SendPlatformToCommand(PlatformReadDto plat)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(plat),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync($"{_conf["CommandServiceUrl"]}", httpContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Successfully!");
            }
            else
            {
                Console.WriteLine("Not send to command service!");
            }
        }
    }
}
