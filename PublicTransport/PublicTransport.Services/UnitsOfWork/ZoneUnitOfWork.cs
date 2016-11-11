using System;
using System.Collections.Generic;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;

namespace PublicTransport.Services.UnitsOfWork
{
    /// <summary>
    ///     Unit of work used to manage zone data.
    /// </summary>
    public class ZoneUnitOfWork : IDisposable
    {
        /// <summary>
        ///     Service used to fetch <see cref="Zone"/> data from the database.
        /// </summary>
        private readonly ZoneService _zoneService;

        /// <summary>
        ///     Database context common for services in this unit of work used to access data.
        /// </summary>
        private readonly PublicTransportContext _db;

        /// <summary>
        ///     Determines whether the database context has been disposed.
        /// </summary>
        private bool _disposed;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public ZoneUnitOfWork()
        {
            _db = new PublicTransportContext();
            _zoneService = new ZoneService(_db);
        }

        /// <summary>
        ///     Calls <see cref="ZoneService"/> create method.
        /// </summary>
        /// <param name="zone"><see cref="Zone"/> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="Zone"/> object successfully inserted into the database.
        /// </returns>
        public Zone CreateZone(Zone zone)
        {
            return _zoneService.Create(zone);
        }

        /// <summary>
        ///     Calls <see cref="ZoneService"/> update method.
        /// </summary>
        /// <param name="zone"><see cref="Zone"/> object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="Zone"/> object successfully updated in the database.
        /// </returns>
        /// <exception cref="Exceptions.EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Zone" /> could not be found in the database.
        /// </exception>
        public Zone UpdateZone(Zone zone)
        {
            return _zoneService.Update(zone);
        }

        /// <summary>
        ///     Calls <see cref="ZoneService"/> delete method.
        /// </summary>
        /// <param name="zone"><see cref="Zone"/> object to be deleted from the database.</param>
        /// <exception cref="Exceptions.EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Zone" /> could not be found in the database.
        /// </exception>
        public void DeleteZone(Zone zone)
        {
            _zoneService.Delete(zone);
        }

        /// <summary>
        ///     Calls <see cref="ZoneService"/> filtering method.
        /// </summary>
        /// <param name="name">Filtering parameter.</param>
        /// <returns>
        ///     List of <see cref="Zone"/> objects matching the filtering query.
        /// </returns>
        public List<Zone> FilterZones(string name)
        {
            return _zoneService.GetZonesContainingString(name);
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