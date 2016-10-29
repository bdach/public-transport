using System;
using System.ComponentModel.DataAnnotations;

namespace PublicTransport.Domain.Entities
{
    public class Calendar : Entity
    {
        [Required]
        public bool Monday { get; set; }

        [Required]
        public bool Tuesday { get; set; }

        [Required]
        public bool Wednesday { get; set; }

        [Required]
        public bool Thursday { get; set; }

        [Required]
        public bool Friday { get; set; }

        [Required]
        public bool Saturday { get; set; }

        [Required]
        public bool Sonday { get; set; }
        
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}
