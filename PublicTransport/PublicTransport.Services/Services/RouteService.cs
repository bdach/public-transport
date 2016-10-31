using System.Data.Entity;
using System.Linq;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Services
{
    /// <summary>
    ///     Service for managing routes.
    /// </summary>
    public class RouteService
    {
        /// <summary>
        ///     Inserts a <see cref="Route" /> record into the database.
        /// </summary>
        /// <param name="route"><see cref="Route" /> object to insert into the database.</param>
        /// <returns>The <see cref="Route" /> object corresponding to the inserted record.</returns>
        public Route Create(Route route)
        {
            using (var db = new PublicTransportContext())
            {
                db.Routes.Add(route);
                db.SaveChanges();
                return route;
            }
        }

        /// <summary>
        ///     Returns the <see cref="Route" /> with the supplied <see cref="Route.Id" />.
        /// </summary>
        /// <param name="id">Identification number of the desired <see cref="Route" />.</param>
        /// <returns>
        ///     <see cref="Route" /> object with the supplied ID number, or null if the <see cref="Route" /> with the supplied ID
        ///     could not be found in the database.
        /// </returns>
        public Route Read(int id)
        {
            using (var db = new PublicTransportContext())
            {
                return db.Routes.FirstOrDefault(u => u.Id == id);
            }
        }

        /// <summary>
        ///     Updates all of the fields of the supplied <see cref="Route" />.
        /// </summary>
        /// <param name="route"><see cref="Route" /> object to update.</param>
        /// <returns>Updated <see cref="Route" /> object.</returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Route" /> could not be found in the
        ///     database.
        /// </exception>
        public Route Update(Route route)
        {
            using (var db = new PublicTransportContext())
            {
                var old = Read(route.Id);
                if (old == null)
                {
                    throw new EntryNotFoundException();
                }

                db.Entry(old).CurrentValues.SetValues(route);
                db.SaveChanges();
                return route;
            }
        }

        /// <summary>
        ///     Deletes the supplied <see cref="Route" /> from the database.
        /// </summary>
        /// <param name="route"><see cref="Route" /> object to delete.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Route" /> could not be found in the
        ///     database.
        /// </exception>
        public void Delete(Route route)
        {
            using (var db = new PublicTransportContext())
            {
                var old = Read(route.Id);
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
