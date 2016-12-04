using System;
using System.Collections.Generic;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Exceptions;
using PublicTransport.Services.Repositories;

namespace PublicTransport.Services
{
    public interface IStopService : IDisposable
    {
        /// <summary>
        ///     Calls <see cref="StopRepository"/> create method.
        /// </summary>
        /// <param name="stop"><see cref="Stop"/> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="Stop"/> object successfully inserted into the database.
        /// </returns>
        Stop CreateStop(Stop stop);

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
        Stop UpdateStop(Stop stop);

        /// <summary>
        ///     Calls <see cref="StopRepository"/> delete method.
        /// </summary>
        /// <param name="stop"><see cref="Stop"/> object to be deleted from the database.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Stop" /> could not be found in the database.
        /// </exception>
        void DeleteStop(Stop stop);

        /// <summary>
        ///     Calls <see cref="StopRepository"/> filtering method.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="Stop"/> objects matching the filtering query.
        /// </returns>
        List<Stop> FilterStops(StopFilter filter);

        /// <summary>
        ///     Calls <see cref="ZoneRepository"/> filtering method.
        /// </summary>
        /// <param name="name">Filtering parameter.</param>
        /// <returns>
        ///     List of <see cref="Zone"/> objects matching the filtering query.
        /// </returns>
        List<Zone> FilterZones(string name);

        /// <summary>
        ///     Calls <see cref="StreetRepository"/> filtering method.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="Street"/> objects matching the filtering query.
        /// </returns>
        List<Street> FilterStreets(StreetFilter filter);
    }

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
        }

        /// <summary>
        ///     Calls <see cref="StopRepository"/> create method.
        /// </summary>
        /// <param name="stop"><see cref="Stop"/> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="Stop"/> object successfully inserted into the database.
        /// </returns>
        public Stop CreateStop(Stop stop)
        {
            return _stopRepository.Create(stop);
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
        public Stop UpdateStop(Stop stop)
        {
            return _stopRepository.Update(stop);
        }

        /// <summary>
        ///     Calls <see cref="StopRepository"/> delete method.
        /// </summary>
        /// <param name="stop"><see cref="Stop"/> object to be deleted from the database.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Stop" /> could not be found in the database.
        /// </exception>
        public void DeleteStop(Stop stop)
        {
            _stopRepository.Delete(stop);
        }

        /// <summary>
        ///     Calls <see cref="StopRepository"/> filtering method.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="Stop"/> objects matching the filtering query.
        /// </returns>
        public List<Stop> FilterStops(StopFilter filter)
        {
            return _stopRepository.FilterStops(filter);
        }

        /// <summary>
        ///     Calls <see cref="ZoneRepository"/> filtering method.
        /// </summary>
        /// <param name="name">Filtering parameter.</param>
        /// <returns>
        ///     List of <see cref="Zone"/> objects matching the filtering query.
        /// </returns>
        public List<Zone> FilterZones(string name)
        {
            return _zoneRepository.GetZonesContainingString(name);
        }

        /// <summary>
        ///     Calls <see cref="StreetRepository"/> filtering method.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="Street"/> objects matching the filtering query.
        /// </returns>
        public List<Street> FilterStreets(StreetFilter filter)
        {
            return _streetRepository.FilterStreets(filter);
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
