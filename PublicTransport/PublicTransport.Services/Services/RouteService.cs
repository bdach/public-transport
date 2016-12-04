using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.Contracts;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.DataTransfer.Converters;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Exceptions;
using PublicTransport.Services.Repositories;

namespace PublicTransport.Services
{
    /// <summary>
    ///     Service used to manage route data.
    /// </summary>
    public class RouteService : IRouteService
    {
        /// <summary>
        ///     Service used to fetch <see cref="Agency" /> data from the database.
        /// </summary>
        private readonly AgencyRepository _agencyRepository;

        /// <summary>
        ///     Service used to fetch <see cref="Calendar" /> data from the database.
        /// </summary>
        private readonly CalendarRepository _calendarRepository;

        /// <summary>
        ///     Service used to fetch <see cref="Route" /> data from the database.
        /// </summary>
        private readonly RouteRepository _routeRepository;

        /// <summary>
        ///     Service used to fetch <see cref="Stop" /> data from the database.
        /// </summary>
        private readonly StopRepository _stopRepository;

        /// <summary>
        ///     Service used to fetch <see cref="StopTime" /> data from the database.
        /// </summary>
        private readonly StopTimeRepository _stopTimeRepository;

        /// <summary>
        ///     Service used to fetch <see cref="Trip" /> data from the database.
        /// </summary>
        private readonly TripRepository _tripRepository;

        /// <summary>
        ///     Used to convert <see cref="Agency" /> objects to <see cref="AgencyDto"/> objects and back.
        /// </summary>
        private readonly IConverter<Agency, AgencyDto> _agencyConverter;

        /// <summary>
        ///     Used to convert <see cref="Stop" /> objects to <see cref="StopDto"/> objects and back.
        /// </summary>
        private readonly IConverter<Stop, StopDto> _stopConverter;

        /// <summary>
        ///     Used to convert <see cref="Route" /> objects to <see cref="RouteDto"/> objects and back.
        /// </summary>
        private readonly IConverter<Route, RouteDto> _routeConverter;

        /// <summary>
        ///     Used to convert <see cref="Trip" /> objects to <see cref="TripDto"/> objects and back.
        /// </summary>
        private readonly IConverter<Trip, TripDto> _tripConverter;

        /// <summary>
        ///     Used to convert <see cref="Calendar" /> objects to <see cref="CalendarDto"/> objects and back.
        /// </summary>
        private readonly IConverter<Calendar, CalendarDto> _calendarConverter;

        /// <summary>
        ///     Used to convert <see cref="StopTime" /> objects to <see cref="StopTime"/> objects and back.
        /// </summary>
        private readonly IConverter<StopTime, StopTimeDto> _stopTimeConverter;

        /// <summary>
        ///     Database context common for services in this service used to access data.
        /// </summary>
        private readonly PublicTransportContext _db;

        /// <summary>
        ///     Determines whether the database context has been disposed.
        /// </summary>
        private bool _disposed;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public RouteService()
        {
            _db = new PublicTransportContext();
            #region Repositories

            _agencyRepository = new AgencyRepository(_db);
            _calendarRepository = new CalendarRepository(_db);
            _routeRepository = new RouteRepository(_db);
            _stopRepository = new StopRepository(_db);
            _stopTimeRepository = new StopTimeRepository(_db);
            _tripRepository = new TripRepository(_db);

            #endregion

            #region Converters

            _agencyConverter = new AgencyConverter();
            _calendarConverter = new CalendarConverter();
            _routeConverter = new RouteConverter();
            _stopConverter = new StopConverter();
            _stopTimeConverter = new StopTimeConverter();
            _tripConverter = new TripConverter();

            #endregion
        }

        #region Stop time methods

        /// <summary>
        ///     Returns a list of <see cref="StopTimeDto" />s for a certain <see cref="Stop" /> which are associated with a specific
        ///     <see cref="Route" />.
        /// </summary>
        /// <returns>
        ///     Returns a list of <see cref="StopTimeDto" />s for a certain <see cref="Stop" /> which are associated with a specific
        ///     <see cref="Route" />.
        /// </returns>
        public List<StopTimeDto> GetRouteTimetableByStopId(StopTimeFilter filter)
        {
            return _stopTimeRepository
                .GetRouteTimetableByStopId(filter)
                .Select(_stopTimeConverter.GetDto)
                .ToList();
        }

        #endregion

        #region Calendar methods

