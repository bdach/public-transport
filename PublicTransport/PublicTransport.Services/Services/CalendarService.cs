using System.Data.Entity;
using System.Linq;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Services
{
    /// <summary>
    ///     Service for managing calendars.
    /// </summary>
    public class CalendarService
    {
        /// <summary>
        ///     An instance of database context.
        /// </summary>
        private readonly PublicTransportContext _db;

        public CalendarService()
        {

        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="db"><see cref="PublicTransportContext" /> to use during service operations.</param>
        public CalendarService(PublicTransportContext db)
        {
            _db = db;
        }

        /// <summary>
        ///     Inserts an <see cref="Calendar" /> record into the database.
        /// </summary>
        /// <param name="calendar"><see cref="Calendar" /> object to insert into the database.</param>
        /// <returns>The <see cref="Calendar" /> object corresponding to the inserted record.</returns>
        public Calendar Create(Calendar calendar)
        {
            _db.Calendars.Add(calendar);
            _db.SaveChanges();
            return calendar;
        }

        /// <summary>
        ///     Returns the <see cref="Calendar" /> with the supplied <see cref="Calendar.Id" />.
        /// </summary>
        /// <param name="id">Identification number of the desired <see cref="Calendar" />.</param>
        /// <returns>
        ///     <see cref="Calendar" /> object with the supplied ID number, or null if the user with the supplied ID could not be
        ///     found in the database.
        /// </returns>
        public Calendar Read(int id)
        {
            return _db.Calendars.FirstOrDefault(u => u.Id == id);
        }

        /// <summary>
        ///     Updates all of the fields of the supplied <see cref="Calendar" />.
        /// </summary>
        /// <param name="calendar"><see cref="Calendar" /> object to update.</param>
        /// <returns>Updated <see cref="Calendar" /> object.</returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Calendar" /> could not be found in the
        ///     database.
        /// </exception>
        public Calendar Update(Calendar calendar)
        {
            var old = Read(calendar.Id);
            if (old == null)
            {
                throw new EntryNotFoundException();
            }

            _db.Entry(old).CurrentValues.SetValues(calendar);
            _db.SaveChanges();
            return calendar;
        }

        /// <summary>
        ///     Deletes the supplied <see cref="Calendar" /> from the database.
        /// </summary>
        /// <param name="calendar"><see cref="Calendar" /> object to delete.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Calendar" /> could not be found in the
        ///     database.
        /// </exception>
        public void Delete(Calendar calendar)
        {
            var old = Read(calendar.Id);
            if (old == null)
            {
                throw new EntryNotFoundException();
            }

            _db.Entry(old).State = EntityState.Deleted;
            _db.SaveChanges();
        }
    }
}