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
    ///     Service interface specifying contract for managing city data.
    /// </summary>
    [ServiceContract]
    public interface IFareService : IDisposable
    {
        /// <summary>
        ///     Creates a <see cref="FareAttribute" /> object in the database.
        /// </summary>
        /// <param name="fareAttribute"><see cref="FareAttributeDto" /> object containing <see cref="FareAttribute" /> data.</param>
        /// <returns>
        ///     <see cref="FareAttributeDto" /> representing the inserted <see cref="FareAttribute" />.
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        FareAttributeDto CreateFareAttribute(FareAttributeDto fareAttribute);

        /// <summary>
        ///     Updates a <see cref="FareAttribute" /> object in the database, using the data stored in the
        ///     <see cref="FareAttributeDto" /> object.
        /// </summary>
        /// <param name="fareAttribute"><see cref="FareAttributeDto" /> representing the object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="FareAttributeDto" /> object containing the updated <see cref="FareAttribute" /> data.
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        FareAttributeDto UpdateFareAttribute(FareAttributeDto fareAttribute);

        /// <summary>
        ///     Deletes a <see cref="FareAttribute" /> from the system.
        /// </summary>
        /// <param name="fareAttribute">
        ///     <see cref="FareAttributeDto" /> representing the <see cref="FareAttribute" /> to be deleted
        ///     from the database.
        /// </param>
        [OperationContract]
        void DeleteFareAttribute(FareAttributeDto fareAttribute);

        /// <summary>
        ///     Creates a <see cref="FareRule" /> object in the database.
        /// </summary>
        /// <param name="fareRule"><see cref="FareRuleDto" /> object containing <see cref="FareRule" /> data.</param>
        /// <returns>
        ///     <see cref="FareRuleDto" /> representing the inserted <see cref="FareRule" />.
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        FareRuleDto CreateFareRule(FareRuleDto fareRule);

        /// <summary>
        ///     Updates a <see cref="FareRule" /> object in the database, using the data stored in the
        ///     <see cref="FareRuleDto" /> object.
        /// </summary>
        /// <param name="fareRule"><see cref="FareRuleDto" /> representing the object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="FareRuleDto" /> object containing the updated <see cref="FareRule" /> data.
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        FareRuleDto UpdateFareRule(FareRuleDto fareRule);

        /// <summary>
        ///     Deletes a <see cref="FareRule" /> from the system.
        /// </summary>
        /// <param name="fareRule">
        ///     <see cref="FareRuleDto" /> representing the <see cref="FareRule" /> to be deleted from the
        ///     database.
        /// </param>
        [OperationContract]
        void DeleteFareRule(FareRuleDto fareRule);

        /// <summary>
        ///     Filters <see cref="FareAttribute" /> objects using the supplied <see cref="FareFilter" />.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="FareAttributeDto" /> objects matching the filtering query.
        /// </returns>
        [OperationContract]
        List<FareAttributeDto> FilterFares(FareFilter filter);

        /// <summary>
        ///     Filters <see cref="Route" /> objects using the supplied <see cref="RouteFilter" />.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="RouteDto" /> objects matching the filtering query.
        /// </returns>
        [OperationContract]
        List<RouteDto> FilterRoutes(RouteFilter filter);

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
