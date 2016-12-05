using System.Runtime.Serialization;
using PublicTransport.Services.Repositories;

namespace PublicTransport.Services.DataTransfer.Filters
{
    /// <summary>
    ///     Data transfer object interface.
    ///     Used by the <see cref="StopRepository" /> to perform filtering.
    /// </summary>
    [DataContract]
    public class StopFilter
    {
        /// <summary>
        ///     Contains the stop name string filter parameter.
        /// </summary>
        [DataMember]
        public string StopNameFilter { get; set; }

        /// <summary>
        ///     Contains the street name string filter parameter.
        /// </summary>
        [DataMember]
        public string StreetNameFilter { get; set; }

        /// <summary>
        ///     Contains the city name string filter parameter.
        /// </summary>
        [DataMember]
        public string CityNameFilter { get; set; }

        /// <summary>
        ///     Contains the zone name string filter parameter.
        /// </summary>
        [DataMember]
        public string ZoneNameFilter { get; set; }

        /// <summary>
        ///     Contains the parent station name string filter parameter.
        /// </summary>
        [DataMember]
        public string ParentStationNameFilter { get; set; }

        /// <summary>
        ///     Limits the search query only to stops which are stations.
        /// </summary>
        [DataMember]
        public bool OnlyStations { get; set; }
    }
}
