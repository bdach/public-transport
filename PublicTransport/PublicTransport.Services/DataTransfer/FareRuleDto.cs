using System.Runtime.Serialization;

namespace PublicTransport.Services.DataTransfer
{
    /// <summary>
    /// Data transfer object for <see cref="Domain.Entities.FareRule"/> objects.
    /// </summary>
    [DataContract]
    public class FareRuleDto
    {
        /// <summary>
        /// FareRule ID.
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        ///     Contains the <see cref="RouteDto" /> associated with the rule.
        /// </summary>
        [DataMember]
        public RouteDto Route { get; set; }

        /// <summary>
        ///     Contains the origin <see cref="ZoneDto" /> associated with the rule.
        /// </summary>
        [DataMember]
        public ZoneDto Origin { get; set; }

        /// <summary>
        ///     Contains the destination <see cref="ZoneDto" /> associated with the rule.
        /// </summary>
        [DataMember]
        public ZoneDto Destination { get; set; }
    }
}