using AutoMapper;
using CommandService.DTOS;
using CommandService.Models;
using CommandService.Repositories.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;

namespace CommandService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly ICommandsRepository _commandsRepository;
        private readonly IMapper _mapper;

        public PlatformsController(ICommandsRepository commandsRepository, IMapper mapper)
        {
            _commandsRepository = commandsRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetPlatforms()
        {
            var platforms = _commandsRepository.GetPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }


        [HttpPost]
        public ActionResult TestInboundConnection(PlatformPublished platformPublished)
        {
            Console.WriteLine($"---> Receibed in CommandService via HTTP: {platformPublished.Id} - {platformPublished.Name}");
            return Ok(platformPublished);
        }
    }
}
