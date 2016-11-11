using System.Collections.Generic;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer.Filters;

namespace PublicTransport.Services.UnitsOfWork
{
    /// <summary>
    ///     Unit of work used to manage route data.
    /// </summary>
    public class RouteUnitOfWork
    {
        /// <summary>
        ///     Service used to fetch <see cref="Agency" /> data from the database.
        /// </summary>
        private readonly AgencyService _agencyService;

        /// <summary>
        ///     Service used to fetch <see cref="Calendar" /> data from the database.
        /// </summary>
        private readonly CalendarService _calendarService;

        /// <summary>
        ///     Database context common for services in this unit of work used to access data.
        /// </summary>
        private readonly PublicTransportContext _db;

        /// <summary>
        ///     Service used to fetch <see cref="Route" /> data from the database.
        /// </summary>
        private readonly RouteService _routeService;

        /// <summary>
        ///     Service used to fetch <see cref="Stop" /> data from the database.
        /// </summary>
        private readonly StopService _stopService;

        /// <summary>
        ///     Service used to fetch <see cref="StopTime" /> data from the database.
        /// </summary>
        private readonly StopTimeService _stopTimeService;

        /// <summary>
        ///     Service used to fetch <see cref="Trip" /> data from the database.
        /// </summary>
        private readonly TripService _tripService;

        /// <summary>
        ///     Determines whether the database context has been disposed.
        /// </summary>
        private bool _disposed;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public RouteUnitOfWork()
        {
            _db = new PublicTransportContext();
            _agencyService = new AgencyService(_db);
            _calendarService = new CalendarService(_db);
            _routeService = new RouteService(_db);
            _stopService = new StopService(_db);
            _stopTimeService = new StopTimeService(_db);
            _tripService = new TripService(_db);
        }

        #region Stop time methods

        /// <summary>
        ///     Returns a list of <see cref="StopTime" />s for a certain <see cref="Stop" /> which are associated with a specific
        ///     <see cref="Route" />.
        /// </summary>
        /// <returns>
        ///     Returns a list of <see cref="StopTime" />s for a certain <see cref="Stop" /> which are associated with a specific
        ///     <see cref="Route" />.
        /// </returns>
        public List<StopTime> GetRouteTimetableByStopId(IStopTimeFilter filter)
        {
            return _stopTimeService.GetRouteTimetableByStopId(filter);
        }

        #endregion

        /// <summary>
        ///     Disposes the database context if not disposed already.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            _db.Dispose();
            _disposed = true;
        }

        #region Agency methods

        #region Calendar methods

        /// <summary>
        ///     Inserts an <see cref="Calendar" /> record into the database.
        /// </summary>
        /// <param name="calendar"><see cref="Calendar" /> object to insert into the database.</param>
        /// <returns>The <see cref="Calendar" /> object corresponding to the inserted record.</returns>
        public Calendar CreateCalendar(Calendar calendar)
        {
            return _calendarService.Create(calendar);
        }

        /// <summary>
        ///     Updates all of the fields of the supplied <see cref="Calendar" />.
        /// </summary>
        /// <param name="calendar"><see cref="Calendar" /> object to update.</param>
        /// <returns>Updated <see cref="Calendar" /> object.</returns>
        /// <exception cref="Exceptions.EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Calendar" /> could not be found in the
        ///     database.
        /// </exception>
        public Calendar UpdateCalendar(Calendar calendar)
        {
            return _calendarService.Update(calendar);
        }

        #endregion

        /// <summary>
        ///     Selects all the <see cref="Agency" /> objects that match all the criteria specified by the
        ///     <see cref="IAgencyFilter" /> object. The returned agencies' name, street name and city name strings all contain the
        ///     parameters supplied in the <see cref="filter" /> parameter.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>List of items satisfying the supplied query.</returns>
        public List<Agency> FilterAgencies(IAgencyFilter filter)
        {
            return _agencyService.FilterAgencies(filter);
        }

        #endregion

        #region Route methods

        /// <summary>
        ///     Inserts a <see cref="Route" /> record into the database.
        /// </summary>
        /// <param name="route"><see cref="Route" /> object to insert into the database.</param>
        /// <returns>The <see cref="Route" /> object corresponding to the inserted record.</returns>
        public Route CreateRoute(Route route)
        {
            return _routeService.Create(route);
        }

