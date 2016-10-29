using System.ComponentModel.DataAnnotations;

namespace PublicTransport.Domain.Entities
{
    public class Zone : Entity
    {
        [Required]
        public string Name { get; set; }
    }
}
