using AutoMapper;
using CommandService.DTOS;
using CommandService.Models;
using System.Net.NetworkInformation;

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
            CreateMap<PlatformPublished, Platform>()
                .ForMember(dest => dest.ExternalId, opts => opts.MapFrom(src => src.Id));

        }
    }
}
