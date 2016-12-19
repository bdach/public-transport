using System;
using System.Collections.Generic;
using System.ServiceModel;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Services.Contracts
{
    [ServiceContract]
    public interface IRouteService : IDisposable
    {
        /// <summary>
        ///     Returns a list of <see cref="StopTimeDto" />s for a certain <see cref="Stop" /> which are associated with a specific
        ///     <see cref="Route" />.
        /// </summary>
        /// <returns>
        ///     Returns a list of <see cref="StopTimeDto" />s for a certain <see cref="Stop" /> which are associated with a specific
        ///     <see cref="Route" />.
        /// </returns>
        [OperationContract]
        List<StopTimeDto> GetRouteTimetableByStopId(StopTimeFilter filter);

        /// <summary>
        ///     Creates a <see cref="Calendar"/> object in the database.
        /// </summary>
        /// <param name="calendar"><see cref="CalendarDto" /> object containing <see cref="Calendar"/> data.</param>
        /// <returns>
        ///     <see cref="CalendarDto" /> representing the inserted <see cref="Calendar"/>.
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        CalendarDto CreateCalendar(CalendarDto calendar);

        /// <summary>
        ///     Updates a <see cref="Calendar"/> object in the database, using the data stored in the
        ///     <see cref="CalendarDto" /> object.
        /// </summary>
        /// <param name="calendar"><see cref="CalendarDto" /> representing the object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="CalendarDto" /> object containing the updated <see cref="Calendar"/> data.
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        CalendarDto UpdateCalendar(CalendarDto calendar);

        /// <summary>
        ///     Filters <see cref="Agency"/> objects using the supplied <see cref="AgencyFilter"/>.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="AgencyDto"/> objects matching the filtering query.
        /// </returns>
        [OperationContract]
        List<AgencyDto> FilterAgencies(AgencyFilter filter);

        /// <summary>
        ///     Creates a <see cref="Route"/> object in the database.
        /// </summary>
        /// <param name="route"><see cref="RouteDto" /> object containing <see cref="FareAttribute"/> data.</param>
        /// <returns>
        ///     <see cref="RouteDto" /> representing the inserted <see cref="Route"/>.
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        RouteDto CreateRoute(RouteDto route);

        /// <summary>
        ///     Updates a <see cref="Route"/> object in the database, using the data stored in the
        ///     <see cref="RouteDto" /> object.
        /// </summary>
        /// <param name="route"><see cref="RouteDto" /> representing the object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="RouteDto" /> object containing the updated <see cref="Route"/> data.
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        RouteDto UpdateRoute(RouteDto route);

        /// <summary>
        ///     Deletes a <see cref="Route"/> from the system.
        /// </summary>
        /// <param name="route"><see cref="RouteDto" /> representing the <see cref="Route"/> to be deleted from the database.</param>
        [OperationContract]
        void DeleteRoute(RouteDto route);

        /// <summary>
        ///     Filters <see cref="Route"/> objects using the supplied <see cref="RouteFilter"/>.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="RouteDto"/> objects matching the filtering query.
        /// </returns>
        [OperationContract]
        List<RouteDto> FilterRoutes(RouteFilter filter);

        /// <summary>
        ///     Filters <see cref="Stop"/> objects using the supplied <see cref="StopFilter"/>.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="StopDto"/> objects matching the filtering query.
        /// </returns>
        [OperationContract]
        List<StopDto> FilterStops(StopFilter filter);

        /// <summary>
        ///     Returns a list of <see cref="Stop" />s associated with a certain <see cref="Route" />.
        /// </summary>
        /// <param name="routeId">Id of the <see cref="Route" />.</param>
        /// <returns>
        ///     A list of <see cref="StopDto" />s associated with a certain <see cref="Route" />.
        /// </returns>
        [OperationContract]
        List<StopDto> GetStopsByRouteId(int routeId);

        /// <summary>
        ///     Creates a <see cref="Trip"/> object in the database.
        /// </summary>
        /// <param name="trip"><see cref="TripDto" /> object containing <see cref="Trip"/> data.</param>
        /// <returns>
        ///     <see cref="TripDto" /> representing the inserted <see cref="Trip"/>.
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        TripDto CreateTrip(TripDto trip);

        /// <summary>
        ///     Updates a <see cref="Trip"/> object in the database, using the data stored in the
        ///     <see cref="TripDto" /> object.
        /// </summary>
        /// <param name="trip"><see cref="TripDto" /> representing the object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="TripDto" /> object containing the updated <see cref="Trip"/> data.
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        TripDto UpdateTrip(TripDto trip);

        /// <summary>
        ///     Deletes a <see cref="Trip"/> from the system.
        /// </summary>
        /// <param name="trip"><see cref="TripDto" /> representing the <see cref="Trip"/> to be deleted from the database.</param>
        [OperationContract]
        void DeleteTrip(TripDto trip);

        /// <summary>
        ///     Updates the <see cref="StopTime" />s associated with this trip.
        /// </summary>
        /// <param name="tripId">ID number of trip to update.</param>
        /// <param name="stops">The stops which should be saved.</param>
        /// <returns>List of <see cref="StopTimeDto"/>s after saving.</returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        List<StopTimeDto> UpdateStops(int tripId, List<StopTimeDto> stops);

        /// <summary>
        ///     Return a list of <see cref="Stop" />s that are assinged to the provided <see cref="Trip" />.
        /// </summary>
        /// <param name="trip"><see cref="Trip" /> to filter stops by.</param>
        /// <returns>
        ///     A list of <see cref="StopDto" />s that are assinged to the provided <see cref="Trip" />.
        /// </returns>
        [OperationContract]
        List<StopTimeDto> GetTripStops(TripDto trip);
    }
}