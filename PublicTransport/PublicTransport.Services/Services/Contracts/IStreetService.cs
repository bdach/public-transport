using System;
using System.Collections.Generic;
using System.ServiceModel;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Services.Contracts
{
    [ServiceContract]
    public interface IStreetService : IDisposable
    {
        /// <summary>
        ///     Creates a street.
        /// </summary>
        /// <param name="street"><see cref="StreetDto"/> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="StreetDto"/> object successfully inserted into the database.
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        StreetDto CreateStreet(StreetDto street);

        /// <summary>
        ///     Updates a street.
        /// </summary>
        /// <param name="street"><see cref="StreetDto"/> object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="StreetDto"/> object successfully updated in the database.
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        StreetDto UpdateStreet(StreetDto street);

        /// <summary>
        ///     Deletes a street.
        /// </summary>
        /// <param name="street"><see cref="StreetDto"/> object to be deleted from the database.</param>
        [OperationContract]
        void DeleteStreet(StreetDto street);

        /// <summary>
        ///     Filters cities by name.
        /// </summary>
        /// <param name="name">Filtering parameter.</param>
        /// <returns>
        ///     List of <see cref="CityDto"/> objects matching the filtering query.
        /// </returns>
        [OperationContract]
        List<CityDto> FilterCities(string name);

        /// <summary>
        ///     Filters streets by name.
        /// </summary>
        /// <param name="filter"><see cref="StreetFilter"/> object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="StreetDto"/> objects matching the filtering query.
        /// </returns>
        [OperationContract]
        List<StreetDto> FilterStreets(StreetFilter filter);
    }
}