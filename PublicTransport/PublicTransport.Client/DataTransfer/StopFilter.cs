using PublicTransport.Services.DataTransfer.Filters;
using ReactiveUI;

namespace PublicTransport.Client.DataTransfer
{
    /// <summary>
    ///     Filtering object used in searching for <see cref="Domain.Entities.Stop" /> objects.
    /// </summary>
    public class StopFilter : ReactiveObject, IStopFilter, IReactiveFilter
    {
        /// <summary>
        ///     Stop name filter.
        /// </summary>
        private string _stopNameFilter = "";

        /// <summary>
        ///     City name filter.
        /// </summary>
        private string _cityNameFilter = "";

        /// <summary>
        ///     Street name filter.
        /// </summary>
        private string _streetNameFilter = "";

        /// <summary>
        ///     Zone name filter.
        /// </summary>
        private string _zoneNameFilter = "";

        /// <summary>
        ///     Parent station name filter.
        /// </summary>
        private string _parentStationNameFilter = "";

        /// <summary>
        ///     Limits the search query only to stops which are stations.
        /// </summary>
        private bool _onlyStations;

        /// <summary>
        ///     Stop name filter.
        /// </summary>
        public string StopNameFilter
        {
            get { return _stopNameFilter; }
            set { this.RaiseAndSetIfChanged(ref _stopNameFilter, value); }
        }

        /// <summary>
        ///     City name filter.
        /// </summary>
        public string CityNameFilter
        {
            get { return _cityNameFilter; }
            set { this.RaiseAndSetIfChanged(ref _cityNameFilter, value); }
        }

        /// <summary>
        ///     Street name filter.
        /// </summary>
        public string StreetNameFilter
        {
            get { return _streetNameFilter; }
            set { this.RaiseAndSetIfChanged(ref _streetNameFilter, value); }
        }

        /// <summary>
        ///     Zone name filter.
        /// </summary>
        public string ZoneNameFilter
        {
            get { return _zoneNameFilter; }
            set { this.RaiseAndSetIfChanged(ref _zoneNameFilter, value); }
        }

        /// <summary>
        ///     Parent station name filter.
        /// </summary>
        public string ParentStationNameFilter
        {
            get { return _parentStationNameFilter; }
            set { this.RaiseAndSetIfChanged(ref _parentStationNameFilter, value); }
        }

        /// <summary>
        ///     Limits the search query only to stops which are stations.
        /// </summary>
        public bool OnlyStations
        {
            get { return _onlyStations; }
            set { this.RaiseAndSetIfChanged(ref _onlyStations, value); }
        }

        // TODO: allow filtering by IsStation

        /// <summary>
        ///     Determines whether the query is valid.
        /// </summary>
        public bool IsValid =>
            !string.IsNullOrWhiteSpace(StopNameFilter) ||
            !string.IsNullOrWhiteSpace(StreetNameFilter) ||
            !string.IsNullOrWhiteSpace(CityNameFilter) ||
            !string.IsNullOrWhiteSpace(ZoneNameFilter) ||
            !string.IsNullOrWhiteSpace(ParentStationNameFilter);
    }
}