        /// <summary>
        ///     Inserts a calendar record into the database.
        /// </summary>
        /// <param name="calendar"><see cref="CalendarDto" /> object to insert into the database.</param>
        /// <returns>The <see cref="CalendarDto" /> object corresponding to the inserted record.</returns>
        /// <exception cref="ValidationFaultException">
        ///     Thrown when the data contained in the received DTO contains validation errors.
        /// </exception>
        public CalendarDto CreateCalendar(CalendarDto calendar)
        {
            try
            {
                var result = _calendarRepository.Create(_calendarConverter.GetEntity(calendar));
                return _calendarConverter.GetDto(result);
            }
            catch (DbEntityValidationException ex)
            {
                throw new ValidationFaultException(ex);
            }
        }

        /// <summary>
        ///     Updates all of the fields of the supplied calendar.
        /// </summary>
        /// <param name="calendar"><see cref="CalendarDto" /> object to update.</param>
        /// <returns>Updated <see cref="CalendarDto" /> object.</returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied calendar could not be found in the database.
        /// </exception>
        /// <exception cref="ValidationFaultException">
        ///     Thrown when the data contained in the received DTO contains validation errors.
        /// </exception>
        public CalendarDto UpdateCalendar(CalendarDto calendar)
        {
            try
            {
                var result = _calendarRepository.Update(_calendarConverter.GetEntity(calendar));
                return _calendarConverter.GetDto(result);
            }
            catch (DbEntityValidationException ex)
            {
                throw new ValidationFaultException(ex);
            }
        }

        #endregion

        #region Agency methods

        /// <summary>
        ///     Selects all the <see cref="Agency" /> objects that match all the criteria specified by the
        ///     <see cref="AgencyFilter" /> object. The returned agencies' name, street name and city name strings all contain the
        ///     parameters supplied in the <see cref="filter" /> parameter.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>List of items satisfying the supplied query.</returns>
        public List<AgencyDto> FilterAgencies(AgencyFilter filter)
        {
            return _agencyRepository
                .FilterAgencies(filter)
                .Select(_agencyConverter.GetDto)
                .ToList();
        }

        #endregion

        #region Route methods

        /// <summary>
        ///     Inserts a route record into the database.
        /// </summary>
        /// <param name="route"><see cref="RouteDto" /> object to insert into the database.</param>
        /// <returns>The <see cref="RouteDto" /> object corresponding to the inserted record.</returns>
        /// <exception cref="ValidationFaultException">
        ///     Thrown when the data contained in the received DTO contains validation errors.
        /// </exception>
        public RouteDto CreateRoute(RouteDto route)
        {
            try
            {
                var result = _routeRepository.Create(_routeConverter.GetEntity(route));
                return _routeConverter.GetDto(result);
            }
            catch (DbEntityValidationException ex)
            {
                throw new ValidationFaultException(ex);
            }
        }

        /// <summary>
        ///     Updates all of the fields of the supplied <see cref="Route" />.
        /// </summary>
        /// <param name="route"><see cref="Route" /> object to update.</param>
        /// <returns>Updated <see cref="Route" /> object.</returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Route" /> could not be found in the database.
        /// </exception>
        /// <exception cref="ValidationFaultException">
        ///     Thrown when the data contained in the received DTO contains validation errors.
        /// </exception>
        public RouteDto UpdateRoute(RouteDto route)
        {
            try
            {
                var result = _routeRepository.Update(_routeConverter.GetEntity(route));
                return _routeConverter.GetDto(result);
            }
            catch (DbEntityValidationException ex)
            {
                throw new ValidationFaultException(ex);
            }
        }

        /// <summary>
        ///     Deletes the supplied route from the database.
        /// </summary>
        /// <param name="route"><see cref="RouteDto" /> object to delete.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="RouteDto" /> could not be found in the database.
        /// </exception>
        public void DeleteRoute(RouteDto route)
        {
            _routeRepository.Delete(_routeConverter.GetEntity(route));
        }

        /// <summary>
        ///     Filters routes using the supplied <see cref="RouteFilter" />.
        /// </summary>
        /// <param name="filter">Filter to use while searching.</param>
        /// <returns>List of routes satisfying the search criteria.</returns>
        public List<RouteDto> FilterRoutes(RouteFilter filter)
        {
            return _routeRepository
                .FilterRoutes(filter)
                .Select(_routeConverter.GetDto)
                .ToList();
        }

