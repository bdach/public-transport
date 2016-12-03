using System;
using System.Collections.Generic;
using System.ServiceModel;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Exceptions;
using PublicTransport.Services.Repositories;

namespace PublicTransport.Services.Contracts
{
    /// <summary>
    ///     Service interface specifying contract for managing agency data.
    /// </summary>
    [ServiceContract]
    public interface IAgencyService : IDisposable
    {
        /// <summary>
        ///     Calls <see cref="AgencyRepository"/> create method.
        /// </summary>
        /// <param name="agency"><see cref="AgencyDto"/> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="AgencyDto"/> object successfully inserted into the database.
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        AgencyDto CreateAgency(AgencyDto agency);

        /// <summary>
        ///     Calls <see cref="AgencyRepository"/> update method.
        /// </summary>
        /// <param name="agency"><see cref="AgencyDto"/> object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="AgencyDto"/> object successfully updated in the database.
        /// </returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="AgencyDto" /> could not be found in the database.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        AgencyDto UpdateAgency(AgencyDto agency);

        /// <summary>
        ///     Calls <see cref="AgencyRepository"/> delete method.
        /// </summary>
        /// <param name="agency"><see cref="AgencyDto"/> object to be deleted from the database.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="AgencyDto" /> could not be found in the database.
        /// </exception>
        [OperationContract]
        void DeleteAgency(AgencyDto agency);

        /// <summary>
        ///     Calls <see cref="AgencyRepository"/> filtering method.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="AgencyDto"/> objects matching the filtering query.
        /// </returns>
        [OperationContract]
        List<AgencyDto> FilterAgencies(AgencyFilter filter);

        /// <summary>
        ///     Calls <see cref="StreetRepository"/> filtering method.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="StreetDto"/> objects matching the filtering query.
        /// </returns>
        [OperationContract]
        List<StreetDto> FilterStreets(StreetFilter filter);
    }
}
