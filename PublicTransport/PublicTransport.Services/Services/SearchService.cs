using System.Collections.Generic;
using System.Linq;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.Contracts;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.DataTransfer.Converters;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Repositories;

namespace PublicTransport.Services
{
    /// <summary>
    ///     Service used to manage stop data.
    /// </summary>
    public class SearchService : ISearchService
    {
        /// <summary>
        ///     Service used to fetch <see cref="Stop" /> data from the database.
        /// </summary>
        private readonly StopRepository _stopRepository;

        /// <summary>
        ///     Service used to fetch <see cref="Route" /> data from the database.
        /// </summary>
        private readonly RouteRepository _routeRepository;

        /// <summary>
        ///     Used for converting <see cref="Stop" /> objects to <see cref="StopDto" /> objects and back.
        /// </summary>
        private readonly IConverter<Stop, StopDto> _stopConverter;

        /// <summary>
        ///     Used for converting <see cref="Route" /> objects to <see cref="RouteDto" /> objects and back.
        /// </summary>
        private readonly IConverter<Route, RouteDto> _routeConverter;

        /// <summary>
        ///     Database context common for services in this service used to access data.
        /// </summary>
        private readonly PublicTransportContext _db;

        /// <summary>
        ///     Determines whether the database context has been disposed.
        /// </summary>
        private bool _disposed;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public SearchService()
        {
            _db = new PublicTransportContext();

            _stopRepository = new StopRepository(_db);
            _routeRepository = new RouteRepository(_db);

            _stopConverter = new StopConverter();
            _routeConverter = new RouteConverter();
        }

        /// <summary>
        ///     Filters <see cref="Stop"/> objects using the supplied <see cref="StopFilter"/>.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="StopDto"/> objects matching the filtering query.
        /// </returns>
        public List<StopDto> FilterStops(StopFilter filter)
        {
            return _stopRepository.FilterStops(filter)
                .Select(_stopConverter.GetDto)
                .ToList();
        }

        /// <summary>
        ///     Finds <see cref="Route" /> objects using the supplied <see cref="RouteSearchFilter" />.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="RouteDto" /> objects matching the filtering query.
        /// </returns>
        public List<RouteDto> FindRoutes(RouteSearchFilter filter)
        {
            return _routeRepository.FindRoutes(filter)
                .Select(_routeConverter.GetDto)
                .ToList();
        }

        /// <summary>
        ///     Disposes the database context if not disposed already.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;
            _db.Dispose();
            _disposed = true;
        }
    }
}
