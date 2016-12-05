using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Services.Repositories
{
    /// <summary>
    ///     Service for managing stops.
    /// </summary>
    public class StopRepository
    {
        /// <summary>
        ///     An instance of database context.
        /// </summary>
        private readonly PublicTransportContext _db;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="db"><see cref="PublicTransportContext" /> to use during service operations.</param>
        public StopRepository(PublicTransportContext db)
        {
            _db = db;
        }

        /// <summary>
        ///     Inserts a <see cref="Stop" /> record into the database.
        /// </summary>
        /// <param name="stop"><see cref="Stop" /> object to insert into the database.</param>
        /// <returns>The <see cref="Stop" /> object corresponding to the inserted record.</returns>
        public Stop Create(Stop stop)
        {
            _db.Stops.Add(stop);
            _db.SaveChanges();
            return stop;
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
            return _db.Stops.FirstOrDefault(u => u.Id == id);
        }

        /// <summary>
        ///     Updates all of the fields of the supplied <see cref="Stop" />.
        /// </summary>
        /// <param name="stop"><see cref="Stop" /> object to update.</param>
        /// <returns>Updated <see cref="Stop" /> object.</returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Stop" /> could not be found in the database.
        /// </exception>
        public Stop Update(Stop stop)
        {
            var old = Read(stop.Id);
            if (old == null)
            {
                throw new EntryNotFoundException();
            }

            _db.Entry(old).CurrentValues.SetValues(stop);
            _db.SaveChanges();
            return stop;
        }

        /// <summary>
        ///     Deletes the supplied <see cref="Stop" /> from the database.
        /// </summary>
        /// <param name="stop"><see cref="Stop" /> object to delete.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Stop" /> could not be found in the database.
        /// </exception>
        public void Delete(Stop stop)
        {
            var old = Read(stop.Id);
            if (old == null)
            {
                throw new EntryNotFoundException();
            }

            _db.Entry(old).State = EntityState.Deleted;
            _db.SaveChanges();
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
            return _db.StopTimes.Include(st => st.Trip)
                .Where(st => st.Trip.RouteId == routeId)
                .OrderBy(st => st.StopSequence)
                .Select(st => st.Stop)
                .Include(s => s.Street.City)
                .Distinct().ToList();
        }

        /// <summary>
        ///     Selects all the <see cref="Stop" /> objects that match all the criteria specified by the
        ///     <see cref="StopFilter" /> object. The returned name strings all contain the
        ///     parameters supplied in the <see cref="filter" /> parameter.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>List of items satisfying the supplied query.</returns>
        public List<Stop> FilterStops(StopFilter filter)
        {
            return _db.Stops.Include(s => s.Street.City).Include(s => s.Zone).Include(s => s.ParentStation)
                .Where(s => s.Name.Contains(filter.StopNameFilter))
                .Where(s => s.Street.Name.Contains(filter.StreetNameFilter))
                .Where(s => s.Street.City.Name.Contains(filter.CityNameFilter))
                .Where(s => s.Zone == null || s.Zone.Name.Contains(filter.ZoneNameFilter))
                .Where(s => s.ParentStation == null || s.ParentStation.Name.Contains(filter.ParentStationNameFilter))
                .Where(s => !filter.OnlyStations || s.IsStation)
                .Take(20).ToList();
        }
    }
}
