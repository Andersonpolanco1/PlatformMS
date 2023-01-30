using CommandService.DTOS;
using CommandService.Models;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using System.Collections;
using System.Collections.Generic;

namespace CommandService.Repositories.Abstract
{
    public interface ICommandsRepository
    {

        IEnumerable<Platform> GetPlatforms();
        void CreatePlatform(Platform platform);
        bool PlatfotmExists(int platformId);
        bool PlatfotmExternalIdExists(int platformExternalId);

        IEnumerable<Command> GetCommandsByPlatformId(int platformId);
        Command GetCommand(int PlatformId, int CommandId);
        Command CreateCommand(int PlatformId, Command Command);

        bool SaveChanges();
    }
}
