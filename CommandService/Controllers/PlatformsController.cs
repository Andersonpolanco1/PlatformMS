using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace CommandService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        public PlatformsController()
        {
                                                                    
        }


        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("---> Testing");

            return Ok("Test");
        }
    }
}
