using System.Data.Entity;
using System.Linq;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Services
{
    /// <summary>
    ///     Service for managing calendar dates.
    /// </summary>
    public class CalendarDateService
    {
        /// <summary>
        ///     Inserts an <see cref="CalendarDate" /> record into the database.
        /// </summary>
        /// <param name="calendarDate"><see cref="CalendarDate" /> object to insert into the database.</param>
        /// <returns>The <see cref="CalendarDate" /> object corresponding to the inserted record.</returns>
        public CalendarDate Create(CalendarDate calendarDate)
        {
            using (var db = new PublicTransportContext())
            {
                db.CalendarDates.Add(calendarDate);
                db.SaveChanges();
                return calendarDate;
            }
        }

        /// <summary>
        ///     Returns the <see cref="CalendarDate" /> with the supplied <see cref="CalendarDate.Id" />.
        /// </summary>
        /// <param name="id">Identification number of the desired <see cref="CalendarDate" />.</param>
        /// <returns>
        ///     <see cref="CalendarDate" /> object with the supplied ID number, or null if the user with the supplied ID could not
        ///     be found in the database.
        /// </returns>
        public CalendarDate Read(int id)
        {
            using (var db = new PublicTransportContext())
            {
                return db.CalendarDates.FirstOrDefault(u => u.Id == id);
            }
        }

        /// <summary>
        ///     Updates all of the fields of the supplied <see cref="CalendarDate" />.
        /// </summary>
        /// <param name="calendarDate"><see cref="CalendarDate" /> object to update.</param>
        /// <returns>Updated <see cref="CalendarDate" /> object.</returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="CalendarDate" /> could not be found in the
        ///     database.
        /// </exception>
        public CalendarDate Update(CalendarDate calendarDate)
        {
            using (var db = new PublicTransportContext())
            {
                var old = Read(calendarDate.Id);
                if (old == null)
                {
                    throw new EntryNotFoundException();
                }

                db.Entry(old).CurrentValues.SetValues(calendarDate);
                db.SaveChanges();
                return calendarDate;
            }
        }

        /// <summary>
        ///     Deletes the supplied <see cref="CalendarDate" /> from the database.
        /// </summary>
        /// <param name="calendar"><see cref="CalendarDate" /> object to delete.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="CalendarDate" /> could not be found in the
        ///     database.
        /// </exception>
        public void Delete(CalendarDate calendar)
        {
            using (var db = new PublicTransportContext())
            {
                var old = Read(calendar.Id);
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