        /// <summary>
        ///     Updates all of the fields of the supplied <see cref="Route" />.
        /// </summary>
        /// <param name="route"><see cref="Route" /> object to update.</param>
        /// <returns>Updated <see cref="Route" /> object.</returns>
        /// <exception cref="Exceptions.EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Route" /> could not be found in the
        ///     database.
        /// </exception>
        public Route UpdateRoute(Route route)
        {
            return _routeService.Update(route);
        }

        /// <summary>
        ///     Deletes the supplied <see cref="Route" /> from the database.
        /// </summary>
        /// <param name="route"><see cref="Route" /> object to delete.</param>
        /// <exception cref="Exceptions.EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Route" /> could not be found in the
        ///     database.
        /// </exception>
        public void DeleteRoute(Route route)
        {
            _routeService.Delete(route);
        }

        /// <summary>
        ///     Filters routes using the supplied <see cref="IRouteFilter" />.
        /// </summary>
        /// <param name="filter">Filter to use while searching.</param>
        /// <returns>List of routes satisfying the search criteria.</returns>
        public List<Route> FilterRoutes(IRouteFilter filter)
        {
            return _routeService.FilterRoutes(filter);
        }

        #endregion

        #region Stop methods

        /// <summary>
        ///     Selects all the <see cref="Stop" /> objects that match all the criteria specified by the
        ///     <see cref="IStopFilter" /> object. The returned name strings all contain the
        ///     parameters supplied in the <see cref="filter" /> parameter.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>List of items satisfying the supplied query.</returns>
        public List<Stop> FilterStops(IStopFilter filter)
        {
            return _stopService.FilterStops(filter);
        }

        /// <summary>
        ///     Returns a list of <see cref="Stop" />s associated with a certain <see cref="Route" />.
        /// </summary>
        /// <param name="routeId">Id of the <see cref="Route" />.</param>
        /// <returns>
        ///     Returns a list of <see cref="Stop" />s associated with a certain <see cref="Route" />.
        /// </returns>
        public List<Stop> GetStopsByRouteId(int routeId)
        {
            return _stopService.GetStopsByRouteId(routeId);
        }

        #endregion

        #region Trip methods

        /// <summary>
        ///     Inserts a <see cref="Trip" /> record into the database.
        /// </summary>
        /// <param name="trip"><see cref="Trip" /> object to insert into the database.</param>
        /// <returns>The <see cref="Trip" /> object corresponding to the inserted record.</returns>
        public Trip CreateTrip(Trip trip)
        {
            return _tripService.Create(trip);
        }

        /// <summary>
        ///     Updates all of the fields of the supplied <see cref="Trip" />.
        /// </summary>
        /// <param name="trip"><see cref="Trip" /> object to update.</param>
        /// <returns>Updated <see cref="Trip" /> object.</returns>
        /// <exception cref="Exceptions.EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Trip" /> could not be found in the
        ///     database.
        /// </exception>
        public Trip UpdateTrip(Trip trip)
        {
            return _tripService.Update(trip);
        }

        /// <summary>
        ///     Deletes the supplied <see cref="Trip" /> from the database.
        /// </summary>
        /// <param name="trip"><see cref="Trip" /> object to delete.</param>
        /// <exception cref="Exceptions.EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Trip" /> could not be found in the
        ///     database.
        /// </exception>
        public void DeleteTrip(Trip trip)
        {
            _tripService.Delete(trip);
        }

        /// <summary>
        ///     Updates the <see cref="StopTime" /> associated with this trip.
        /// </summary>
        /// <param name="tripId">ID number of trip to update</param>
        /// <param name="stops">The stops which should be saved.</param>
        /// <returns>List of stops after saving.</returns>
        public List<StopTime> UpdateStops(int tripId, List<StopTime> stops)
        {
            return _tripService.UpdateStops(tripId, stops);
        }

        /// <summary>
        ///     Return a list of <see cref="Stop" />s that are assinged to the provided <see cref="Trip" />.
        /// </summary>
        /// <param name="trip"><see cref="Trip" /> to filter stops by.</param>
        /// <returns>
        ///     Return a list of <see cref="Stop" />s that are assinged to the provided <see cref="Trip" />.
        /// </returns>
        public List<StopTime> GetTripStops(Trip trip)
        {
            return _tripService.GetTripStops(trip);
        }

        #endregion
    }
}