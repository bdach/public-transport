using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer.Filters;
using ReactiveUI;

namespace PublicTransport.Client.DataTransfer
{
    /// <summary>
    ///     Filtering object used in searching for <see cref="Route" /> objects.
    /// </summary>
    public class RouteSearchReactiveFilter : ReactiveObject, IReactiveFilter
    {
        /// <summary>
        ///     Origin stop  filter.
        /// </summary>
        private int _originStopIdFilter;

        /// <summary>
        ///     Destination stop  filter.
        /// </summary>
        private int _destinationStopIdFilter;

        /// <summary>
        ///     Origin stop  filter.
        /// </summary>
        public int OriginStopIdFilter
        {
            get { return _originStopIdFilter; }
            set { this.RaiseAndSetIfChanged(ref _originStopIdFilter, value); }
        }

        /// <summary>
        ///     Destination stop  filter.
        /// </summary>
        public int DestinationStopIdFilter
        {
            get { return _destinationStopIdFilter; }
            set { this.RaiseAndSetIfChanged(ref _destinationStopIdFilter, value); }
        }

        /// <summary>
        ///     Converts reactive filter to filter used by the service.
        /// </summary>
        /// <returns>Filter used by the service.</returns>
        public RouteSearchFilter Convert()
        {
            return new RouteSearchFilter
            {
                OriginStopIdFilter = OriginStopIdFilter,
                DestinationStopIdFilter = DestinationStopIdFilter
            };
        }

        /// <summary>
        ///     Determines whether the query is valid.
        /// </summary>
        public bool IsValid => OriginStopIdFilter > 0 && DestinationStopIdFilter > 0;
    }
}
