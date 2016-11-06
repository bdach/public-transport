namespace PublicTransport.Client.DataTransfer
{
    /// <summary>
    ///     Common interface used for filter objects used in the ReactiveUI frontend.
    /// </summary>
    public interface IReactiveFilter
    {
        /// <summary>
        ///     Determines whether a query is valid.
        /// </summary>
        bool IsValid { get; }
    }
}