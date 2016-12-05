using System.Runtime.Serialization;
using PublicTransport.Services.Repositories;

namespace PublicTransport.Services.DataTransfer.Filters
{
    /// <summary>
    ///     Data transfer object interface.
    ///     Used by the <see cref="StreetRepository" /> to perform filtering.
    /// </summary>
    [DataContract]
    public class StreetFilter
    {
        /// <summary>
        ///     Contains the street name filter string parameter.
        /// </summary>
        [DataMember]
        public string StreetNameFilter { get; set; }

        /// <summary>
        ///     Contains the city name filter string parameter.
        /// </summary>
        [DataMember]
        public string CityNameFilter { get; set; }
    }
}