using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.Contracts;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.DataTransfer.Converters;
using PublicTransport.Services.Exceptions;
using PublicTransport.Services.Repositories;

namespace PublicTransport.Services
{
    /// <summary>
    ///     Service used to manage zone data.
    /// </summary>
    public class ZoneService : IZoneService
    {
        /// <summary>
        ///     Service used to fetch <see cref="Zone"/> data from the database.
        /// </summary>
        private readonly ZoneRepository _zoneRepository;

        /// <summary>
        ///     Database context common for services in this service used to access data.
        /// </summary>
        private readonly PublicTransportContext _db;

        /// <summary>
        ///     Used for converting <see cref="Zone" /> objects to <see cref="ZoneDto" /> objects and back.
        /// </summary>
        private readonly IConverter<Zone, ZoneDto> _converter;

        /// <summary>
        ///     Determines whether the database context has been disposed.
        /// </summary>
        private bool _disposed;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public ZoneService()
        {
            _db = new PublicTransportContext();
            _zoneRepository = new ZoneRepository(_db);
            _converter = new ZoneConverter();
        }

        /// <summary>
        ///     Calls <see cref="ZoneRepository"/> create method.
        /// </summary>
        /// <param name="zone"><see cref="ZoneDto"/> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="ZoneDto"/> object successfully inserted into the database.
        /// </returns>
        /// <exception cref="ValidationFaultException">
        ///     Thrown when the entity has one or more validation errors.
        /// </exception>
        public ZoneDto CreateZone(ZoneDto zone)
        {
            try
            {
                var result = _zoneRepository.Create(_converter.GetEntity(zone));
                return _converter.GetDto(result);
            }
            catch (DbEntityValidationException ex)
            {
                throw new ValidationFaultException(ex);
            }
        }

        /// <summary>
        ///     Calls <see cref="ZoneRepository"/> update method.
        /// </summary>
        /// <param name="zone"><see cref="ZoneDto"/> object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="ZoneDto"/> object successfully updated in the database.
        /// </returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="ZoneDto" /> could not be found in the database.
        /// </exception>
        /// <exception cref="ValidationFaultException">
        ///     Thrown when the entity has one or more validation errors.
        /// </exception>
        public ZoneDto UpdateZone(ZoneDto zone)
        {
            try
            {
                var result = _zoneRepository.Update(_converter.GetEntity(zone));
                return _converter.GetDto(result);
            }
            catch (DbEntityValidationException ex)
            {
                throw new ValidationFaultException(ex);
            }
        }

        /// <summary>
        ///     Calls <see cref="ZoneRepository"/> delete method.
        /// </summary>
        /// <param name="zone"><see cref="ZoneDto"/> object to be deleted from the database.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="ZoneDto" /> could not be found in the database.
        /// </exception>
        public void DeleteZone(ZoneDto zone)
        {
            _zoneRepository.Delete(_converter.GetEntity(zone));
        }

        /// <summary>
        ///     Calls <see cref="ZoneRepository"/> filtering method.
        /// </summary>
        /// <param name="name">Filtering parameter.</param>
        /// <returns>
        ///     List of <see cref="ZoneDto"/> objects matching the filtering query.
        /// </returns>
        public List<ZoneDto> FilterZones(string name)
        {
            return _zoneRepository.GetZonesContainingString(name)
                .Select(_converter.GetDto)
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