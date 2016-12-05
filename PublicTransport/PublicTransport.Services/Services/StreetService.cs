using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.Contracts;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.DataTransfer.Converters;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Exceptions;
using PublicTransport.Services.Repositories;

namespace PublicTransport.Services
{
    /// <summary>
    ///     Service used to manage street data.
    /// </summary>
    public class StreetService : IStreetService
    {
        /// <summary>
        ///     Used for converting <see cref="City" /> objects to <see cref="CityDto" /> objects and back.
        /// </summary>
        private readonly IConverter<City, CityDto> _cityConverter;

        /// <summary>
        ///     Service used to fetch <see cref="City" /> data from the database.
        /// </summary>
        private readonly CityRepository _cityRepository;

        /// <summary>
        ///     Database context common for services in this service used to access data.
        /// </summary>
        private readonly PublicTransportContext _db;

        /// <summary>
        ///     Used for converting <see cref="Street" /> objects to <see cref="StreetDto" /> objects and back.
        /// </summary>
        private readonly IConverter<Street, StreetDto> _streetConverter;

        /// <summary>
        ///     Service used to fetch <see cref="Street" /> data from the database.
        /// </summary>
        private readonly StreetRepository _streetRepository;

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
        ///     Creates a <see cref="Street" /> object in the database.
        /// </summary>
        /// <param name="street"><see cref="StreetDto" /> object containing <see cref="Street" /> data.</param>
        /// <returns>
        ///     <see cref="StreetDto" /> representing the inserted <see cref="Street" />.
        /// </returns>
        /// <exception cref="ValidationFaultException">
        ///     Thrown when the data contained in the received DTO contains validation errors.
        /// </exception>
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
        ///     Updates a <see cref="Street" /> object in the database, using the data stored in the
        ///     <see cref="StreetDto" /> object.
        /// </summary>
        /// <param name="street"><see cref="StreetDto" /> representing the object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="StreetDto" /> object containing the updated <see cref="Street" /> data.
        /// </returns>
        /// <exception cref="ValidationFaultException">
        ///     Thrown when the data contained in the received DTO contains validation errors.
        /// </exception>
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
        ///     Deletes a <see cref="Street" /> from the system.
        /// </summary>
        /// <param name="street"><see cref="StreetDto" /> representing the <see cref="Street" /> to be deleted from the database.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the <see cref="Street" /> could not be found in the database.
        /// </exception>
        public void DeleteStreet(StreetDto street)
        {
            _streetRepository.Delete(_streetConverter.GetEntity(street));
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
                .Select(c => _cityConverter.GetDto(c))
                .ToList();
        }

        /// <summary>
        ///     Filters <see cref="Street" /> objects using the supplied <see cref="StreetFilter" />.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="StreetDto" /> objects matching the filtering query.
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