namespace PublicTransport.Domain.Enums
{
    /// <summary>
    ///     Specifies the number of transfers permitted on a fare.
    /// </summary>
    public enum TransferCount
    {
        /// <summary>
        ///     No transfers permitted on this fare.
        /// </summary>
        None,

        /// <summary>
        ///     Passengers may transfer once.
        /// </summary>
        One,

        /// <summary>
        ///     Passenger may transfer twice.
        /// </summary>
        Two
    }
}