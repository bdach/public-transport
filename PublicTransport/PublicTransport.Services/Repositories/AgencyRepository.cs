﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Services.Repositories
{
    /// <summary>
    ///     Service for managing agencies.
    /// </summary>
    public class AgencyRepository
    {
        /// <summary>
        ///     An instance of database context.
        /// </summary>
        private readonly PublicTransportContext _db;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="db"><see cref="PublicTransportContext" /> to use during service operations.</param>
        public AgencyRepository(PublicTransportContext db)
        {
            _db = db;
        }

        /// <summary>
        ///     Inserts an <see cref="Agency" /> record into the database.
        /// </summary>
        /// <param name="agency"><see cref="Agency" /> object to insert into the database.</param>
        /// <returns>The <see cref="Agency" /> object corresponding to the inserted record.</returns>
        public Agency Create(Agency agency)
        {
            _db.Agencies.Add(agency);
            _db.SaveChanges();
            return agency;
        }

        /// <summary>
        ///     Returns the <see cref="Agency" /> with the supplied <see cref="Agency.Id" />.
        /// </summary>
        /// <param name="id">Identification number of the desired <see cref="Agency" />.</param>
        /// <returns>
        ///     <see cref="Agency" /> object with the supplied ID number, or null if the user with the supplied ID could not be
        ///     found in the database.
        /// </returns>
        public Agency Read(int id)
        {
            return _db.Agencies.FirstOrDefault(u => u.Id == id);
        }

        /// <summary>
        ///     Updates all of the fields of the supplied <see cref="Agency" />.
        /// </summary>
        /// <param name="agency"><see cref="Agency" /> object to update.</param>
        /// <returns>Updated <see cref="Agency" /> object.</returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Agency" /> could not be found in the database.
        /// </exception>
        public Agency Update(Agency agency)
        {
            var old = Read(agency.Id);
            if (old == null)
            {
                throw new EntryNotFoundException();
            }

            _db.Entry(old).CurrentValues.SetValues(agency);
            _db.SaveChanges();
            return agency;
        }

        /// <summary>
        ///     Deletes the supplied <see cref="Agency" /> from the database.
        /// </summary>
        /// <param name="agency"><see cref="Agency" /> object to delete.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Agency" /> could not be found in the database.
        /// </exception>
        public void Delete(Agency agency)
        {
            var old = Read(agency.Id);
            if (old == null)
            {
                throw new EntryNotFoundException();
            }

            _db.Entry(old).State = EntityState.Deleted;
            _db.SaveChanges();
        }

        /// <summary>
        ///     Selects all the <see cref="Agency" /> objects that match all the criteria specified by the
        ///     <see cref="AgencyFilter" /> object. The returned agencies' name, street name and city name strings all contain the
        ///     parameters supplied in the <see cref="filter" /> parameter.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>List of items satisfying the supplied query.</returns>
        public List<Agency> FilterAgencies(AgencyFilter filter)
        {
            return _db.Agencies.Include(a => a.Street.City)
                .Where(a => a.Name.Contains(filter.AgencyNameFilter))
                .Where(a => a.Street.Name.Contains(filter.StreetNameFilter))
                .Where(a => a.Street.City.Name.Contains(filter.CityNameFilter))
                .Take(20).ToList();
        }
    }
}