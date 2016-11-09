using System;
using PublicTransport.Services.DataTransfer;
using ReactiveUI;

namespace PublicTransport.Client.DataTransfer
{
    /// <summary>
    ///     Filtering object used in searching for <see cref="Domain.Entities.StopTime" /> objects.
    /// </summary>
    public class StopTimeFilter : ReactiveObject, IStopTimeFilter, IReactiveFilter
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
        ///     Determines whether the query is valid.
        /// </summary>
        public bool IsValid => true;
    }
}