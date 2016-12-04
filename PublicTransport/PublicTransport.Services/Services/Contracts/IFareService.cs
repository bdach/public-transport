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
    ///     Service interface specifying contract for managing city data.
    /// </summary>
    [ServiceContract]
    public interface IFareService : IDisposable
    {
        /// <summary>
        ///     Calls <see cref="FareAttributeRepository"/> create method.
        /// </summary>
        /// <param name="fareAttribute"><see cref="FareAttribute"/> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="FareAttribute"/> object successfully inserted into the database.
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        FareAttributeDto CreateFareAttribute(FareAttributeDto fareAttribute);

        /// <summary>
        ///     Calls <see cref="FareAttributeRepository"/> update method.
        /// </summary>
        /// <param name="fareAttribute"><see cref="FareAttribute"/> object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="FareAttribute"/> object successfully updated in the database.
        /// </returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="FareAttribute" /> could not be found in the database.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        FareAttributeDto UpdateFareAttribute(FareAttributeDto fareAttribute);

        /// <summary>
        ///     Calls <see cref="FareAttributeRepository"/> delete method.
        /// </summary>
        /// <param name="fareAttribute"><see cref="FareAttribute"/> object to be deleted from the database.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="FareAttribute" /> could not be found in the database.
        /// </exception>
        [OperationContract]
        void DeleteFareAttribute(FareAttributeDto fareAttribute);

        /// <summary>
        ///     Calls <see cref="FareRuleRepository"/> create method.
        /// </summary>
        /// <param name="fareRule"><see cref="FareRule"/> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="FareRule"/> object successfully inserted into the database.
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        FareRuleDto CreateFareRule(FareRuleDto fareRule);

        /// <summary>
        ///     Calls <see cref="FareRuleRepository"/> update method.
        /// </summary>
        /// <param name="fareRule"><see cref="FareRule"/> object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="FareRule"/> object successfully updated in the database.
        /// </returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="FareRule" /> could not be found in the database.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        FareRuleDto UpdateFareRule(FareRuleDto fareRule);

        /// <summary>
        ///     Calls <see cref="FareRuleRepository"/> delete method.
        /// </summary>
        /// <param name="fareRule"><see cref="FareRule"/> object to be deleted from the database.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="FareRule" /> could not be found in the database.
        /// </exception>
        [OperationContract]
        void DeleteFareRule(FareRuleDto fareRule);

        /// <summary>
        ///     Calls <see cref="FareAttributeRepository"/> filtering method.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="FareAttribute"/> objects matching the filtering query.
        /// </returns>
        [OperationContract]
        List<FareAttributeDto> FilterFares(FareFilter filter);

        /// <summary>
        ///     Calls <see cref="RouteRepository"/> filtering method.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="Route"/> objects matching the filtering query.
        /// </returns>
        [OperationContract]
        List<RouteDto> FilterRoutes(RouteFilter filter);

        /// <summary>
        ///     Calls <see cref="ZoneRepository"/> filtering method.
        /// </summary>
        /// <param name="name">Filtering parameter.</param>
        /// <returns>
        ///     List of <see cref="Zone"/> objects matching the filtering query.
        /// </returns>
        [OperationContract]
        List<ZoneDto> FilterZones(string name);
    }
}
