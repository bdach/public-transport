using System;
using System.Collections.Generic;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer.Filters;

namespace PublicTransport.Services.UnitsOfWork
{
    /// <summary>
    ///     Unit of work used to manage agency data.
    /// </summary>
    public class AgencyUnitOfWork : IDisposable
    {
        /// <summary>
        ///     Service used to fetch <see cref="Agency"/> data from the database.
        /// </summary>
        private readonly AgencyService _agencyService;

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
        public AgencyUnitOfWork()
        {
            _db = new PublicTransportContext();
            _agencyService = new AgencyService(_db);
            _streetService = new StreetService(_db);
        }

        /// <summary>
        ///     Calls <see cref="AgencyService"/> create method.
        /// </summary>
        /// <param name="agency"><see cref="Agency"/> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="Agency"/> object successfully inserted into the database.
        /// </returns>
        public Agency CreateAgency(Agency agency)
        {
            return _agencyService.Create(agency);
        }

        /// <summary>
        ///     Calls <see cref="AgencyService"/> update method.
        /// </summary>
        /// <param name="agency"><see cref="Agency"/> object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="Agency"/> object successfully updated in the database.
        /// </returns>
        /// <exception cref="Exceptions.EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Agency" /> could not be found in the database.
        /// </exception>
        public Agency UpdateAgency(Agency agency)
        {
            return _agencyService.Update(agency);
        }

        /// <summary>
        ///     Calls <see cref="AgencyService"/> delete method.
        /// </summary>
        /// <param name="agency"><see cref="Agency"/> object to be deleted from the database.</param>
        /// <exception cref="Exceptions.EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Agency" /> could not be found in the database.
        /// </exception>
        public void DeleteAgency(Agency agency)
        {
            _agencyService.Delete(agency);
        }

        /// <summary>
        ///     Calls <see cref="AgencyService"/> filtering method.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="Agency"/> objects matching the filtering query.
        /// </returns>
        public List<Agency> FilterAgencies(IAgencyFilter filter)
        {
            return _agencyService.FilterAgencies(filter);
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