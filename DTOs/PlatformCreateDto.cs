using System.ComponentModel.DataAnnotations;

namespace PlatformService.DTOs
{
    public class PlatformCreateDto
    {
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        public string Publishser { get; set; } = string.Empty;
        public string Cost { get; set; } = string.Empty;
    }
}
