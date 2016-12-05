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
    ///     Service interface specifying contract for managing agency data.
    /// </summary>
    [ServiceContract]
    public interface IAgencyService : IDisposable
    {
        /// <summary>
        ///     Creates an <see cref="Agency"/> object in the database.
        /// </summary>
        /// <param name="agency"><see cref="AgencyDto" /> object containing <see cref="Agency"/> data.</param>
        /// <returns>
        ///     <see cref="AgencyDto" /> representing the inserted <see cref="Agency"/>.
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        AgencyDto CreateAgency(AgencyDto agency);

        /// <summary>
        ///     Updates an <see cref="Agency"/> object in the database, using the data stored in the
        ///     <see cref="AgencyDto" /> object.
        /// </summary>
        /// <param name="agency"><see cref="AgencyDto" /> representing the object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="AgencyDto" /> object containing the updated <see cref="Agency"/> data.
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        AgencyDto UpdateAgency(AgencyDto agency);

        /// <summary>
        ///     Deletes an <see cref="Agency"/> from the system.
        /// </summary>
        /// <param name="agency"><see cref="AgencyDto" /> representing the <see cref="Agency"/> to be deleted from the database.</param>
        [OperationContract]
        void DeleteAgency(AgencyDto agency);

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
