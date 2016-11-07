namespace PublicTransport.Services.DataTransfer
{
    /// <summary>
    ///     Data transfer object interface.
    ///     Used by the <see cref="StopService" /> to perform filtering.
    /// </summary>
    public interface IStopFilter
    {
        /// <summary>
        ///     Contains the stop name string filter parameter.
        /// </summary>
        string StopNameFilter { get; set; }

        /// <summary>
        ///     Contains the street name string filter parameter.
        /// </summary>
        string StreetNameFilter { get; set; }

        /// <summary>
        ///     Contains the city name string filter parameter.
        /// </summary>
        string CityNameFilter { get; set; }

        /// <summary>
        ///     Contains the zone name string filter parameter.
        /// </summary>
        string ZoneNameFilter { get; set; }

        /// <summary>
        ///     Contains the parent station name string filter parameter.
        /// </summary>
        string ParentStationNameFilter { get; set; }

        /// <summary>
        ///     Limits the search query only to stops which are stations.
        /// </summary>
        bool OnlyStations { get; set; }
    }
}
