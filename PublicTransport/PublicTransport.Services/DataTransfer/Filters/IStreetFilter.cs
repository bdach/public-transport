namespace PublicTransport.Services.DataTransfer.Filters
{
    /// <summary>
    ///     Data transfer object interface.
    ///     Used by the <see cref="StreetService" /> to perform filtering.
    /// </summary>
    public interface IStreetFilter
    {
        /// <summary>
        ///     Contains the street name filter string parameter.
        /// </summary>
        string StreetNameFilter { get; }

        /// <summary>
        ///     Contains the city name filter string parameter.
        /// </summary>
        string CityNameFilter { get; }
    }
}