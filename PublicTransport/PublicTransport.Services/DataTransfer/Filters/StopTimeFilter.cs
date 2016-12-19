using System;
using PublicTransport.Services.Repositories;

namespace PublicTransport.Services.DataTransfer.Filters
{
    /// <summary>
    ///     Data transfer object interface.
    ///     Used by the <see cref="StopTimeRepository" /> to perform filtering.
    /// </summary>
    public class StopTimeFilter
    {
        /// <summary>
        ///     Stop id filter.
        /// </summary>
        public int StopId { get; set; }

        /// <summary>
        ///     Route id filter.
        /// </summary>
        public int RouteId { get; set; }

        /// <summary>
        ///     Date filter.
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        ///     Time filter.
        /// </summary>
        public TimeSpan? Time { get; set; }
    }
}