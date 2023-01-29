using System.ComponentModel.DataAnnotations;

namespace CommandService.DTOS
{
    public class CommandCreateDto
    {
        [MaxLength(100)]
        public string HowTo { get; set; }

        [MaxLength(100)]
        public string CommandLine { get; set; }
    }
}
