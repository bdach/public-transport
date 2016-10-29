using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PublicTransport.Domain.Enums;

namespace PublicTransport.Domain.Entities
{
    public class FareAttribute : Entity
    {
        [Required]
        public int FareRuleId { get; set; }

        [ForeignKey("FareRuleId")]
        public FareRule FareRule { get; set; }

        [Required]
        public decimal Price { get; set; }
        
        public TransferCount? Transfers { get; set; }
        
        public int? TransferDuration { get; set; }
    }
}
