using System;
using PublicTransport.Services.DataTransfer;
using ReactiveUI;

namespace PublicTransport.Client.DataTransfer
{
    public class StopTimeFilter : ReactiveObject, IStopTimeFilter, IReactiveFilter
    {
        private int _stopId;
        private int _routeId;
        private DateTime? _date;
        private TimeSpan? _time;

        public int StopId
        {
            get { return _stopId; }
            set { this.RaiseAndSetIfChanged(ref _stopId, value); }
        }

        public int RouteId
        {
            get { return _routeId; }
            set { this.RaiseAndSetIfChanged(ref _routeId, value); }
        }

        public DateTime? Date
        {
            get { return _date; }
            set { this.RaiseAndSetIfChanged(ref _date, value); }
        }

        public TimeSpan? Time
        {
            get { return _time; }
            set { this.RaiseAndSetIfChanged(ref _time, value); }
        }

        public bool IsValid => true;
    }
}