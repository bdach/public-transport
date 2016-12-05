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
    public interface IStreetService : IDisposable
    {
        /// <summary>
        ///     Creates a <see cref="Street" /> object in the database.
        /// </summary>
        /// <param name="street"><see cref="StreetDto" /> object containing <see cref="Street" /> data.</param>
        /// <returns>
        ///     <see cref="StreetDto" /> representing the inserted <see cref="Street" />.
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        StreetDto CreateStreet(StreetDto street);

        /// <summary>
        ///     Updates a <see cref="Street" /> object in the database, using the data stored in the
        ///     <see cref="StreetDto" /> object.
        /// </summary>
        /// <param name="street"><see cref="StreetDto" /> representing the object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="StreetDto" /> object containing the updated <see cref="Street" /> data.
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        StreetDto UpdateStreet(StreetDto street);

        /// <summary>
        ///     Deletes a <see cref="Street" /> from the system.
        /// </summary>
        /// <param name="street"><see cref="StreetDto" /> representing the <see cref="Street" /> to be deleted from the database.</param>
        [OperationContract]
        void DeleteStreet(StreetDto street);

        /// <summary>
        ///     Filters <see cref="City" /> objects using the supplied string.
        /// </summary>
        /// <param name="name">String to filter cities by.</param>
        /// <returns>
        ///     List of <see cref="CityDto" /> objects matching the filtering query.
        /// </returns>
        [OperationContract]
        List<CityDto> FilterCities(string name);

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