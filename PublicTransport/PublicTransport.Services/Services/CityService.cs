using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.ServiceModel;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.DataTransfer.Converters;
using PublicTransport.Services.Exceptions;
using PublicTransport.Services.Interfaces;

namespace PublicTransport.Services
{
    /// <summary>
    ///     Service used to manage city data.
    /// </summary>
    public class CityService : ICityService
    {
        /// <summary>
        ///     Service used to fetch <see cref="Domain.Entities.City" /> data from the database.
        /// </summary>
        private readonly CityRepository _cityRepository;

        /// <summary>
        ///     Used for converting <see cref="Domain.Entities.City" /> objects to <see cref="CityDto" /> objects and back.
        /// </summary>
        private readonly IConverter<City, CityDto> _converter;

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
        public CityService()
        {
            _db = new PublicTransportContext();
            _cityRepository = new CityRepository(_db);
            _converter = new CityConverter();
        }

        /// <summary>
        ///     Creates a <see cref="Domain.Entities.City" /> object in the database.
        /// </summary>
        /// <param name="city"><see cref="Domain.Entities.City" /> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="Domain.Entities.City" /> object successfully inserted into the database.
        /// </returns>
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
        ///     Updates a <see cref="Domain.Entities.City" /> object in the database, using the data stored in the
        ///     <see cref="CityDto" /> object.
        /// </summary>
        /// <param name="city"><see cref="Domain.Entities.City" /> object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="Domain.Entities.City" /> object successfully updated in the database.
        /// </returns>
        /// <exception cref="ValidationFaultException">
        ///     Thrown when the data contained in the received DTO contains validation errors.
        /// </exception>
        /// <exception cref="FaultException">
        ///     Thrown when the supplied <see cref="Domain.Entities.City" /> could not be found in the database.
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
        ///     Deletes a <see cref="Domain.Entities.City" /> from the system.
        /// </summary>
        /// <param name="city"><see cref="Domain.Entities.City" /> object to be deleted from the database.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Domain.Entities.City" /> could not be found in the database.
        /// </exception>
        public void DeleteCity(CityDto city)
        {
            _cityRepository.Delete(_converter.GetEntity(city));
        }

        /// <summary>
        ///     Filters the stored cities.
        /// </summary>
        /// <param name="name">Filtering parameter.</param>
        /// <returns>
        ///     List of <see cref="Domain.Entities.City" /> objects matching the filtering query.
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