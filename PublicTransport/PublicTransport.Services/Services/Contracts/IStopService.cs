using System;
using System.Collections.Generic;
using System.ServiceModel;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Exceptions;
using PublicTransport.Services.Repositories;

namespace PublicTransport.Services.Contracts
{
    /// <summary>
    ///     Service interface specifying contract for managing stop data.
    /// </summary>
    [ServiceContract]
    public interface IStopService : IDisposable
    {
        /// <summary>
        ///     Calls <see cref="StopRepository"/> create method.
        /// </summary>
        /// <param name="stop"><see cref="Stop"/> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="StopRepository"/> object successfully inserted into the database.
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        StopDto CreateStop(StopDto stop);

        /// <summary>
        ///     Calls <see cref="StopRepository"/> update method.
        /// </summary>
        /// <param name="stop"><see cref="Stop"/> object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="Stop"/> object successfully updated in the database.
        /// </returns>
        /// <exception cref="Stop">
        ///     Thrown when the supplied <see cref="EntryNotFoundException" /> could not be found in the database.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        StopDto UpdateStop(StopDto stop);

        /// <summary>
        ///     Calls <see cref="StopRepository"/> delete method.
        /// </summary>
        /// <param name="stop"><see cref="Stop"/> object to be deleted from the database.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Stop" /> could not be found in the database.
        /// </exception>
        [OperationContract]
        void DeleteStop(StopDto stop);

        /// <summary>
        ///     Calls <see cref="StopRepository"/> filtering method.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="Stop"/> objects matching the filtering query.
        /// </returns>
        [OperationContract]
        List<StopDto> FilterStops(StopFilter filter);

        /// <summary>
        ///     Calls <see cref="ZoneRepository"/> filtering method.
        /// </summary>
        /// <param name="name">Filtering parameter.</param>
        /// <returns>
        ///     List of <see cref="Zone"/> objects matching the filtering query.
        /// </returns>
        [OperationContract]
        List<ZoneDto> FilterZones(string name);

        /// <summary>
        ///     Calls <see cref="StreetRepository"/> filtering method.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="Street"/> objects matching the filtering query.
        /// </returns>
        [OperationContract]
        List<StreetDto> FilterStreets(StreetFilter filter);
    }

}
