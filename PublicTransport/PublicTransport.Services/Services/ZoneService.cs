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
        ///     Used for converting <see cref="Zone" /> objects to <see cref="ZoneDto" /> objects and back.
        /// </summary>
        private readonly IConverter<Zone, ZoneDto> _converter;

        /// <summary>
        ///     Database context common for services in this service used to access data.
        /// </summary>
        private readonly PublicTransportContext _db;

        /// <summary>
        ///     Service used to fetch <see cref="Zone" /> data from the database.
        /// </summary>
        private readonly ZoneRepository _zoneRepository;

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
        ///     Creates a <see cref="Zone" /> object in the database.
        /// </summary>
        /// <param name="zone"><see cref="ZoneDto" /> object containing <see cref="Zone" /> data.</param>
        /// <returns>
        ///     <see cref="ZoneDto" /> representing the inserted <see cref="Zone" />.
        /// </returns>
        /// <exception cref="ValidationFaultException">
        ///     Thrown when the data contained in the received DTO contains validation errors.
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
        ///     Updates a <see cref="Zone" /> object in the database, using the data stored in the
        ///     <see cref="ZoneDto" /> object.
        /// </summary>
        /// <param name="zone"><see cref="ZoneDto" /> representing the object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="ZoneDto" /> object containing the updated <see cref="Zone" /> data.
        /// </returns>
        /// <exception cref="ValidationFaultException">
        ///     Thrown when the data contained in the received DTO contains validation errors.
        /// </exception>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Zone" /> could not be found in the database.
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
        ///     Deletes a <see cref="Zone" /> from the system.
        /// </summary>
        /// <param name="zone"><see cref="ZoneDto" /> representing the <see cref="Zone" /> to be deleted from the database.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the <see cref="Zone" /> could not be found in the database.
        /// </exception>
        public void DeleteZone(ZoneDto zone)
        {
            _zoneRepository.Delete(_converter.GetEntity(zone));
        }

        /// <summary>
        ///     Filters <see cref="Zone" /> objects using the supplied string.
        /// </summary>
        /// <param name="name">String to filter zones by.</param>
        /// <returns>
        ///     List of <see cref="ZoneDto" /> objects matching the filtering query.
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