using System.Data.Entity;
using System.Linq;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Services
{
    /// <summary>
    ///     Service for managing trips.
    /// </summary>
    public class TripService
    {
        /// <summary>
        ///     Inserts a <see cref="Trip" /> record into the database.
        /// </summary>
        /// <param name="trip"><see cref="Trip" /> object to insert into the database.</param>
        /// <returns>The <see cref="Trip" /> object corresponding to the inserted record.</returns>
        public Trip Create(Trip trip)
        {
            using (var db = new PublicTransportContext())
            {
                db.Trips.Add(trip);
                db.SaveChanges();
                return trip;
            }
        }

        /// <summary>
        ///     Returns the <see cref="Trip" /> with the supplied <see cref="Trip.Id" />.
        /// </summary>
        /// <param name="id">Identification number of the desired <see cref="Trip" />.</param>
        /// <returns>
        ///     <see cref="Trip" /> object with the supplied ID number, or null if the <see cref="Trip" /> with the supplied ID
        ///     could not be found in the database.
        /// </returns>
        public Trip Read(int id)
        {
            using (var db = new PublicTransportContext())
            {
                return db.Trips.FirstOrDefault(u => u.Id == id);
            }
        }

        /// <summary>
        ///     Updates all of the fields of the supplied <see cref="Trip" />.
        /// </summary>
        /// <param name="trip"><see cref="Trip" /> object to update.</param>
        /// <returns>Updated <see cref="Trip" /> object.</returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Trip" /> could not be found in the
        ///     database.
        /// </exception>
        public Trip Update(Trip trip)
        {
            using (var db = new PublicTransportContext())
            {
                var old = Read(trip.Id);
                if (old == null)
                {
                    throw new EntryNotFoundException();
                }

                db.Entry(old).CurrentValues.SetValues(trip);
                db.SaveChanges();
                return trip;
            }
        }

        /// <summary>
        ///     Deletes the supplied <see cref="Trip" /> from the database.
        /// </summary>
        /// <param name="trip"><see cref="Trip" /> object to delete.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Trip" /> could not be found in the
        ///     database.
        /// </exception>
        public void Delete(Trip trip)
        {
            using (var db = new PublicTransportContext())
            {
                var old = Read(trip.Id);
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
