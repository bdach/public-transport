using PublicTransport.Domain.Enums;
using PublicTransport.Services.DataTransfer;
using ReactiveUI;

namespace PublicTransport.Client.DataTransfer
{
    /// <summary>
    ///     Filtering object used in searching for <see cref="Domain.Entities.Route" /> objects.
    /// </summary>
    public class RouteFilter : ReactiveObject, IReactiveFilter, IRouteFilter
    {
        /// <summary>
        ///     Agency name filter.
        /// </summary>
        private string _agencyNameFilter = "";

        /// <summary>
        ///     Route long name filter.
        /// </summary>
        private string _longNameFilter = "";

        /// <summary>
        ///     Route short name filter.
        /// </summary>
        private string _shortNameFilter = "";

        /// <summary>
        ///     Route type filter.
        /// </summary>
        private RouteType? _routeTypeFilter;

        /// <summary>
        ///     Agency name filter.
        /// </summary>
        public string AgencyNameFilter
        {
            get { return _agencyNameFilter; }
            set { this.RaiseAndSetIfChanged(ref _agencyNameFilter, value); }
        }

        /// <summary>
        ///     Route long name filter.
        /// </summary>
        public string LongNameFilter
        {
            get { return _longNameFilter; }
            set { this.RaiseAndSetIfChanged(ref _longNameFilter, value); }
        }

        /// <summary>
        ///     Route short name filter.
        /// </summary>
        public string ShortNameFilter
        {
            get { return _shortNameFilter; }
            set { this.RaiseAndSetIfChanged(ref _shortNameFilter, value); }
        }

        /// <summary>
        ///     Route type filter.
        /// </summary>
        public RouteType? RouteTypeFilter
        {
            get { return _routeTypeFilter; }
            set { this.RaiseAndSetIfChanged(ref _routeTypeFilter, value); }
        }

        /// <summary>
        ///     Determines whether the query is valid.
        /// </summary>
        public bool IsValid =>
            !string.IsNullOrWhiteSpace(AgencyNameFilter) ||
            !string.IsNullOrWhiteSpace(LongNameFilter) ||
            !string.IsNullOrWhiteSpace(ShortNameFilter) ||
            RouteTypeFilter.HasValue;
    }
}