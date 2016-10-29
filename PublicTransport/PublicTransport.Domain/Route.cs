using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PublicTransport.Domain.Enums;

namespace PublicTransport.Domain
{
    public class Route : Entity
    {
        [Required]
        public int AgencyId { get; set; }

        [ForeignKey("AgencyId")]
        public Agency Agency { get; set; }

        public string ShortName { get; set; }

        public string LongName { get; set; }

        public RouteType RouteType { get; set; }
    }
}
