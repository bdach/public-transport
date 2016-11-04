using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Services
{
    /// <summary>
    ///     Service for managing streets.
    /// </summary>
    public class StreetService : IDisposable
    {
        private readonly PublicTransportContext _db = new PublicTransportContext();
        private bool _disposed;

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
            return _db.Streets.FirstOrDefault(u => u.Id == id);
        }

        /// <summary>
        ///     Updates all of the fields of the supplied <see cref="Street" />.
        /// </summary>
        /// <param name="street"><see cref="Street" /> object to update.</param>
        /// <returns>Updated <see cref="Street" /> object.</returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Street" /> could not be found in the
        ///     database.
        /// </exception>
        public Street Update(Street street)
        {
            var old = Read(street.Id);
            if (old == null)
            {
                throw new EntryNotFoundException();
            }

            _db.Cities.Attach(street.City);
            _db.Entry(old).CurrentValues.SetValues(street);
            _db.SaveChanges();
            return street;
        }

        /// <summary>
        ///     Deletes the supplied <see cref="Street" /> from the database.
        /// </summary>
        /// <param name="street"><see cref="Street" /> object to delete.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Street" /> could not be found in the
        ///     database.
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
        ///     Return a list of <see cref="Street"/>s whose names contain provided string.
        /// </summary>
        /// <param name="str">String which has to be present in the name.</param>
        /// <returns>
        ///     Return a list of <see cref="Street"/>s whose names contain provided string.
        /// </returns>
        public List<Street> GetStreetsContainingString(string str)
        {
            return _db.Streets.Where(x => x.Name.Contains(str)).Include(x => x.City).Take(10).ToList();
        }

        /// <summary>
        ///     Disposed database context.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;
            _db.Dispose();
            _disposed = true;
        }
    }
}
