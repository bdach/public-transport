using System.ComponentModel.DataAnnotations;

namespace PublicTransport.Domain.Entities
{
    public class Agency : Entity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Phone { get; set; }
        
        public string Url { get; set; }
        
        public string Regon { get; set; }
    }
}
