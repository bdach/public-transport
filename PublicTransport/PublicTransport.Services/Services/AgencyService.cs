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
    ///     Service used to manage agency data.
    /// </summary>
    public class AgencyService : IAgencyService
    {
        /// <summary>
        ///     Service used to fetch <see cref="Agency"/> data from the database.
        /// </summary>
        private readonly AgencyRepository _agencyRepository;

        /// <summary>
        ///     Service used to fetch <see cref="Street"/> data from the database.
        /// </summary>
        private readonly StreetRepository _streetRepository;

        /// <summary>
        ///     Used for converting <see cref="Agency" /> objects to <see cref="AgencyDto" /> objects and back.
        /// </summary>
        private readonly IConverter<Agency, AgencyDto> _agencyConverter;

        /// <summary>
        ///     Used for converting <see cref="Street" /> objects to <see cref="StreetDto" /> objects and back.
        /// </summary>
        private readonly IConverter<Street, StreetDto> _streetConverter;

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
        public AgencyService()
        {
            _db = new PublicTransportContext();
            _agencyRepository = new AgencyRepository(_db);
            _streetRepository = new StreetRepository(_db);
            _agencyConverter = new AgencyConverter();
            _streetConverter = new StreetConverter();
        }

        /// <summary>
        ///     Creates an <see cref="Agency"/> object in the database.
        /// </summary>
        /// <param name="agency"><see cref="AgencyDto" /> object containing <see cref="Agency"/> data.</param>
        /// <returns>
        ///     <see cref="AgencyDto" /> representing the inserted <see cref="Agency"/>.
        /// </returns>
        /// <exception cref="ValidationFaultException">
        ///     Thrown when the data contained in the received DTO contains validation errors.
        /// </exception>
        public AgencyDto CreateAgency(AgencyDto agency)
        {
            try
            {
                var result = _agencyRepository.Create(_agencyConverter.GetEntity(agency));
                return _agencyConverter.GetDto(result);
            }
            catch (DbEntityValidationException ex)
            {
                throw new ValidationFaultException(ex);
            }
        }

        /// <summary>
        ///     Updates an <see cref="Agency"/> object in the database, using the data stored in the
        ///     <see cref="AgencyDto" /> object.
        /// </summary>
        /// <param name="agency"><see cref="AgencyDto" /> representing the object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="AgencyDto" /> object containing the updated <see cref="Agency"/> data.
        /// </returns>
        /// <exception cref="ValidationFaultException">
        ///     Thrown when the data contained in the received DTO contains validation errors.
        /// </exception>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Agency"/> could not be found in the database.
        /// </exception>
        public AgencyDto UpdateAgency(AgencyDto agency)
        {
            try
            {
                var result = _agencyRepository.Update(_agencyConverter.GetEntity(agency));
                return _agencyConverter.GetDto(result);
            }
            catch (DbEntityValidationException ex)
            {
                throw new ValidationFaultException(ex);
            }
        }

        /// <summary>
        ///     Deletes an <see cref="Agency"/> from the system.
        /// </summary>
        /// <param name="agency"><see cref="AgencyDto" /> representing the <see cref="Agency"/> to be deleted from the database.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the <see cref="Agency" /> could not be found in the database.
        /// </exception>
        public void DeleteAgency(AgencyDto agency)
        {
            _agencyRepository.Delete(_agencyConverter.GetEntity(agency));
        }

        /// <summary>
        ///     Filters <see cref="Agency"/> objects using the supplied <see cref="AgencyFilter"/>.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="AgencyDto"/> objects matching the filtering query.
        /// </returns>
        public List<AgencyDto> FilterAgencies(AgencyFilter filter)
        {
            return _agencyRepository.FilterAgencies(filter)
                .Select(_agencyConverter.GetDto)
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
                .Select(_streetConverter.GetDto)
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