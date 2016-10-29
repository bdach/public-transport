namespace PublicTransport.Domain.Enums
{
    /// <summary>
    ///     Describes the type of transportation used on a route.
    /// </summary>
    public enum RouteType
    {
        /// <summary>
        ///     Any light rail or street level system within a metropolitan area.
        /// </summary>
        Tram,

        /// <summary>
        ///     Any underground rail system within a metropolitan area.
        /// </summary>
        Subway,

        /// <summary>
        ///     Used for intercity or long-distance travel.
        /// </summary>
        Rail,

        /// <summary>
        ///     Used for short- and long-distance bus routes.
        /// </summary>
        Bus,

        /// <summary>
        ///     Used for short- and long-distance boat service.
        /// </summary>
        Ferry
    }
}