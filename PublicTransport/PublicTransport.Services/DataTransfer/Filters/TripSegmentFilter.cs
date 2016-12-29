using System.Runtime.Serialization;

namespace PublicTransport.Services.DataTransfer.Filters
{
    [DataContract]
    public class TripSegmentFilter
    {
        /// <summary>
        ///     ID number of the <see cref="Domain.Entities.Trip" /> for which to perform the lookup.
        /// </summary>
        [DataMember]
        public int TripId { get; set; }

        /// <summary>
        ///     The lower bound for the <see cref="Domain.Entities.StopTime" /> sequence number.
        /// </summary>
        [DataMember]
        public int OriginSequenceNumber { get; set; }

        /// <summary>
        ///     The upper bound for the <see cref="Domain.Entities.StopTime" /> sequence number.
        /// </summary>
        [DataMember]
        public int DestinationSequenceNumber { get; set; }

        /// <summary>
        ///     Returns true if the filtering criteria are valid, false otherwise.
        /// </summary>
        public bool IsValid =>
            TripId > 0 &&
            OriginSequenceNumber >= 0 &&
            DestinationSequenceNumber >= 0 &&
            OriginSequenceNumber < DestinationSequenceNumber;
    }
}