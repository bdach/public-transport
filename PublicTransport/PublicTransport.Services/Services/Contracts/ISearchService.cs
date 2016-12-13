using System;
using System.Collections.Generic;
using System.ServiceModel;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.DataTransfer.Filters;

namespace PublicTransport.Services.Contracts
{
    /// <summary>
    ///     Service interface specifying contract for searching routes.
    /// </summary>
    [ServiceContract]
    public interface ISearchService : IDisposable
    {
        /// <summary>
        ///     Filters <see cref="Stop"/> objects using the supplied <see cref="StopFilter"/>.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="StopDto"/> objects matching the filtering query.
        /// </returns>
        [OperationContract]
        List<StopDto> FilterStops(StopFilter filter);

        /// <summary>
        ///     Finds <see cref="Route" /> objects using the supplied <see cref="RouteSearchFilter" />.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="RouteDto" /> objects matching the filtering query.
        /// </returns>
        [OperationContract]
        List<RouteDto> FindRoutes(RouteSearchFilter filter);
    }
}
