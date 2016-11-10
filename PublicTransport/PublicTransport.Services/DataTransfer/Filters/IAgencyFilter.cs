namespace PublicTransport.Services.DataTransfer.Filters
{
    /// <summary>
    ///     Data transfer object interface.
    ///     Used by the <see cref="AgencyService" /> to perform filtering.
    /// </summary>
    public interface IAgencyFilter
    {
        /// <summary>
        ///     Contains the agency name string filter parameter.
        /// </summary>
        string AgencyNameFilter { get; }

        /// <summary>
        ///     Contains the city name string filter parameter.
        /// </summary>
        string CityNameFilter { get; }

        /// <summary>
        ///     Contains the street name string filter parameter.
        /// </summary>
        string StreetNameFilter { get; }
    }
}