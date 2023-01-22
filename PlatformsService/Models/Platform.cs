using System.ComponentModel.DataAnnotations;

namespace PlatformService.Models
{
    public class Platform
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        public string Publishser { get; set; } = string.Empty;
        public string Cost { get; set; } = string.Empty;
    }
}
 