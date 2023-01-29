using AutoMapper;
using CommandService.DTOS;
using CommandService.Models;
using CommandService.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
 
namespace CommandService.Controllers
{
    [Route("api/c/Platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController: ControllerBase
    {
        private readonly ICommandsRepository _commandsRepository;
        private readonly IMapper _mapper;

        public CommandsController(ICommandsRepository commandsRepository, IMapper mapper)
        {
            _commandsRepository = commandsRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetCommandsByPlatformId(int platformId)
        {
            if (!_commandsRepository.PlatfotmExists(platformId))
                return NotFound();

            var commands = _commandsRepository.GetCommandsByPlatformId(platformId);
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }

        [HttpGet("{commandId}", Name = "GetCommand")]
        public IActionResult GetCommand(int platformId, int commandId)
        {
            if (!_commandsRepository.PlatfotmExists(platformId))
                return NotFound();

            var command = _commandsRepository.GetCommand(platformId, commandId);

            if(command is null)
                return NotFound();

            return Ok(_mapper.Map<CommandReadDto>(command));
        }

        [HttpPost]
        public IActionResult CreateCommand(int platformId, CommandCreateDto command)
        {
            if (!_commandsRepository.PlatfotmExists(platformId))
                return NotFound();

            var commandToCreate = _mapper.Map<Command>(command);
            var commandDb = _commandsRepository.CreateCommand(platformId, commandToCreate);

            return CreatedAtRoute(nameof(GetCommand),
                new { PlatformId = platformId, CommandId = commandDb.Id}, 
                _mapper.Map<CommandReadDto>(commandDb)
                );
        }
    }

    


}
