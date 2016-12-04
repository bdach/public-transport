using PublicTransport.Services.Repositories;

namespace PublicTransport.Services.DataTransfer.Filters
{
    /// <summary>
    ///     Data transfer object interface.
    ///     Used by the <see cref="FareAttributeRepository" /> to perform filtering.
    /// </summary>
    public class FareFilter
    {
        /// <summary>
        ///     Contains the route name string filter parameter.
        /// </summary>
        public string RouteNameFilter { get; set; }

        /// <summary>
        ///     Contains the origin zone name string filter parameter.
        /// </summary>
        public string OriginZoneNameFilter { get; set; }

        /// <summary>
        ///     Contains the destination zone name string filter parameter.
        /// </summary>
        public string DestinationZoneNameFilter { get; set; }
    }
}
