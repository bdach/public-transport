using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.Contracts;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.DataTransfer.Converters;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Exceptions;
using PublicTransport.Services.Repositories;

namespace PublicTransport.Services
{
    /// <summary>
    ///     Service used to manage stop data.
    /// </summary>
    public class StopService : IStopService
    {
        /// <summary>
        ///     Service used to fetch <see cref="Stop" /> data from the database.
        /// </summary>
        private readonly StopRepository _stopRepository;

        /// <summary>
        ///     Service used to fetch <see cref="Zone" /> data from the database.
        /// </summary>
        private readonly ZoneRepository _zoneRepository;

        /// <summary>
        ///     Service used to fetch <see cref="Street" /> data from the database.
        /// </summary>
        private readonly StreetRepository _streetRepository;

        private readonly StopTimeRepository _stopTimeRepository;

        /// <summary>
        ///     Used for converting <see cref="Stop" /> objects to <see cref="StopDto" /> objects and back.
        /// </summary>
        private readonly IConverter<Stop, StopDto> _stopConverter;

        /// <summary>
        ///     Used for converting <see cref="Zone" /> objects to <see cref="ZoneDto" /> objects and back.
        /// </summary>
        private readonly IConverter<Zone, ZoneDto> _zoneConverter;

        /// <summary>
        ///     Used for converting <see cref="Street" /> objects to <see cref="StreetDto" /> objects and back.
        /// </summary>
        private readonly IConverter<Street, StreetDto> _streetConverter;

        private readonly IConverter<Route, RouteDto> _routeConverter;
        private readonly IConverter<StopTime, StopTimeDto> _stopTimeConverter;

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
        public StopService()
        {
            _db = new PublicTransportContext();

            _stopRepository = new StopRepository(_db);
            _zoneRepository = new ZoneRepository(_db);
            _streetRepository = new StreetRepository(_db);
            _stopTimeRepository = new StopTimeRepository(_db);

            _stopConverter = new StopConverter();
            _zoneConverter = new ZoneConverter();
            _streetConverter = new StreetConverter();
            _stopTimeConverter = new StopTimeConverter();
            _routeConverter = new RouteConverter();
        }

        /// <summary>
        ///     Creates a <see cref="Stop"/> object in the database.
        /// </summary>
        /// <param name="stop"><see cref="StopDto" /> object containing <see cref="Stop"/> data.</param>
        /// <returns>
        ///     <see cref="StopDto" /> representing the inserted <see cref="Stop"/>.
        /// </returns>
        /// <exception cref="ValidationFaultException">
        ///     Thrown when the data contained in the received DTO contains validation errors.
        /// </exception>
        public StopDto CreateStop(StopDto stop)
        {
            try
            {
                var result = _stopRepository.Create(_stopConverter.GetEntity(stop));
                return _stopConverter.GetDto(result);
            }
            catch (DbEntityValidationException ex)
            {
                throw new ValidationFaultException(ex);
            }
        }

        /// <summary>
        ///     Updates a <see cref="Stop"/> object in the database, using the data stored in the
        ///     <see cref="StopDto" /> object.
        /// </summary>
        /// <param name="stop"><see cref="StopDto" /> representing the object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="StopDto" /> object containing the updated <see cref="Stop"/> data.
        /// </returns>
        /// <exception cref="ValidationFaultException">
        ///     Thrown when the data contained in the received DTO contains validation errors.
        /// </exception>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Stop"/> could not be found in the database.
        /// </exception>
        public StopDto UpdateStop(StopDto stop)
        {
            try
            {
                var result = _stopRepository.Update(_stopConverter.GetEntity(stop));
                return _stopConverter.GetDto(result);
            }
            catch (DbEntityValidationException ex)
            {
                throw new ValidationFaultException(ex);
            }
        }

        /// <summary>
        ///     Deletes a <see cref="Stop"/> from the system.
        /// </summary>
        /// <param name="stop"><see cref="StopDto" /> representing the <see cref="Stop"/> to be deleted from the database.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the <see cref="Stop" /> could not be found in the database.
        /// </exception>
        public void DeleteStop(StopDto stop)
        {
            _stopRepository.Delete(_stopConverter.GetEntity(stop));
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
        ///     Filters <see cref="Zone"/> objects using the supplied string.
        /// </summary>
        /// <param name="name">String to filter zones by.</param>
        /// <returns>
        ///     List of <see cref="ZoneDto"/> objects matching the filtering query.
        /// </returns>
        public List<ZoneDto> FilterZones(string name)
        {
            return _zoneRepository.GetZonesContainingString(name)
                .Select(_zoneConverter.GetDto)
                .ToList();
        }

        /// <summary>
        ///     Filters <see cref="Street" /> objects using the supplied <see cref="StreetFilter" />.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="StreetDto" /> objects matching the filtering query.
        /// </returns>
        public List<StreetDto> FilterStreets(StreetFilter filter)
        {
            return _streetRepository.FilterStreets(filter)
                .Select(_streetConverter.GetDto)
                .ToList();
        }

        /// <summary>
        ///     Fetches a timetable for the <see cref="Stop"/> with the given ID.
        /// </summary>
        /// <param name="stopId">ID of the <see cref="Stop"/> the timetable should be displayed for.</param>
        /// <returns>
        ///     A dictionary indexed by <see cref="RouteDto"/> objects, whose values are lists of <see cref="StopTimeDto"/>s.
        ///     The values represent the times of arrival/departure of the given route for the selected stop.
        /// </returns>
        public Dictionary<RouteDto, List<StopTimeDto>> GetStopTimetable(int stopId)
        {
            return _stopTimeRepository.GetFullTimetableByStopId(stopId)
                .ToDictionary(kv => _routeConverter.GetDto(kv.Key),
                    kv => kv.Value.Select(st => _stopTimeConverter.GetDto(st)).ToList());
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
