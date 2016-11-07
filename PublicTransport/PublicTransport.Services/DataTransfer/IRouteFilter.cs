using PublicTransport.Domain.Enums;

namespace PublicTransport.Services.DataTransfer
{
    /// <summary>
    ///     Data transfer object interface.
    ///     Used by the <see cref="RouteService" /> to perform filtering.
    /// </summary>
    public interface IRouteFilter
    {
        /// <summary>
        ///     Agency name filter.
        /// </summary>
        string AgencyNameFilter { get; }

        /// <summary>
        ///     Route short name filter.
        /// </summary>
        string ShortNameFilter { get; }

        /// <summary>
        ///     Route long name filter.
        /// </summary>
        string LongNameFilter { get; }

        /// <summary>
        ///     Route type filter.
        /// </summary>
        RouteType? RouteTypeFilter { get; }
    }
}