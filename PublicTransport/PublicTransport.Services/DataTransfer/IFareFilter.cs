namespace PublicTransport.Services.DataTransfer
{
    /// <summary>
    ///     Data transfer object interface.
    ///     Used by the <see cref="FareAttributeService" /> to perform filtering.
    /// </summary>
    public interface IFareFilter
    {
        /// <summary>
        ///     Contains the route name string filter parameter.
        /// </summary>
        string RouteNameFilter { get; set; }

        /// <summary>
        ///     Contains the origin zone name string filter parameter.
        /// </summary>
        string OriginZoneNameFilter { get; set; }

        /// <summary>
        ///     Contains the destination zone name string filter parameter.
        /// </summary>
        string DestinationZoneNameFilter { get; set; }
    }
}
