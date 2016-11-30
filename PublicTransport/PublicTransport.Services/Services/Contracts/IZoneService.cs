using System;
using System.Collections.Generic;
using System.ServiceModel;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Services.Contracts
{
    [ServiceContract]
    public interface IZoneService : IDisposable
    {
        /// <summary>
        ///     Inserts a zone into the database.
        /// </summary>
        /// <param name="zone"><see cref="ZoneDto"/> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="ZoneDto"/> object successfully inserted into the database.
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        ZoneDto CreateZone(ZoneDto zone);

        /// <summary>
        ///     Updates a zone.
        /// </summary>
        /// <param name="zone"><see cref="ZoneDto"/> object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="ZoneDto"/> object successfully updated in the database.
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        ZoneDto UpdateZone(ZoneDto zone);

        /// <summary>
        ///     Deletes a zone.
        /// </summary>
        /// <param name="zone"><see cref="ZoneDto"/> object to be deleted from the database.</param>
        [OperationContract]
        void DeleteZone(ZoneDto zone);

        /// <summary>
        ///     Filters zones.
        /// </summary>
        /// <param name="name">String that the returned zones' names should contain.</param>
        /// <returns>
        ///     List of <see cref="ZoneDto"/> objects matching the filtering query.
        /// </returns>
        [OperationContract]
        List<ZoneDto> FilterZones(string name);
    }
}