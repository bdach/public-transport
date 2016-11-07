using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PublicTransport.Domain.Enums;

namespace PublicTransport.Domain.Entities
{
    /// <summary>
    ///     Defines a fare class.
    /// </summary>
    public class FareAttribute : Entity
    {
        /// <summary>
        ///     Contains an ID that uniquely identifies a fare class.
        /// </summary>
        [Required]
        public int FareRuleId { get; set; }

        /// <summary>
        ///     Contains the associated fare class.
        /// </summary>
        [ForeignKey("FareRuleId")]
        public FareRule FareRule { get; set; }

        /// <summary>
        ///     Contains the fare price.
        /// </summary>
        [Required]
        public decimal Price { get; set; }

        /// <summary>
        ///     Specifies the number of transfers permitted on this fare. If empty, unlimited transfers are permitted.
        /// </summary>
        [Required]
        public TransferCount Transfers { get; set; }

        /// <summary>
        ///     Specifies a length of time in seconds before a transfer expires.
        ///     When used with a <see cref="Transfers" /> value of <see cref="TransferCount.None" />, the field indicates how long
        ///     a ticket is valid for a fare where no transfers are allowed.
        /// </summary>
        public int? TransferDuration { get; set; }
    }
}