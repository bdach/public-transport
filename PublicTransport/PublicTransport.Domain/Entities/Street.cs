using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicTransport.Domain.Entities
{
    public class Street : Entity
    {
        [Required]
        public int CityId { get; set; }

        [ForeignKey("CityId")]
        public City City { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
