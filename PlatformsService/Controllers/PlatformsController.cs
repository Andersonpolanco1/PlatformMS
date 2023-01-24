using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

        public PlatformsController(IPlatformRepository platformRepository, IMapper mapper, ICommandDataclient client)
        {
            _platformRepository = platformRepository;
            _mapper = mapper;
            _commandDataclient = client;
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
            catch (System.Exception)
            {
                Console.WriteLine("Not send to command service");
            }

            return CreatedAtRoute(nameof(Get), new { platformId = newPlatform.Id }, platformRead);
        }


    }
}
