﻿using PublicTransport.Services.DataTransfer.Filters;
using ReactiveUI;

namespace PublicTransport.Client.DataTransfer
{
    /// <summary>
    ///     Filtering object used in searching for <see cref="Domain.Entities.FareAttribute" /> objects.
    /// </summary>
    public class FareReactiveFilter : ReactiveObject, IReactiveFilter
    {
        /// <summary>
        ///     Route name filter.
        /// </summary>
        private string _routeNameFilter = "";

        /// <summary>
        ///     Origin zone name filter.
        /// </summary>
        private string _originZoneNameFilter = "";

        /// <summary>
        ///     Destination zone name filter.
        /// </summary>
        private string _destinationZoneNameFilter = "";

        /// <summary>
        ///     Route name filter.
        /// </summary>
        public string RouteNameFilter
        {
            get { return _routeNameFilter; }
            set { this.RaiseAndSetIfChanged(ref _routeNameFilter, value); }
        }

        /// <summary>
        ///     Origin zone name filter.
        /// </summary>
        public string OriginZoneNameFilter
        {
            get { return _originZoneNameFilter; }
            set { this.RaiseAndSetIfChanged(ref _originZoneNameFilter, value); }
        }

        /// <summary>
        ///     Destination zone name filter.
        /// </summary>
        public string DestinationZoneNameFilter
        {
            get { return _destinationZoneNameFilter; }
            set { this.RaiseAndSetIfChanged(ref _destinationZoneNameFilter, value); }
        }

        /// <summary>
        ///     Converts reactive filter to filter used by the service.
        /// </summary>
        /// <returns>Filter used by the service.</returns>
        public FareFilter Convert()
        {
            return new FareFilter
            {
                RouteNameFilter = RouteNameFilter,
                OriginZoneNameFilter = OriginZoneNameFilter,
                DestinationZoneNameFilter = DestinationZoneNameFilter
            };
        }

        /// <summary>
        ///     Determines whether the query is valid.
        /// </summary>
        public bool IsValid =>
            !string.IsNullOrWhiteSpace(RouteNameFilter) ||
            !string.IsNullOrWhiteSpace(OriginZoneNameFilter) ||
            !string.IsNullOrWhiteSpace(DestinationZoneNameFilter);
    }
}
