using System.Runtime.Serialization;
using PublicTransport.Domain.Enums;
using PublicTransport.Services.Repositories;

namespace PublicTransport.Services.DataTransfer.Filters
{
    /// <summary>
    ///     Data transfer object interface.
    ///     Used by the <see cref="RouteRepository" /> to perform filtering.
    /// </summary>
    [DataContract]
    public class RouteFilter
    {
        /// <summary>
        ///     Agency name filter.
        /// </summary>
        [DataMember]
        public string AgencyNameFilter { get; set; }

        /// <summary>
        ///     Route long name filter.
        /// </summary>
        [DataMember]
        public string LongNameFilter { get; set; }

        /// <summary>
        ///     Route short name filter.
        /// </summary>
        [DataMember]
        public string ShortNameFilter { get; set; }

        /// <summary>
        ///     Route type filter.
        /// </summary>
        [DataMember]
        public RouteType? RouteTypeFilter { get; set; }
    }
}