using System.Collections.Generic;
using System.ServiceModel;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Services.Contracts
{
    [ServiceContract]
    public interface IRouteService
    {
        /// <summary>
        ///     Returns a list of stop times for a certain <see cref="Stop" /> which are associated with a specific
        ///     <see cref="Route" />.
        /// </summary>
        /// <returns>
        ///     Returns a list of stop times for a certain <see cref="Stop" /> which are associated with a specific
        ///     <see cref="Route" />.
        /// </returns>
        [OperationContract]
        List<StopTimeDto> GetRouteTimetableByStopId(StopTimeFilter filter);

        /// <summary>
        ///     Inserts a calendar record into the database.
        /// </summary>
        /// <param name="calendar"><see cref="CalendarDto" /> object to insert into the database.</param>
        /// <returns>The <see cref="CalendarDto" /> object corresponding to the inserted record.</returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        CalendarDto CreateCalendar(CalendarDto calendar);

        /// <summary>
        ///     Updates all of the fields of the supplied calendar.
        /// </summary>
        /// <param name="calendar"><see cref="CalendarDto" /> object to update.</param>
        /// <returns>Updated <see cref="CalendarDto" /> object.</returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        CalendarDto UpdateCalendar(CalendarDto calendar);

        /// <summary>
        ///     Selects all the agency objects that match all the criteria specified by the
        ///     <see cref="AgencyFilter" /> object. The returned agencies' name, street name and city name strings all contain the
        ///     parameters supplied in the <see cref="filter" /> parameter.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>List of items satisfying the supplied query.</returns>
        [OperationContract]
        List<AgencyDto> FilterAgencies(AgencyFilter filter);

        /// <summary>
        ///     Inserts a route record into the database.
        /// </summary>
        /// <param name="route"><see cref="RouteDto" /> object to insert into the database.</param>
        /// <returns>The <see cref="RouteDto" /> object corresponding to the inserted record.</returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        RouteDto CreateRoute(RouteDto route);

        /// <summary>
        ///     Updates all of the fields of the supplied route.
        /// </summary>
        /// <param name="route"><see cref="RouteDto" /> object to update.</param>
        /// <returns>Updated <see cref="RouteDto" /> object.</returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        RouteDto UpdateRoute(RouteDto route);

        /// <summary>
        ///     Deletes the supplied route from the database.
        /// </summary>
        /// <param name="route"><see cref="RouteDto" /> object to delete.</param>
        [OperationContract]
        void DeleteRoute(RouteDto route);

        /// <summary>
        ///     Filters routes using the supplied <see cref="RouteFilter" />.
        /// </summary>
        /// <param name="filter">Filter to use while searching.</param>
        /// <returns>List of routes satisfying the search criteria.</returns>
        [OperationContract]
        List<RouteDto> FilterRoutes(RouteFilter filter);

        /// <summary>
        ///     Selects all the stop objects that match all the criteria specified by the
        ///     <see cref="StopFilter" /> object. The returned name strings all contain the
        ///     parameters supplied in the <see cref="filter" /> parameter.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>List of items satisfying the supplied query.</returns>
        [OperationContract]
        List<StopDto> FilterStops(StopFilter filter);

        /// <summary>
        ///     Returns a list of stops associated with a certain <see cref="Route" />.
        /// </summary>
        /// <param name="routeId">Id of the <see cref="Route" />.</param>
        /// <returns>
        ///     Returns a list of stops associated with a certain <see cref="Route" />.
        /// </returns>
        [OperationContract]
        List<StopDto> GetStopsByRouteId(int routeId);

        /// <summary>
        ///     Inserts a trip record into the database.
        /// </summary>
        /// <param name="trip"><see cref="TripDto" /> object to insert into the database.</param>
        /// <returns>The <see cref="TripDto" /> object corresponding to the inserted record.</returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        TripDto CreateTrip(TripDto trip);

        /// <summary>
        ///     Updates all of the fields of the supplied trip.
        /// </summary>
        /// <param name="trip"><see cref="TripDto" /> object to update.</param>
        /// <returns>Updated <see cref="TripDto" /> object.</returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        TripDto UpdateTrip(TripDto trip);

        /// <summary>
        ///     Deletes the supplied trip from the database.
        /// </summary>
        /// <param name="trip"><see cref="TripDto" /> object to delete.</param>
        [OperationContract]
        void DeleteTrip(TripDto trip);

        /// <summary>
        ///     Updates the stop times associated with this trip.
        /// </summary>
        /// <param name="tripId">ID number of trip to update</param>
        /// <param name="stops">The stops which should be saved.</param>
        /// <returns>List of stops after saving.</returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        List<StopTimeDto> UpdateStops(int tripId, List<StopTimeDto> stops);

        /// <summary>
        ///     Return a list of stops that are assinged to the provided <see cref="Trip" />.
        /// </summary>
        /// <param name="trip"><see cref="Trip" /> to filter stops by.</param>
        /// <returns>
        ///     Return a list of stops that are assinged to the provided <see cref="Trip" />.
        /// </returns>
        [OperationContract]
        List<StopTimeDto> GetTripStops(TripDto trip);
    }
}