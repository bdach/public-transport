using System;
using PublicTransport.Services.DataTransfer.Filters;
using ReactiveUI;

namespace PublicTransport.Client.DataTransfer
{
    /// <summary>
    ///     Filtering object used in searching for <see cref="Domain.Entities.StopTime" /> objects.
    /// </summary>
    public class StopTimeReactiveFilter : ReactiveObject, IReactiveFilter
    {
        /// <summary>
        ///     Stop id filter.
        /// </summary>
        private int _stopId;

        /// <summary>
        ///     Route id filter.
        /// </summary>
        private int _routeId;

        /// <summary>
        ///     Date filter.
        /// </summary>
        private DateTime? _date;

        /// <summary>
        ///     Time filter.
        /// </summary>
        private TimeSpan? _time;

        /// <summary>
        ///     Stop id filter.
        /// </summary>
        public int StopId
        {
            get { return _stopId; }
            set { this.RaiseAndSetIfChanged(ref _stopId, value); }
        }

        /// <summary>
        ///     Route id filter.
        /// </summary>
        public int RouteId
        {
            get { return _routeId; }
            set { this.RaiseAndSetIfChanged(ref _routeId, value); }
        }

        /// <summary>
        ///     Date filter.
        /// </summary>
        public DateTime? Date
        {
            get { return _date; }
            set { this.RaiseAndSetIfChanged(ref _date, value); }
        }

        /// <summary>
        ///     Time filter.
        /// </summary>
        public TimeSpan? Time
        {
            get { return _time; }
            set { this.RaiseAndSetIfChanged(ref _time, value); }
        }

        /// <summary>
        ///     Converts reactive filter to filter used by the service.
        /// </summary>
        /// <returns>Filter used by the service.</returns>
        public StopTimeFilter Convert()
        {
            return new StopTimeFilter
            {
                RouteId = RouteId,
                StopId = StopId,
                Date = Date,
                Time = Time
            };
        }

        /// <summary>
        ///     Determines whether the query is valid.
        /// </summary>
        public bool IsValid => true;
    }
}