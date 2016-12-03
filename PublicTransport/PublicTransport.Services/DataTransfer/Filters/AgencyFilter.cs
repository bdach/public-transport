using System.Runtime.Serialization;
using PublicTransport.Services.Repositories;

namespace PublicTransport.Services.DataTransfer.Filters
{
    /// <summary>
    ///     Data transfer object interface.
    ///     Used by the <see cref="AgencyRepository" /> to perform filtering.
    /// </summary>
    [DataContract]
    public class AgencyFilter
    {
        /// <summary>
        ///     Contains the agency name string filter parameter.
        /// </summary>
        [DataMember]
        public string AgencyNameFilter { get; set; }

        /// <summary>
        ///     Contains the city name string filter parameter.
        /// </summary>
        [DataMember]
        public string CityNameFilter { get; set; }

        /// <summary>
        ///     Contains the street name string filter parameter.
        /// </summary>
        [DataMember]
        public string StreetNameFilter { get; set; }
    }
}