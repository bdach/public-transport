using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.DataTransfer.Converters;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Exceptions;
using PublicTransport.Services.Interfaces;

namespace PublicTransport.Services
{
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

        private readonly IConverter<City, CityDto> _cityConverter;
        private readonly IConverter<Street, StreetDto> _streetConverter;

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
            _cityConverter = new CityConverter();
            _streetConverter = new StreetConverter();
        }

        /// <summary>
        ///     Calls <see cref="StreetRepository"/> create method.
        /// </summary>
        /// <param name="street"><see cref="Street"/> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="Street"/> object successfully inserted into the database.
        /// </returns>
        public StreetDto CreateStreet(StreetDto street)
        {
            try
            {
                var result = _streetRepository.Create(_streetConverter.GetEntity(street));
                return _streetConverter.GetDto(result);
            }
            catch (DbEntityValidationException ex)
            {
                throw new ValidationFaultException(ex);
            }
        }

        /// <summary>
        ///     Calls <see cref="StreetRepository"/> update method.
        /// </summary>
        /// <param name="street"><see cref="Street"/> object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="Street"/> object successfully updated in the database.
        /// </returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Street" /> could not be found in the database.
        /// </exception>
        public StreetDto UpdateStreet(StreetDto street)
        {
            try
            {
                var result = _streetRepository.Update(_streetConverter.GetEntity(street));
                return _streetConverter.GetDto(result);
            }
            catch (DbEntityValidationException ex)
            {
                throw new ValidationFaultException(ex);
            }
        }

        /// <summary>
        ///     Calls <see cref="StreetRepository"/> delete method.
        /// </summary>
        /// <param name="street"><see cref="Street"/> object to be deleted from the database.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Street" /> could not be found in the database.
        /// </exception>
        public void DeleteStreet(StreetDto street)
        {
            _streetRepository.Delete(_streetConverter.GetEntity(street));
        }

        /// <summary>
        ///     Calls <see cref="CityRepository"/> filtering method.
        /// </summary>
        /// <param name="name">Filtering parameter.</param>
        /// <returns>
        ///     List of <see cref="City"/> objects matching the filtering query.
        /// </returns>
        public List<CityDto> FilterCities(string name)
        {
            return _cityRepository.GetCitiesContainingString(name)
                .Select(c => _cityConverter.GetDto(c))
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
                .Select(s => _streetConverter.GetDto(s))
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