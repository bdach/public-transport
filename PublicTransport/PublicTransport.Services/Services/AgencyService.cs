using System.Data.Entity;
using System.Linq;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Services
{
    /// <summary>
    ///     Service for managing agencies.
    /// </summary>
    public class AgencyService
    {
        /// <summary>
        ///     Inserts an <see cref="Agency" /> record into the database.
        /// </summary>
        /// <param name="agency"><see cref="Agency" /> object to insert into the database.</param>
        /// <returns>The <see cref="Agency" /> object corresponding to the inserted record.</returns>
        public Agency Create(Agency agency)
        {
            using (var db = new PublicTransportContext())
            {
                db.Agencies.Add(agency);
                db.SaveChanges();
                return agency;
            }
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
            using (var db = new PublicTransportContext())
            {
                return db.Agencies.FirstOrDefault(u => u.Id == id);
            }
        }

        /// <summary>
        ///     Updates all of the fields of the supplied <see cref="Agency" />.
        /// </summary>
        /// <param name="agency"><see cref="Agency" /> object to update.</param>
        /// <returns>Updated <see cref="Agency" /> object.</returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Agency" /> could not be found in the
        ///     database.
        /// </exception>
        public Agency Update(Agency agency)
        {
            using (var db = new PublicTransportContext())
            {
                var old = Read(agency.Id);
                if (old == null)
                {
                    throw new EntryNotFoundException();
                }

                db.Entry(old).CurrentValues.SetValues(agency);
                db.SaveChanges();
                return agency;
            }
        }

        /// <summary>
        ///     Deletes the supplied <see cref="Agency" /> from the database.
        /// </summary>
        /// <param name="agency"><see cref="Agency" /> object to delete.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Agency" /> could not be found in the
        ///     database.
        /// </exception>
        public void Delete(Agency agency)
        {
            using (var db = new PublicTransportContext())
            {
                var old = Read(agency.Id);
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