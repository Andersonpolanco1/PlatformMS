using System.ComponentModel.DataAnnotations;

namespace PlatformService.DTOs
{
    public class PlatformReadDto
    {
        public string Name { get; set; } = string.Empty;
        public string Publishser { get; set; } = string.Empty;
        public string Cost { get; set; } = string.Empty;
    }
}
