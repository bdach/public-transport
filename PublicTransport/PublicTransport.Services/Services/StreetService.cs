using System;
using System.Collections.Generic;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer.Filters;

namespace PublicTransport.Services
{
    public interface IStreetService : IDisposable
    {
        /// <summary>
        ///     Calls <see cref="StreetRepository"/> create method.
        /// </summary>
        /// <param name="street"><see cref="Street"/> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="Street"/> object successfully inserted into the database.
        /// </returns>
        Street CreateStreet(Street street);

        /// <summary>
        ///     Calls <see cref="StreetRepository"/> update method.
        /// </summary>
        /// <param name="street"><see cref="Street"/> object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="Street"/> object successfully updated in the database.
        /// </returns>
        /// <exception cref="Exceptions.EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Street" /> could not be found in the database.
        /// </exception>
        Street UpdateStreet(Street street);

        /// <summary>
        ///     Calls <see cref="StreetRepository"/> delete method.
        /// </summary>
        /// <param name="street"><see cref="Street"/> object to be deleted from the database.</param>
        /// <exception cref="Exceptions.EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Street" /> could not be found in the database.
        /// </exception>
        void DeleteStreet(Street street);

        /// <summary>
        ///     Calls <see cref="CityRepository"/> filtering method.
        /// </summary>
        /// <param name="name">Filtering parameter.</param>
        /// <returns>
        ///     List of <see cref="City"/> objects matching the filtering query.
        /// </returns>
        List<City> FilterCities(string name);

        /// <summary>
        ///     Calls <see cref="StreetRepository"/> filtering method.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="Street"/> objects matching the filtering query.
        /// </returns>
        List<Street> FilterStreets(IStreetFilter filter);
    }

    /// <summary>
    ///     Unit of work used to manage street data.
    /// </summary>
    public class StreetService : IStreetService
    {
        /// <summary>
        ///     Service used to fetch <see cref="City"/> data from the database.
        /// </summary>
        private readonly CityRepository _cityRepository;

        /// <summary>
        ///     Service used to fetch <see cref="Street"/> data from the database.
        /// </summary>
        private readonly StreetRepository _streetRepository;

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
        public StreetService()
        {
            _db = new PublicTransportContext();
            _cityRepository = new CityRepository(_db);
            _streetRepository = new StreetRepository(_db);
        }

        /// <summary>
        ///     Calls <see cref="StreetRepository"/> create method.
        /// </summary>
        /// <param name="street"><see cref="Street"/> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="Street"/> object successfully inserted into the database.
        /// </returns>
        public Street CreateStreet(Street street)
        {
            return _streetRepository.Create(street);
        }

        /// <summary>
        ///     Calls <see cref="StreetRepository"/> update method.
        /// </summary>
        /// <param name="street"><see cref="Street"/> object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="Street"/> object successfully updated in the database.
        /// </returns>
        /// <exception cref="Exceptions.EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Street" /> could not be found in the database.
        /// </exception>
        public Street UpdateStreet(Street street)
        {
            return _streetRepository.Update(street);
        }

        /// <summary>
        ///     Calls <see cref="StreetRepository"/> delete method.
        /// </summary>
        /// <param name="street"><see cref="Street"/> object to be deleted from the database.</param>
        /// <exception cref="Exceptions.EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Street" /> could not be found in the database.
        /// </exception>
        public void DeleteStreet(Street street)
        {
            _streetRepository.Delete(street);
        }

        /// <summary>
        ///     Calls <see cref="CityRepository"/> filtering method.
        /// </summary>
        /// <param name="name">Filtering parameter.</param>
        /// <returns>
        ///     List of <see cref="City"/> objects matching the filtering query.
        /// </returns>
        public List<City> FilterCities(string name)
        {
            return _cityRepository.GetCitiesContainingString(name);
        }

        /// <summary>
        ///     Calls <see cref="StreetRepository"/> filtering method.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="Street"/> objects matching the filtering query.
        /// </returns>
        public List<Street> FilterStreets(IStreetFilter filter)
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