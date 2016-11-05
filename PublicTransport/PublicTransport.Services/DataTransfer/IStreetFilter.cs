namespace PublicTransport.Services.DataTransfer
{
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