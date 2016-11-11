using System;
using System.Collections.Generic;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;

namespace PublicTransport.Services.UnitsOfWork
{
    /// <summary>
    ///     Unit of work used to manage city data.
    /// </summary>
    public class CityUnitOfWork : IDisposable
    {
        /// <summary>
        ///     Service used to fetch <see cref="City"/> data from the database.
        /// </summary>
        private readonly CityService _cityService;

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
        public CityUnitOfWork()
        {
            _db = new PublicTransportContext();
            _cityService = new CityService(_db);
        }

        /// <summary>
        ///     Calls <see cref="CityService"/> create method.
        /// </summary>
        /// <param name="city"><see cref="City"/> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="City"/> object successfully inserted into the database.
        /// </returns>
        public City CreateCity(City city)
        {
            return _cityService.Create(city);
        }

        /// <summary>
        ///     Calls <see cref="CityService"/> update method.
        /// </summary>
        /// <param name="city"><see cref="City"/> object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="City"/> object successfully updated in the database.
        /// </returns>
        /// <exception cref="Exceptions.EntryNotFoundException">
        ///     Thrown when the supplied <see cref="City" /> could not be found in the database.
        /// </exception>
        public City UpdateCity(City city)
        {
            return _cityService.Update(city);
        }

        /// <summary>
        ///     Calls <see cref="CityService"/> delete method.
        /// </summary>
        /// <param name="city"><see cref="City"/> object to be deleted from the database.</param>
        /// <exception cref="Exceptions.EntryNotFoundException">
        ///     Thrown when the supplied <see cref="City" /> could not be found in the database.
        /// </exception>
        public void DeleteCity(City city)
        {
            _cityService.Delete(city);
        }

        /// <summary>
        ///     Calls <see cref="CityService"/> filtering method.
        /// </summary>
        /// <param name="name">Filtering parameter.</param>
        /// <returns>
        ///     List of <see cref="City"/> objects matching the filtering query.
        /// </returns>
        public List<City> FilterCities(string name)
        {
            return _cityService.GetCitiesContainingString(name);
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
