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
    public class StreetService
    {
        /// <summary>
        ///     Inserts a <see cref="Street" /> record into the database.
        /// </summary>
        /// <param name="street"><see cref="Street" /> object to insert into the database.</param>
        /// <returns>The <see cref="Street" /> object corresponding to the inserted record.</returns>
        public Street Create(Street street)
        {
            using (var db = new PublicTransportContext())
            {
                db.Streets.Add(street);
                db.SaveChanges();
                return street;
            }
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
            using (var db = new PublicTransportContext())
            {
                return db.Streets.FirstOrDefault(u => u.Id == id);
            }
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
            using (var db = new PublicTransportContext())
            {
                var old = Read(street.Id);
                if (old == null)
                {
                    throw new EntryNotFoundException();
                }

                db.Entry(old).CurrentValues.SetValues(street);
                db.SaveChanges();
                return street;
            }
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
            using (var db = new PublicTransportContext())
            {
                var old = Read(street.Id);
                if (old == null)
                {
                    throw new EntryNotFoundException();
                }

                db.Entry(old).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }
    }
}
