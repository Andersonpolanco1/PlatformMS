using AutoMapper;
using CommandService.DTOS;
using CommandService.Models;
using CommandService.Repositories.Abstract;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;

namespace CommandService.EventProcesors
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory serviceScopeFactory, IMapper mapper)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _mapper = mapper;
        }
        public void ProcessEvent(string message)
        {
            var evnt = DetermineEvent(message);

            switch (evnt)
            {
                case EventType.PlatformPublished:
                    AddPlatform(message);
                    break;
                case EventType.Undefined:
                    break;
                default:
                    break;
            }
        }

        private EventType DetermineEvent(string message)
        {
            var eventDto = JsonSerializer.Deserialize<GenericEventDto>(message);

            switch (eventDto.Event)
            {
                case "platform_published":
                    Console.WriteLine("--> Platform published detected!");
                    return EventType.PlatformPublished;

                default:
                    Console.WriteLine("--> Could not determine event!");
                    return EventType.Undefined;
            }
        }

        private void AddPlatform(string platformPublished)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var repository = scope.ServiceProvider.GetService<ICommandsRepository>();
                var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublished>(platformPublished);

                try
                {
                    var platformCreate = _mapper.Map<PlatformCreateDto>(platformPublishedDto);

                    if (repository.PlatfotmExternalIdExists(platformCreate.ExternalId))
                    {
                        Console.WriteLine("--> Could not save platform in command service, Platform exists.");
                    }
                    else
                    {
                        var platform = _mapper.Map<Platform>(platformCreate);
                        repository.CreatePlatform(platform);
                        repository.SaveChanges();
                        Console.WriteLine("--> platform saved in commandService!.");
                    }
                }
                catch (Exception)
                {

                    Console.WriteLine("--> Could not save platform in command service, some error occurred.");
                }
            }
        }
    }

    public enum EventType
    {
        PlatformPublished,
        Undefined
    }
}
