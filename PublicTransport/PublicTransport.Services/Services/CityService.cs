using System.Collections.Generic;
using System.Linq;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.DataTransfer.Converters;
using PublicTransport.Services.Interfaces;

namespace PublicTransport.Services
{
    /// <summary>
    ///     Unit of work used to manage city data.
    /// </summary>
    public class CityService : ICityService
    {
        /// <summary>
        ///     Service used to fetch <see cref="City"/> data from the database.
        /// </summary>
        private readonly CityRepository _cityRepository;

        /// <summary>
        ///     Database context common for services in this unit of work used to access data.
        /// </summary>
        private readonly PublicTransportContext _db;

        private readonly CityConverter _converter;

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
        ///     Calls <see cref="CityRepository"/> create method.
        /// </summary>
        /// <param name="city"><see cref="City"/> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="City"/> object successfully inserted into the database.
        /// </returns>
        public CityDto CreateCity(CityDto city)
        {
            var result = _cityRepository.Create(_converter.GetEntity(city));
            return _converter.GetDto(result);
        }

        /// <summary>
        ///     Calls <see cref="CityRepository"/> update method.
        /// </summary>
        /// <param name="city"><see cref="City"/> object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="City"/> object successfully updated in the database.
        /// </returns>
        /// <exception cref="Exceptions.EntryNotFoundException">
        ///     Thrown when the supplied <see cref="City" /> could not be found in the database.
        /// </exception>
        public CityDto UpdateCity(CityDto city)
        {
            var result = _cityRepository.Update(_converter.GetEntity(city));
            return _converter.GetDto(result);
        }

        /// <summary>
        ///     Calls <see cref="CityRepository"/> delete method.
        /// </summary>
        /// <param name="city"><see cref="City"/> object to be deleted from the database.</param>
        /// <exception cref="Exceptions.EntryNotFoundException">
        ///     Thrown when the supplied <see cref="City" /> could not be found in the database.
        /// </exception>
        public void DeleteCity(CityDto city)
        {
            _cityRepository.Delete(_converter.GetEntity(city));
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
