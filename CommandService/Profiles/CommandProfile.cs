using AutoMapper;
using CommandService.DTOS;
using CommandService.Models;

namespace CommandService.Profiles
{
    public class CommandProfile: Profile
    {
        public CommandProfile()
        {
            CreateMap<CommandCreateDto, Command>();
            CreateMap<PlatformCreateDto, Platform>();

            CreateMap<Command, CommandReadDto>();
            CreateMap<Platform, PlatformReadDto>();

        }
    }
}
