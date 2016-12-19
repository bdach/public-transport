using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.ServiceModel;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Services.Contracts
{
    [ServiceContract]
    public interface IZoneService : IDisposable
    {
        /// <summary>
        ///     Creates a <see cref="Zone" /> object in the database.
        /// </summary>
        /// <param name="zone"><see cref="ZoneDto" /> object containing <see cref="Zone" /> data.</param>
        /// <returns>
        ///     <see cref="ZoneDto" /> representing the inserted <see cref="Zone" />.
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        ZoneDto CreateZone(ZoneDto zone);

        /// <summary>
        ///     Updates a <see cref="Zone" /> object in the database, using the data stored in the
        ///     <see cref="ZoneDto" /> object.
        /// </summary>
        /// <param name="zone"><see cref="ZoneDto" /> representing the object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="ZoneDto" /> object containing the updated <see cref="Zone" /> data.
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        ZoneDto UpdateZone(ZoneDto zone);

        /// <summary>
        ///     Deletes a <see cref="Zone" /> from the system.
        /// </summary>
        /// <param name="zone"><see cref="ZoneDto" /> representing the <see cref="Zone" /> to be deleted from the database.</param>
        [OperationContract]
        void DeleteZone(ZoneDto zone);

        /// <summary>
        ///     Filters <see cref="Zone" /> objects using the supplied string.
        /// </summary>
        /// <param name="name">String to filter zones by.</param>
        /// <returns>
        ///     List of <see cref="ZoneDto" /> objects matching the filtering query.
        /// </returns>
        [OperationContract]
        List<ZoneDto> FilterZones(string name);
    }
}