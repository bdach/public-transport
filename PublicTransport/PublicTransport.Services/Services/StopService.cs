using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Services
{
    /// <summary>
    ///     Service for managing stops.
    /// </summary>
    public class StopService
    {
        /// <summary>
        ///     Inserts a <see cref="Stop" /> record into the database.
        /// </summary>
        /// <param name="stop"><see cref="Stop" /> object to insert into the database.</param>
        /// <returns>The <see cref="Stop" /> object corresponding to the inserted record.</returns>
        public Stop Create(Stop stop)
        {
            using (var db = new PublicTransportContext())
            {
                db.Stops.Add(stop);
                db.SaveChanges();
                return stop;
            }
        }

        /// <summary>
        ///     Returns the <see cref="Stop" /> with the supplied <see cref="Stop.Id" />.
        /// </summary>
        /// <param name="id">Identification number of the desired <see cref="Stop" />.</param>
        /// <returns>
        ///     <see cref="Stop" /> object with the supplied ID number, or null if the <see cref="Stop" /> with the supplied ID
        ///     could not be found in the database.
        /// </returns>
        public Stop Read(int id)
        {
            using (var db = new PublicTransportContext())
            {
                return db.Stops.FirstOrDefault(u => u.Id == id);
            }
        }

        /// <summary>
        ///     Updates all of the fields of the supplied <see cref="Stop" />.
        /// </summary>
        /// <param name="stop"><see cref="Stop" /> object to update.</param>
        /// <returns>Updated <see cref="Stop" /> object.</returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Stop" /> could not be found in the
        ///     database.
        /// </exception>
        public Stop Update(Stop stop)
        {
            using (var db = new PublicTransportContext())
            {
                var old = Read(stop.Id);
                if (old == null)
                {
                    throw new EntryNotFoundException();
                }

                db.Entry(old).CurrentValues.SetValues(stop);
                db.SaveChanges();
                return stop;
            }
        }

        /// <summary>
        ///     Deletes the supplied <see cref="Stop" /> from the database.
        /// </summary>
        /// <param name="stop"><see cref="Stop" /> object to delete.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Stop" /> could not be found in the
        ///     database.
        /// </exception>
        public void Delete(Stop stop)
        {
            using (var db = new PublicTransportContext())
            {
                var old = Read(stop.Id);
                if (old == null)
                {
                    throw new EntryNotFoundException();
                }

                db.Entry(old).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }

        /// <summary>
        ///     Returns a list of <see cref="Stop"/>s associated with a certain <see cref="Route"/>.
        /// </summary>
        /// <param name="routeId">Id of the <see cref="Route"/>.</param>
        /// <returns>
        ///     Returns a list of <see cref="Stop"/>s associated with a certain <see cref="Route"/>.
        /// </returns>
        public List<Stop> GetStopsByRouteId(int routeId)
        {
            using (var db = new PublicTransportContext())
            {
                return db.StopTimes.Where(x => x.Trip.RouteId == routeId).Select(x => x.Stop).Distinct().ToList();
            }
        }
    }
}
