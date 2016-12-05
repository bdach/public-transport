using System;
using System.Collections.Generic;
using System.ServiceModel;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Services.Contracts
{
    /// <summary>
    ///     Service interface specifying contract for managing stop data.
    /// </summary>
    [ServiceContract]
    public interface IStopService : IDisposable
    {
        /// <summary>
        ///     Creates a <see cref="Stop"/> object in the database.
        /// </summary>
        /// <param name="stop"><see cref="StopDto" /> object containing <see cref="Stop"/> data.</param>
        /// <returns>
        ///     <see cref="StopDto" /> representing the inserted <see cref="Stop"/>.
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        StopDto CreateStop(StopDto stop);

        /// <summary>
        ///     Updates a <see cref="Stop"/> object in the database, using the data stored in the
        ///     <see cref="StopDto" /> object.
        /// </summary>
        /// <param name="stop"><see cref="StopDto" /> representing the object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="StopDto" /> object containing the updated <see cref="Stop"/> data.
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        StopDto UpdateStop(StopDto stop);

        /// <summary>
        ///     Deletes a <see cref="Stop"/> from the system.
        /// </summary>
        /// <param name="stop"><see cref="StopDto" /> representing the <see cref="Stop"/> to be deleted from the database.</param>
        [OperationContract]
        void DeleteStop(StopDto stop);

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
        ///     Filters <see cref="Zone"/> objects using the supplied string.
        /// </summary>
        /// <param name="name">String to filter zones by.</param>
        /// <returns>
        ///     List of <see cref="ZoneDto"/> objects matching the filtering query.
        /// </returns>
        [OperationContract]
        List<ZoneDto> FilterZones(string name);

        /// <summary>
        ///     Filters <see cref="Street" /> objects using the supplied <see cref="StreetFilter" />.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="StreetDto" /> objects matching the filtering query.
        /// </returns>
        [OperationContract]
        List<StreetDto> FilterStreets(StreetFilter filter);
    }

}
