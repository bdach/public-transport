using System.Data.Entity;
using System.Linq;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Services
{
    /// <summary>
    ///     Service for managing zones.
    /// </summary>
    public class ZoneService
    {
        /// <summary>
        ///     Inserts a <see cref="Zone" /> record into the database.
        /// </summary>
        /// <param name="zone"><see cref="Zone" /> object to insert into the database.</param>
        /// <returns>The <see cref="Zone" /> object corresponding to the inserted record.</returns>
        public Zone Create(Zone zone)
        {
            using (var db = new PublicTransportContext())
            {
                db.Zones.Add(zone);
                db.SaveChanges();
                return zone;
            }
        }

        /// <summary>
        ///     Returns the <see cref="Zone" /> with the supplied <see cref="Zone.Id" />.
        /// </summary>
        /// <param name="id">Identification number of the desired <see cref="Zone" />.</param>
        /// <returns>
        ///     <see cref="Zone" /> object with the supplied ID number, or null if the <see cref="Zone" /> with the supplied ID
        ///     could not be found in the database.
        /// </returns>
        public Zone Read(int id)
        {
            using (var db = new PublicTransportContext())
            {
                return db.Zones.FirstOrDefault(u => u.Id == id);
            }
        }

        /// <summary>
        ///     Updates all of the fields of the supplied <see cref="Zone" />.
        /// </summary>
        /// <param name="zone"><see cref="Zone" /> object to update.</param>
        /// <returns>Updated <see cref="Zone" /> object.</returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Zone" /> could not be found in the
        ///     database.
        /// </exception>
        public Zone Update(Zone zone)
        {
            using (var db = new PublicTransportContext())
            {
                var old = Read(zone.Id);
                if (old == null)
                {
                    throw new EntryNotFoundException();
                }

                db.Entry(old).CurrentValues.SetValues(zone);
                db.SaveChanges();
                return zone;
            }
        }

        /// <summary>
        ///     Deletes the supplied <see cref="Zone" /> from the database.
        /// </summary>
        /// <param name="zone"><see cref="Zone" /> object to delete.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Zone" /> could not be found in the
        ///     database.
        /// </exception>
        public void Delete(Zone zone)
        {
            using (var db = new PublicTransportContext())
            {
                var old = Read(zone.Id);
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
