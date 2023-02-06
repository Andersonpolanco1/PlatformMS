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
            CreateMap<Command, CommandReadDto>();

            CreateMap<PlatformCreateDto, Platform>();
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformPublished, Platform>()
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForMember(dest => dest.ExternalId, opts => opts.MapFrom(src => src.Id));

        }
    }
}
