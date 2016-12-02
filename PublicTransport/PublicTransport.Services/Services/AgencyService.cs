using System;
using System.Collections.Generic;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Exceptions;
using PublicTransport.Services.Repositories;

namespace PublicTransport.Services
{
    public interface IAgencyService : IDisposable
    {
        /// <summary>
        ///     Calls <see cref="AgencyRepository"/> create method.
        /// </summary>
        /// <param name="agency"><see cref="Agency"/> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="Agency"/> object successfully inserted into the database.
        /// </returns>
        Agency CreateAgency(Agency agency);

        /// <summary>
        ///     Calls <see cref="AgencyRepository"/> update method.
        /// </summary>
        /// <param name="agency"><see cref="Agency"/> object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="Agency"/> object successfully updated in the database.
        /// </returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Agency" /> could not be found in the database.
        /// </exception>
        Agency UpdateAgency(Agency agency);

        /// <summary>
        ///     Calls <see cref="AgencyRepository"/> delete method.
        /// </summary>
        /// <param name="agency"><see cref="Agency"/> object to be deleted from the database.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Agency" /> could not be found in the database.
        /// </exception>
        void DeleteAgency(Agency agency);

        /// <summary>
        ///     Calls <see cref="AgencyRepository"/> filtering method.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="Agency"/> objects matching the filtering query.
        /// </returns>
        List<Agency> FilterAgencies(IAgencyFilter filter);

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
        }

        /// <summary>
        ///     Calls <see cref="AgencyRepository"/> create method.
        /// </summary>
        /// <param name="agency"><see cref="Agency"/> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="Agency"/> object successfully inserted into the database.
        /// </returns>
        public Agency CreateAgency(Agency agency)
        {
            return _agencyRepository.Create(agency);
        }

        /// <summary>
        ///     Calls <see cref="AgencyRepository"/> update method.
        /// </summary>
        /// <param name="agency"><see cref="Agency"/> object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="Agency"/> object successfully updated in the database.
        /// </returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Agency" /> could not be found in the database.
        /// </exception>
        public Agency UpdateAgency(Agency agency)
        {
            return _agencyRepository.Update(agency);
        }

        /// <summary>
        ///     Calls <see cref="AgencyRepository"/> delete method.
        /// </summary>
        /// <param name="agency"><see cref="Agency"/> object to be deleted from the database.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Agency" /> could not be found in the database.
        /// </exception>
        public void DeleteAgency(Agency agency)
        {
            _agencyRepository.Delete(agency);
        }

        /// <summary>
        ///     Calls <see cref="AgencyRepository"/> filtering method.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="Agency"/> objects matching the filtering query.
        /// </returns>
        public List<Agency> FilterAgencies(IAgencyFilter filter)
        {
            return _agencyRepository.FilterAgencies(filter);
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