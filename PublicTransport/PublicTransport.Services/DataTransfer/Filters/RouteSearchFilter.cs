using PublicTransport.Services.Repositories;

namespace PublicTransport.Services.DataTransfer.Filters
{
    /// <summary>
    ///     Data transfer object interface.
    ///     Used by the <see cref="RouteRepository" /> to perform filtering.
    /// </summary>
    public class RouteSearchFilter
    {
        /// <summary>
        ///     Origin stop name filter.
        /// </summary>
        public int OriginStopIdFilter { get; set; }

        /// <summary>
        ///     Destination stop name filter.
        /// </summary>
        public int DestinationStopIdFilter { get; set; }

        /// <summary>
        ///     Determines whether the query is valid.
        /// </summary>
        public bool IsValid => OriginStopIdFilter > 0 && DestinationStopIdFilter > 0;
    }
}
