using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Services
{
    /// <summary>
    ///     Service for managing streets.
    /// </summary>
    public class StreetRepository
    {
        /// <summary>
        ///     An instance of database context.
        /// </summary>
        private readonly PublicTransportContext _db;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="db"><see cref="PublicTransportContext" /> to use during service operations.</param>
        public StreetRepository(PublicTransportContext db)
        {
            _db = db;   
        }

        /// <summary>
        ///     Inserts a <see cref="Street" /> record into the database.
        /// </summary>
        /// <param name="street"><see cref="Street" /> object to insert into the database.</param>
        /// <returns>The <see cref="Street" /> object corresponding to the inserted record.</returns>
        public Street Create(Street street)
        {
            _db.Streets.Add(street);
            _db.SaveChanges();
            return street;
        }

        /// <summary>
        ///     Returns the <see cref="Street" /> with the supplied <see cref="Street.Id" />.
        /// </summary>
        /// <param name="id">Identification number of the desired <see cref="Street" />.</param>
        /// <returns>
        ///     <see cref="Street" /> object with the supplied ID number, or null if the <see cref="Street" /> with the supplied ID
        ///     could not be found in the database.
        /// </returns>
        public Street Read(int id)
        {
            return _db.Streets.Include(u => u.City).FirstOrDefault(u => u.Id == id);
        }

        /// <summary>
        ///     Updates all of the fields of the supplied <see cref="Street" />.
        /// </summary>
        /// <param name="street"><see cref="Street" /> object to update.</param>
        /// <returns>Updated <see cref="Street" /> object.</returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Street" /> could not be found in the database.
        /// </exception>
        public Street Update(Street street)
        {
            var old = Read(street.Id);
            if (old == null)
            {
                throw new EntryNotFoundException();
            }

            _db.Entry(old).CurrentValues.SetValues(street);
            _db.SaveChanges();
            return street;
        }

        /// <summary>
        ///     Deletes the supplied <see cref="Street" /> from the database.
        /// </summary>
        /// <param name="street"><see cref="Street" /> object to delete.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Street" /> could not be found in the database.
        /// </exception>
        public void Delete(Street street)
        {
            var old = Read(street.Id);
            if (old == null)
            {
                throw new EntryNotFoundException();
            }

            _db.Entry(old).State = EntityState.Deleted;
            _db.SaveChanges();
        }

        /// <summary>
        /// Filters out <see cref="Street"/> objects, using values from the supplied <see cref="IStreetFilter"/> object to perform the query.
        /// </summary>
        /// <param name="streetFilter">Object containing the query parameters.</param>
        /// <returns>List of items satisfying the supplied query.</returns>
        public List<Street> FilterStreets(IStreetFilter streetFilter)
        {
            return _db.Streets.Include(s => s.City)
                .Where(s => s.Name.Contains(streetFilter.StreetNameFilter))
                .Where(s => s.City.Name.Contains(streetFilter.CityNameFilter))
                .Take(20).ToList();
        }
    }
}
