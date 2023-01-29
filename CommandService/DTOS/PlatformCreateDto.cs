using System.ComponentModel.DataAnnotations;

namespace CommandService.DTOS
{
    public class PlatformCreateDto
    {
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