        #endregion

        #region Stop methods

        /// <summary>
        ///     Selects all the <see cref="Stop" /> objects that match all the criteria specified by the
        ///     <see cref="StopFilter" /> object. The returned name strings all contain the
        ///     parameters supplied in the <see cref="filter" /> parameter.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>List of items satisfying the supplied query.</returns>
        public List<StopDto> FilterStops(StopFilter filter)
        {
            return _stopRepository
                .FilterStops(filter)
                .Select(_stopConverter.GetDto)
                .ToList();
        }

        /// <summary>
        ///     Returns a list of <see cref="Stop" />s associated with a certain <see cref="Route" />.
        /// </summary>
        /// <param name="routeId">Id of the <see cref="Route" />.</param>
        /// <returns>
        ///     Returns a list of <see cref="Stop" />s associated with a certain <see cref="Route" />.
        /// </returns>
        public List<StopDto> GetStopsByRouteId(int routeId)
        {
            return _stopRepository
                .GetStopsByRouteId(routeId)
                .Select(_stopConverter.GetDto)
                .ToList();
        }

        #endregion

        #region Trip methods

        /// <summary>
        ///     Inserts a trip record into the database.
        /// </summary>
        /// <param name="trip"><see cref="TripDto" /> object to insert into the database.</param>
        /// <returns>The <see cref="TripDto" /> object corresponding to the inserted record.</returns>
        /// <exception cref="ValidationFaultException">
        ///     Thrown when the data contained in the received DTO contains validation errors.
        /// </exception>
        public TripDto CreateTrip(TripDto trip)
        {
            try
            {
                var result = _tripRepository.Create(_tripConverter.GetEntity(trip));
                return _tripConverter.GetDto(result);
            }
            catch (DbEntityValidationException ex)
            {
                throw new ValidationFaultException(ex);
            }
        }

        /// <summary>
        ///     Updates all of the fields of the supplied trip.
        /// </summary>
        /// <param name="trip"><see cref="TripDto" /> object to update.</param>
        /// <returns>Updated <see cref="TripDto" /> object.</returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Trip" /> could not be found in the database.
        /// </exception>
        /// <exception cref="ValidationFaultException">
        ///     Thrown when the data contained in the received DTO contains validation errors.
        /// </exception>
        public TripDto UpdateTrip(TripDto trip)
        {
            try
            {
                var result = _tripRepository.Update(_tripConverter.GetEntity(trip));
                return _tripConverter.GetDto(result);
            }
            catch (DbEntityValidationException ex)
            {
                throw new ValidationFaultException(ex);
            }
        }

        /// <summary>
        ///     Deletes the supplied trip from the database.
        /// </summary>
        /// <param name="trip"><see cref="TripDto" /> object to delete.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="TripDto" /> could not be found in the database.
        /// </exception>
        public void DeleteTrip(TripDto trip)
        {
            _tripRepository.Delete(_tripConverter.GetEntity(trip));
        }

        /// <summary>
        ///     Updates the <see cref="StopTime" /> associated with this trip.
        /// </summary>
        /// <param name="tripId">ID number of trip to update</param>
        /// <param name="stops">The stops which should be saved.</param>
        /// <returns>List of stops after saving.</returns>
        public List<StopTimeDto> UpdateStops(int tripId, List<StopTimeDto> stops)
        {
            try
            {
                var results = _tripRepository.UpdateStops(tripId, stops.Select(_stopTimeConverter.GetEntity).ToList());
                return results.Select(_stopTimeConverter.GetDto).ToList();
            }
            catch (DbEntityValidationException ex)
            {
                throw new ValidationFaultException(ex);
            }
        }

        /// <summary>
        ///     Return a list of <see cref="Stop" />s that are assinged to the provided <see cref="Trip" />.
        /// </summary>
        /// <param name="trip"><see cref="Trip" /> to filter stops by.</param>
        /// <returns>
        ///     Return a list of <see cref="Stop" />s that are assinged to the provided <see cref="Trip" />.
        /// </returns>
        public List<StopTimeDto> GetTripStops(TripDto trip)
        {
            return _tripRepository
                .GetTripStops(_tripConverter.GetEntity(trip))
                .Select(_stopTimeConverter.GetDto)
                .ToList();
        }

        #endregion

        /// <summary>
        ///     Disposes the database context if not disposed already.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return; 
            _db.Dispose();
            _disposed = true;
        }
    }
}