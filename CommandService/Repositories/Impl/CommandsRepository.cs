using CommandService.DTOS;
using CommandService.Models;
using CommandService.Repositories.Abstract;
using System.Collections.Generic;
using System;
using CommandService.Data;
using System.Linq;

namespace CommandService.Repositories.Impl
{
    public class CommandsRepository : ICommandsRepository
    {
        private readonly ApplicationDbContext _context;

        public CommandsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Command CreateCommand(int PlatformId, Command command)
        {
            if (command is null)
                throw new ArgumentNullException("Command to save is null");

            command.PlatformId = PlatformId;
            _context.Commands.Add(command);
            _context.SaveChanges();

            return command;

        }

        public void CreatePlatform(Platform platform)
        {
            if (platform is null)
                throw new ArgumentNullException("Platform to save is null");

            _context.Platforms.Add(platform);
        }

        public Command GetCommand(int platformId, int commandId)
        {
            return _context.Commands.FirstOrDefault(c =>
                c.PlatformId == platformId && c.Id == commandId);
        }

        public IEnumerable<Command> GetCommandsByPlatformId(int platformId)
        {
            return _context.Commands.Where(c => c.PlatformId == platformId);
        }

        public IEnumerable<Platform> GetPlatforms()
        {
            return _context.Platforms.ToList();
        }

        public bool PlatfotmExists(int platformId)
        {
            return _context.Platforms.Any(p => p.Id == platformId);
        }

        public bool PlatfotmExternalIdExists(int platformExternalId)
        {
            return _context.Platforms.Any(p => p.ExternalId == platformExternalId);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges()) > 1;
        }
    }
}
