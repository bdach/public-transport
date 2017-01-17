using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Services.Repositories
{
    public interface IStopTimeRepository
    {
        /// <summary>
        ///     Inserts a <see cref="StopTime" /> record into the database.
        /// </summary>
        /// <param name="stopTime"><see cref="StopTime" /> object to insert into the database.</param>
        /// <returns>The <see cref="StopTime" /> object corresponding to the inserted record.</returns>
        StopTime Create(StopTime stopTime);

        /// <summary>
        ///     Returns the <see cref="StopTime" /> with the supplied <see cref="StopTime.Id" />.
        /// </summary>
        /// <param name="id">Identification number of the desired <see cref="StopTime" />.</param>
        /// <returns>
        ///     <see cref="StopTime" /> object with the supplied ID number, or null if the <see cref="StopTime" /> with the supplied ID
        ///     could not be found in the database.
        /// </returns>
        StopTime Read(int id);

        /// <summary>
        ///     Updates all of the fields of the supplied <see cref="StopTime" />.
        /// </summary>
        /// <param name="stopTime"><see cref="StopTime" /> object to update.</param>
        /// <returns>Updated <see cref="StopTime" /> object.</returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="StopTime" /> could not be found in the database.
        /// </exception>
        StopTime Update(StopTime stopTime);

        /// <summary>
        ///     Deletes the supplied <see cref="StopTime" /> from the database.
        /// </summary>
        /// <param name="stopTime"><see cref="StopTime" /> object to delete.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="StopTime" /> could not be found in the database.
        /// </exception>
        void Delete(StopTime stopTime);

        /// <summary>
        ///     Returns a list of <see cref="StopTime" />s for a certain <see cref="Stop"/>.
        /// </summary>
        /// <param name="stopId">Id of the <see cref="Stop" />.</param>
        /// <returns>
        ///     Returns a list of <see cref="StopTime" />s for a certain <see cref="Stop"/>.
        /// </returns>
        Dictionary<Route, List<StopTime>> GetFullTimetableByStopId(int stopId);

        /// <summary>
        ///     Returns a list of <see cref="StopTime" />s for a certain <see cref="Stop"/> which are associated with a specific <see cref="Route"/>.
        /// </summary>
        /// <returns>
        ///     Returns a list of <see cref="StopTime" />s for a certain <see cref="Stop"/> which are associated with a specific <see cref="Route"/>.
        /// </returns>
        List<StopTime> GetRouteTimetableByStopId(StopTimeFilter filter);

        /// <summary>
        ///     Fetches the full timetable for the <see cref="Route"/> with the supplied ID.
        /// </summary>
        /// <param name="routeId">ID of the route for which the timetable should be fetched.</param>
        /// <returns>
        ///     Returns all <see cref="StopTime"/>s associated with a certain <see cref="Route"/>, grouped by 
        ///     <see cref="Stop"/>s.
        /// </returns>
        Dictionary<Stop, List<StopTime>> GetFullTimetableByRouteId(int routeId);
    }

    /// <summary>
    ///     Service for managing stop times.
    /// </summary>
    public class StopTimeRepository : IStopTimeRepository
    {
        /// <summary>
        ///     An instance of database context.
        /// </summary>
        private readonly PublicTransportContext _db;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="db"><see cref="PublicTransportContext" /> to use during service operations.</param>
        public StopTimeRepository(PublicTransportContext db)
        {
            _db = db;
        }

        /// <summary>
        ///     Inserts a <see cref="StopTime" /> record into the database.
        /// </summary>
        /// <param name="stopTime"><see cref="StopTime" /> object to insert into the database.</param>
        /// <returns>The <see cref="StopTime" /> object corresponding to the inserted record.</returns>
        public StopTime Create(StopTime stopTime)
        {
            _db.StopTimes.Add(stopTime);
            _db.SaveChanges();
            return stopTime;
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
            return _db.StopTimes.FirstOrDefault(u => u.Id == id);
        }

        /// <summary>
        ///     Updates all of the fields of the supplied <see cref="StopTime" />.
        /// </summary>
        /// <param name="stopTime"><see cref="StopTime" /> object to update.</param>
        /// <returns>Updated <see cref="StopTime" /> object.</returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="StopTime" /> could not be found in the database.
        /// </exception>
        public StopTime Update(StopTime stopTime)
        {
            var old = Read(stopTime.Id);
            if (old == null)
            {
                throw new EntryNotFoundException();
            }

            _db.Entry(old).CurrentValues.SetValues(stopTime);
            _db.SaveChanges();
            return stopTime;
        }

        /// <summary>
        ///     Deletes the supplied <see cref="StopTime" /> from the database.
        /// </summary>
        /// <param name="stopTime"><see cref="StopTime" /> object to delete.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="StopTime" /> could not be found in the database.
        /// </exception>
        public void Delete(StopTime stopTime)
        {
            var old = Read(stopTime.Id);
            if (old == null)
            {
                throw new EntryNotFoundException();
            }

            _db.Entry(old).State = EntityState.Deleted;
            _db.SaveChanges();
        }

        /// <summary>
        ///     Returns a list of <see cref="StopTime" />s for a certain <see cref="Stop"/>.
        /// </summary>
        /// <param name="stopId">Id of the <see cref="Stop" />.</param>
        /// <returns>
        ///     Returns a list of <see cref="StopTime" />s for a certain <see cref="Stop"/>.
        /// </returns>
        public Dictionary<Route, List<StopTime>> GetFullTimetableByStopId(int stopId)
        {
            return _db.StopTimes
                .Include(st => st.Trip.Route.Agency.Street.City)
                .Where(st => st.StopId == stopId)
                .ToList()   // this enumerates, calling this for side effects (skipping this line ignores Agency include)
                .GroupBy(st => st.Trip.Route)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        /// <summary>
        ///     Returns a list of <see cref="StopTime" />s for a certain <see cref="Stop"/> which are associated with a specific <see cref="Route"/>.
        /// </summary>
        /// <returns>
        ///     Returns a list of <see cref="StopTime" />s for a certain <see cref="Stop"/> which are associated with a specific <see cref="Route"/>.
        /// </returns>
        public List<StopTime> GetRouteTimetableByStopId(StopTimeFilter filter)
        {
            var day = filter.Date?.DayOfWeek;
            Func<StopTime, bool> isActive;
            switch (day)
            {
                case DayOfWeek.Monday:
                    isActive = stopTime => stopTime.Trip.Service.Monday;
                    break;
                case DayOfWeek.Tuesday:
                    isActive = stopTime => stopTime.Trip.Service.Tuesday;
                    break;
                case DayOfWeek.Wednesday:
                    isActive = stopTime => stopTime.Trip.Service.Wednesday;
                    break;
                case DayOfWeek.Thursday:
                    isActive = stopTime => stopTime.Trip.Service.Thursday;
                    break;
                case DayOfWeek.Friday:
                    isActive = stopTime => stopTime.Trip.Service.Friday;
                    break;
                case DayOfWeek.Saturday:
                    isActive = stopTime => stopTime.Trip.Service.Saturday;
                    break;
                case DayOfWeek.Sunday:
                    isActive = stopTime => stopTime.Trip.Service.Sunday;
                    break;
                case null:
                    isActive = stopTime => true;
                    break;
                default:
                    isActive = stopTime => true;
                    break;
            }
            return _db.StopTimes.Include(st => st.Trip.Service)
                .Include(st => st.Stop.Street.City)
                .Include(st => st.Trip.Route.Agency.Street.City)
                .Where(st => st.StopId == filter.StopId)
                .Where(st => st.Trip.RouteId == filter.RouteId)
                .Where(st => !filter.Date.HasValue || (st.Trip.Service.StartDate <= filter.Date.Value 
                    && st.Trip.Service.EndDate >= filter.Date.Value))
                .Where(st => !filter.Time.HasValue || st.ArrivalTime >= filter.Time.Value)
                .Where(isActive).ToList();
        }

        /// <summary>
        ///     Fetches the full timetable for the <see cref="Route"/> with the supplied ID.
        /// </summary>
        /// <param name="routeId">ID of the route for which the timetable should be fetched.</param>
        /// <returns>
        ///     Returns all <see cref="StopTime"/>s associated with a certain <see cref="Route"/>, grouped by 
        ///     <see cref="Stop"/>s.
        /// </returns>
        public Dictionary<Stop, List<StopTime>> GetFullTimetableByRouteId(int routeId)
        {
            return _db.StopTimes
                .Include(st => st.Trip.Route)
                .Include(st => st.Stop.ParentStation)
                .Include(st => st.Stop.Street.City)
                .Where(st => st.Trip.RouteId == routeId)
                .ToList() // side-effects
                .GroupBy(st => st.Stop)
                .ToDictionary(g => g.Key, g => g.ToList());
        }
    }
}
