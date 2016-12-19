using System.Runtime.Serialization;
using PublicTransport.Domain.Enums;

namespace PublicTransport.Services.DataTransfer
{
    /// <summary>
    /// Data transfer object for <see cref="Domain.Entities.FareAttribute"/> objects.
    /// </summary>
    [DataContract]
    public class FareAttributeDto
    {
        /// <summary>
        /// FareAttribute ID.
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        ///     Contains the associated fare class.
        /// </summary>
        [DataMember]
        public FareRuleDto FareRule { get; set; }

        /// <summary>
        ///     Contains the fare price.
        /// </summary>
        [DataMember]
        public decimal Price { get; set; }

        /// <summary>
        ///     Specifies the number of transfers permitted on this fare. If empty, unlimited transfers are permitted.
        /// </summary>
        [DataMember]
        public TransferCount Transfers { get; set; }

        /// <summary>
        ///     Specifies a length of time in seconds before a transfer expires.
        ///     When used with a <see cref="Transfers" /> value of <see cref="TransferCount.None" />, the field indicates how long
        ///     a ticket is valid for a fare where no transfers are allowed.
        /// </summary>
        [DataMember]
        public int? TransferDuration { get; set; }
    }
}