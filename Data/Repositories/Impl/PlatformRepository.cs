using PlatformService.Data.Repositories.Abstract;
using PlatformService.Models;
using System.Collections.Generic;
using System.Linq;

namespace PlatformService.Data.Repositories.Impl
{
    public class PlatformRepository : IPlatformRepository
    {
        private readonly ApplicationDbContext _context;

        public PlatformRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void CreatePlatform(Platform platform)
        {
            _context.Platforms.Add(platform);
        }

        public IEnumerable<Platform> GetAll()
        {
            return _context.Platforms.ToList();
        }

        public Platform GetById(int platformId)
        {
            return _context.Platforms.FirstOrDefault(p => p.Id == platformId);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges()) >= 0;
        }
    }
}
