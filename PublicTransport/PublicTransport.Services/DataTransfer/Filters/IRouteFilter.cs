using PublicTransport.Domain.Enums;
using PublicTransport.Services.Repositories;

namespace PublicTransport.Services.DataTransfer.Filters
{
    /// <summary>
    ///     Data transfer object interface.
    ///     Used by the <see cref="RouteRepository" /> to perform filtering.
    /// </summary>
    public interface IRouteFilter
    {
        /// <summary>
        ///     Agency name filter.
        /// </summary>
        string AgencyNameFilter { get; }

        /// <summary>
        ///     Route long name filter.
        /// </summary>
        string LongNameFilter { get; }

        /// <summary>
        ///     Route short name filter.
        /// </summary>
        string ShortNameFilter { get; }

        /// <summary>
        ///     Route type filter.
        /// </summary>
        RouteType? RouteTypeFilter { get; }
    }
}