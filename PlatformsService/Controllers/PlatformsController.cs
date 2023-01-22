using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.Data.Repositories.Abstract;
using PlatformService.DTOs;
using PlatformService.Models;
using Platform = PlatformService.Models.Platform;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepository _platformRepository;
        private readonly IMapper _mapper;

        public PlatformsController(IPlatformRepository platformRepository, IMapper mapper)
        {
            _platformRepository = platformRepository;
            _mapper = mapper;
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
        public ActionResult<PlatformReadDto> GetPlatforms(PlatformCreateDto platform)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newPlatform = _mapper.Map<Platform>(platform);
            _platformRepository.CreatePlatform(newPlatform);
            _platformRepository.SaveChanges();
            return CreatedAtRoute(nameof(Get), new { platformId = newPlatform.Id }, _mapper.Map<PlatformReadDto>(newPlatform));
        }


    }
}
