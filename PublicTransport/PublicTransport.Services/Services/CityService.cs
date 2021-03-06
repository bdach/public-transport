﻿using System.Collections.Generic;
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
    ///     Service used to manage city data.
    /// </summary>
    public class CityService : ICityService
    {
        /// <summary>
        ///     Service used to fetch <see cref="City" /> data from the database.
        /// </summary>
        private readonly CityRepository _cityRepository;

        /// <summary>
        ///     Used for converting <see cref="City" /> objects to <see cref="CityDto" /> objects and back.
        /// </summary>
        private readonly IConverter<City, CityDto> _converter;

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
        public CityService()
        {
            _db = new PublicTransportContext();
            _cityRepository = new CityRepository(_db);
            _converter = new CityConverter();
        }

        /// <summary>
        ///     Creates a <see cref="City"/> object in the database.
        /// </summary>
        /// <param name="city"><see cref="CityDto" /> object containing <see cref="City"/> data.</param>
        /// <returns>
        ///     <see cref="CityDto" /> representing the inserted <see cref="City"/>.
        /// </returns>
        /// <exception cref="ValidationFaultException">
        ///     Thrown when the data contained in the received DTO contains validation errors.
        /// </exception>
        public CityDto CreateCity(CityDto city)
        {
            try
            {
                var result = _cityRepository.Create(_converter.GetEntity(city));
                return _converter.GetDto(result);
            }
            catch (DbEntityValidationException ex)
            {
                throw new ValidationFaultException(ex);
            }
        }

        /// <summary>
        ///     Updates a <see cref="City"/> object in the database, using the data stored in the
        ///     <see cref="CityDto" /> object.
        /// </summary>
        /// <param name="city"><see cref="CityDto" /> representing the object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="CityDto" /> object containing the updated <see cref="City"/> data.
        /// </returns>
        /// <exception cref="ValidationFaultException">
        ///     Thrown when the data contained in the received DTO contains validation errors.
        /// </exception>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="City"/> could not be found in the database.
        /// </exception>
        public CityDto UpdateCity(CityDto city)
        {
            try
            {
                var result = _cityRepository.Update(_converter.GetEntity(city));
                return _converter.GetDto(result);
            }
            catch (DbEntityValidationException ex)
            {
                throw new ValidationFaultException(ex);
            }
        }

        /// <summary>
        ///     Deletes a <see cref="City"/> from the system.
        /// </summary>
        /// <param name="city"><see cref="CityDto" /> representing the <see cref="City"/> to be deleted from the database.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the <see cref="City" /> could not be found in the database.
        /// </exception>
        public void DeleteCity(CityDto city)
        {
            _cityRepository.Delete(_converter.GetEntity(city));
        }

        /// <summary>
        ///     Filters <see cref="City" /> objects using the supplied string.
        /// </summary>
        /// <param name="name">String to filter cities by.</param>
        /// <returns>
        ///     List of <see cref="CityDto" /> objects matching the filtering query.
        /// </returns>
        public List<CityDto> FilterCities(string name)
        {
            return _cityRepository.GetCitiesContainingString(name)
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