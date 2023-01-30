using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

        public PlatformsController(IPlatformRepository platformRepository, IMapper mapper, ICommandDataclient client, IMessageBusClient messageBusClient)
        {
            _platformRepository = platformRepository;
            _mapper = mapper;
            _commandDataclient = client;
            _messageBusClient = messageBusClient;
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

            var platformRead =  _mapper.Map<PlatformReadDto>(newPlatform);

            try
            {
                await _commandDataclient.SendPlatformToCommand(platformRead);
            }
            catch (Exception)
            {
                Console.WriteLine("--> Not send to command service");
            }

            try
            {
                var platformPublished = _mapper.Map<PlatformPublishedDto>(platformRead);
                platformPublished.Event = PlatformEvents.PlatformPublished;
                _messageBusClient.PublishNewPlatform(platformPublished);
            }
            catch (Exception)
            {

                Console.WriteLine("--> Not send to messagebus");
            }

            return CreatedAtRoute(nameof(Get), new { platformId = newPlatform.Id }, platformRead);
        }


    }
}
