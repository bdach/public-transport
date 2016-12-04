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

        /// <summary>
        ///     Used for converting <see cref="Domain.Entities.Stop" /> objects to <see cref="StopDto" /> objects and back.
        /// </summary>
        private readonly IConverter<Stop, StopDto> _stopConverter;

        /// <summary>
        ///     Used for converting <see cref="Domain.Entities.Zone" /> objects to <see cref="ZoneDto" /> objects and back.
        /// </summary>
        private readonly IConverter<Zone, ZoneDto> _zoneConverter;

        /// <summary>
        ///     Used for converting <see cref="Domain.Entities.Street" /> objects to <see cref="StreetDto" /> objects and back.
        /// </summary>
        private readonly IConverter<Street, StreetDto> _streetConverter;

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
            _stopConverter = new StopConverter();
            _zoneConverter = new ZoneConverter();
            _streetConverter = new StreetConverter();
        }

        /// <summary>
        ///     Calls <see cref="StopRepository"/> create method.
        /// </summary>
        /// <param name="stop"><see cref="Stop"/> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="Stop"/> object successfully inserted into the database.
        /// </returns>
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
        ///     Calls <see cref="StopRepository"/> update method.
        /// </summary>
        /// <param name="stop"><see cref="Stop"/> object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="Stop"/> object successfully updated in the database.
        /// </returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Stop" /> could not be found in the database.
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
        ///     Calls <see cref="StopRepository"/> delete method.
        /// </summary>
        /// <param name="stop"><see cref="Stop"/> object to be deleted from the database.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Stop" /> could not be found in the database.
        /// </exception>
        public void DeleteStop(StopDto stop)
        {
            _stopRepository.Delete(_stopConverter.GetEntity(stop));
        }

        /// <summary>
        ///     Calls <see cref="StopRepository"/> filtering method.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="Stop"/> objects matching the filtering query.
        /// </returns>
        public List<StopDto> FilterStops(StopFilter filter)
        {
            return _stopRepository.FilterStops(filter)
                .Select(_stopConverter.GetDto)
                .ToList();
        }

        /// <summary>
        ///     Calls <see cref="ZoneRepository"/> filtering method.
        /// </summary>
        /// <param name="name">Filtering parameter.</param>
        /// <returns>
        ///     List of <see cref="Zone"/> objects matching the filtering query.
        /// </returns>
        public List<ZoneDto> FilterZones(string name)
        {
            return _zoneRepository.GetZonesContainingString(name)
                .Select(_zoneConverter.GetDto)
                .ToList();
        }

        /// <summary>
        ///     Calls <see cref="StreetRepository"/> filtering method.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="Street"/> objects matching the filtering query.
        /// </returns>
        public List<StreetDto> FilterStreets(StreetFilter filter)
        {
            return _streetRepository.FilterStreets(filter)
                .Select(_streetConverter.GetDto)
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
