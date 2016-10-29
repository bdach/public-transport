namespace PublicTransport.Domain.Enums
{
    /// <summary>
    ///     Indicates the type of exception specified as an <see cref="Entities.CalendarDates" /> object.
    /// </summary>
    public enum ExceptionType
    {
        /// <summary>
        ///     Indicates that the service has been added for the specified date.
        /// </summary>
        Added,

        /// <summary>
        ///     Indicates that the service has been removed for the specified date.
        /// </summary>
        Removed
    }
}