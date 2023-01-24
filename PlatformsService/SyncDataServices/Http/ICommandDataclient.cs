using PlatformService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformService.SyncDataServices.Http
{
    public interface ICommandDataclient
    {
        Task SendPlatformToCommand(PlatformReadDto plat);
    }
}
