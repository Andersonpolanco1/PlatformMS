using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CommandService.Models
{
    public class Command
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string HowTo { get; set; }

        [MaxLength(100)]
        public string CommandLine { get; set; }

        public int PlatformId { get; set; }
        public Platform Platform { get; set; }
    }
}
