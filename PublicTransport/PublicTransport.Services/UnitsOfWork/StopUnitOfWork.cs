using System;
using System.Collections.Generic;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer.Filters;

namespace PublicTransport.Services.UnitsOfWork
{
    /// <summary>
    ///     Unit of work used to manage stop data.
    /// </summary>
    public class StopUnitOfWork : IDisposable
    {
        /// <summary>
        ///     Service used to fetch <see cref="Stop" /> data from the database.
        /// </summary>
        private readonly StopService _stopService;

        /// <summary>
        ///     Service used to fetch <see cref="Zone" /> data from the database.
        /// </summary>
        private readonly ZoneService _zoneService;

        /// <summary>
        ///     Service used to fetch <see cref="Street" /> data from the database.
        /// </summary>
        private readonly StreetService _streetService;

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
        public StopUnitOfWork()
        {
            _db = new PublicTransportContext();
            _stopService = new StopService(_db);
            _zoneService = new ZoneService(_db);
            _streetService = new StreetService(_db);
        }

        /// <summary>
        ///     Calls <see cref="StopService"/> create method.
        /// </summary>
        /// <param name="stop"><see cref="Stop"/> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="Stop"/> object successfully inserted into the database.
        /// </returns>
        public Stop CreateStop(Stop stop)
        {
            return _stopService.Create(stop);
        }

        /// <summary>
        ///     Calls <see cref="StopService"/> update method.
        /// </summary>
        /// <param name="stop"><see cref="Stop"/> object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="Stop"/> object successfully updated in the database.
        /// </returns>
        /// <exception cref="Exceptions.EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Stop" /> could not be found in the database.
        /// </exception>
        public Stop UpdateStop(Stop stop)
        {
            return _stopService.Update(stop);
        }

        /// <summary>
        ///     Calls <see cref="StopService"/> delete method.
        /// </summary>
        /// <param name="stop"><see cref="Stop"/> object to be deleted from the database.</param>
        /// <exception cref="Exceptions.EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Stop" /> could not be found in the database.
        /// </exception>
        public void DeleteStop(Stop stop)
        {
            _stopService.Delete(stop);
        }

        /// <summary>
        ///     Calls <see cref="StopService"/> filtering method.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="Stop"/> objects matching the filtering query.
        /// </returns>
        public List<Stop> FilterStops(IStopFilter filter)
        {
            return _stopService.FilterStops(filter);
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
        ///     Calls <see cref="StreetService"/> filtering method.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="Street"/> objects matching the filtering query.
        /// </returns>
        public List<Street> FilterStreets(IStreetFilter filter)
        {
            return _streetService.FilterStreets(filter);
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
