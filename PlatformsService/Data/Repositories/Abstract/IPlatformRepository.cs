using PlatformService.Models;
using System.Collections;
using System.Collections.Generic;

namespace PlatformService.Data.Repositories.Abstract
{
    public interface IPlatformRepository
    {
        IEnumerable<Platform> GetAll();
        Platform GetById(int platformId);
        void CreatePlatform(Platform platform);
        bool SaveChanges();
    }
}
