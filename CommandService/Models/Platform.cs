using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CommandService.Models
{
    public class Platform
    {
        public int Id { get; set; }

        public int ExternalId { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public IEnumerable<Command> Commands { get; set; } = new List<Command>();   
    }
}
