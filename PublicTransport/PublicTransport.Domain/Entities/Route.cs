using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PublicTransport.Domain.Enums;

namespace PublicTransport.Domain.Entities
{
    public class Route : Entity
    {
        [Required]
        public int AgencyId { get; set; }

        [ForeignKey("AgencyId")]
        public Agency Agency { get; set; }

        [Required]
        public string ShortName { get; set; }

        [Required]
        public string LongName { get; set; }

        [Required]
        public RouteType RouteType { get; set; }
    }
}
