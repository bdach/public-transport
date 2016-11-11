using System;
using System.Collections.Generic;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer.Filters;

namespace PublicTransport.Services.UnitsOfWork
{
    public interface IStreetUnitOfWork : IDisposable
    {
        /// <summary>
        ///     Calls <see cref="StreetService"/> create method.
        /// </summary>
        /// <param name="street"><see cref="Street"/> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="Street"/> object successfully inserted into the database.
        /// </returns>
        Street CreateStreet(Street street);

        /// <summary>
        ///     Calls <see cref="StreetService"/> update method.
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
        ///     Calls <see cref="StreetService"/> delete method.
        /// </summary>
        /// <param name="street"><see cref="Street"/> object to be deleted from the database.</param>
        /// <exception cref="Exceptions.EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Street" /> could not be found in the database.
        /// </exception>
        void DeleteStreet(Street street);

        /// <summary>
        ///     Calls <see cref="CityService"/> filtering method.
        /// </summary>
        /// <param name="name">Filtering parameter.</param>
        /// <returns>
        ///     List of <see cref="City"/> objects matching the filtering query.
        /// </returns>
        List<City> FilterCities(string name);

        /// <summary>
        ///     Calls <see cref="StreetService"/> filtering method.
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
    public class StreetUnitOfWork : IStreetUnitOfWork
    {
        /// <summary>
        ///     Service used to fetch <see cref="City"/> data from the database.
        /// </summary>
        private readonly CityService _cityService;

        /// <summary>
        ///     Service used to fetch <see cref="Street"/> data from the database.
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
        public StreetUnitOfWork()
        {
            _db = new PublicTransportContext();
            _cityService = new CityService(_db);
            _streetService = new StreetService(_db);
        }

        /// <summary>
        ///     Calls <see cref="StreetService"/> create method.
        /// </summary>
        /// <param name="street"><see cref="Street"/> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="Street"/> object successfully inserted into the database.
        /// </returns>
        public Street CreateStreet(Street street)
        {
            return _streetService.Create(street);
        }

        /// <summary>
        ///     Calls <see cref="StreetService"/> update method.
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
            return _streetService.Update(street);
        }

        /// <summary>
        ///     Calls <see cref="StreetService"/> delete method.
        /// </summary>
        /// <param name="street"><see cref="Street"/> object to be deleted from the database.</param>
        /// <exception cref="Exceptions.EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Street" /> could not be found in the database.
        /// </exception>
        public void DeleteStreet(Street street)
        {
            _streetService.Delete(street);
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