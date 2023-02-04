using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PlatformService.AsyncDataServices;
using PlatformService.Data.Repositories.Abstract;
using PlatformService.DTOs;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepository _platformRepository;
        private readonly IMapper _mapper;
        private readonly ICommandDataclient _commandDataclient;
        private readonly IMessageBusClient _messageBusClient;
        private readonly ILogger<PlatformsController> _logger;

        public PlatformsController(IPlatformRepository platformRepository, IMapper mapper, ICommandDataclient client, IMessageBusClient messageBusClient, ILogger<PlatformsController> logger)
        {
            _platformRepository = platformRepository;
            _mapper = mapper;
            _commandDataclient = client;
            _messageBusClient = messageBusClient;
            _logger = logger;
        }

        // GET: api/Platforms
        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            var platforms = _platformRepository.GetAll();
            return  Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }


        // GET: api/Platforms/5
        [HttpGet("{platformId}", Name ="Get")]
        public ActionResult<PlatformReadDto> Get(int platformId)
        {
            var platform = _platformRepository.GetById(platformId);
            return Ok(_mapper.Map<PlatformReadDto>(platform));
        }

        // POST: api/Platforms
        [HttpPost]
        public async  Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platform)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newPlatform = _mapper.Map<Platform>(platform);
            _platformRepository.CreatePlatform(newPlatform);
            _platformRepository.SaveChanges();

            var platformPublished = _mapper.Map<PlatformPublishedDto>(newPlatform);
            platformPublished.Event = PlatformEvents.PlatformPublished;

            //This is a example to send data via HTTP to another service
            try
            {
                await _commandDataclient.SendPlatformToCommand(platformPublished);
                _logger.LogInformation($"--> Message was sent to command service via HTTP");
            }
            catch (Exception ex)
            {
                _logger.LogError($"--> Can not send Message to command service via HTTP: {ex.Message}");
            }
            // end example
       
            _messageBusClient.PublishNewPlatform(platformPublished);  // Sending to Message Bus 

            var platformRead = _mapper.Map<PlatformReadDto>(newPlatform);
            return CreatedAtRoute(nameof(Get), new { platformId = newPlatform.Id }, platformRead);
        }


    }
}
