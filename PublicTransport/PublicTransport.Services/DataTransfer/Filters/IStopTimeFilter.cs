using System;
using PublicTransport.Services.Repositories;

namespace PublicTransport.Services.DataTransfer.Filters
{
    /// <summary>
    ///     Data transfer object interface.
    ///     Used by the <see cref="StopTimeRepository" /> to perform filtering.
    /// </summary>
    public interface IStopTimeFilter
    {
        /// <summary>
        ///     Stop id filter.
        /// </summary>
        int StopId { get; }

        /// <summary>
        ///     Route id filter.
        /// </summary>
        int RouteId { get; }

        /// <summary>
        ///     Date filter.
        /// </summary>
        DateTime? Date { get; }

        /// <summary>
        ///     Time filter.
        /// </summary>
        TimeSpan? Time { get; }
    }
}