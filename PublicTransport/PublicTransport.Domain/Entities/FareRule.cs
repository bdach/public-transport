using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicTransport.Domain.Entities
{
    public class FareRule : Entity
    {
        [Required]
        public int RouteId { get; set; }

        [ForeignKey("RouteId")]
        public Route Route { get; set; }

        [Required]
        public int OriginId { get; set; }

        [ForeignKey("OriginId")]
        public Zone Origin { get; set; }

        [Required]
        public int DestinationId { get; set; }

        [ForeignKey("DestinationId")]
        public Zone Destination { get; set; }
    }
}
