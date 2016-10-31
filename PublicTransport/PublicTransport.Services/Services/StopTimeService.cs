using System.Data.Entity;
using System.Linq;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Services
{
    /// <summary>
    ///     Service for managing stop times.
    /// </summary>
    public class StopTimeService
    {
        /// <summary>
        ///     Inserts a <see cref="StopTime" /> record into the database.
        /// </summary>
        /// <param name="stopTime"><see cref="StopTime" /> object to insert into the database.</param>
        /// <returns>The <see cref="StopTime" /> object corresponding to the inserted record.</returns>
        public StopTime Create(StopTime stopTime)
        {
            using (var db = new PublicTransportContext())
            {
                db.StopTimes.Add(stopTime);
                db.SaveChanges();
                return stopTime;
            }
        }

        /// <summary>
        ///     Returns the <see cref="StopTime" /> with the supplied <see cref="StopTime.Id" />.
        /// </summary>
        /// <param name="id">Identification number of the desired <see cref="StopTime" />.</param>
        /// <returns>
        ///     <see cref="StopTime" /> object with the supplied ID number, or null if the <see cref="StopTime" /> with the supplied ID
        ///     could not be found in the database.
        /// </returns>
        public StopTime Read(int id)
        {
            using (var db = new PublicTransportContext())
            {
                return db.StopTimes.FirstOrDefault(u => u.Id == id);
            }
        }

        /// <summary>
        ///     Updates all of the fields of the supplied <see cref="StopTime" />.
        /// </summary>
        /// <param name="stopTime"><see cref="StopTime" /> object to update.</param>
        /// <returns>Updated <see cref="StopTime" /> object.</returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="StopTime" /> could not be found in the
        ///     database.
        /// </exception>
        public StopTime Update(StopTime stopTime)
        {
            using (var db = new PublicTransportContext())
            {
                var old = Read(stopTime.Id);
                if (old == null)
                {
                    throw new EntryNotFoundException();
                }

                db.Entry(old).CurrentValues.SetValues(stopTime);
                db.SaveChanges();
                return stopTime;
            }
        }

        /// <summary>
        ///     Deletes the supplied <see cref="StopTime" /> from the database.
        /// </summary>
        /// <param name="stopTime"><see cref="StopTime" /> object to delete.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="StopTime" /> could not be found in the
        ///     database.
        /// </exception>
        public void Delete(StopTime stopTime)
        {
            using (var db = new PublicTransportContext())
            {
                var old = Read(stopTime.Id);
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
