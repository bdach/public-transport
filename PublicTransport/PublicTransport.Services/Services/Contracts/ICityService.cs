using System;
using System.Collections.Generic;
using System.ServiceModel;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Services.Contracts
{
    /// <summary>
    ///     Service interface specifying contract for managing city data.
    /// </summary>
    [ServiceContract]
    public interface ICityService : IDisposable
    {
        /// <summary>
        ///     Creates a city.
        /// </summary>
        /// <param name="city"><see cref="CityDto" /> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="CityDto" /> object successfully inserted into the database.
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        CityDto CreateCity(CityDto city);

        /// <summary>
        ///     Updates a city.
        /// </summary>
        /// <param name="city"><see cref="CityDto" /> object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="CityDto" /> object successfully updated in the database.
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        CityDto UpdateCity(CityDto city);

        /// <summary>
        ///     Deletes a city.
        /// </summary>
        /// <param name="city"><see cref="CityDto" /> object to be deleted from the database.</param>
        [OperationContract]
        void DeleteCity(CityDto city);

        /// <summary>
        ///     Filters the stored cities.
        /// </summary>
        /// <param name="name">Filtering parameter.</param>
        /// <returns>
        ///     List of <see cref="CityDto" /> objects matching the filtering query.
        /// </returns>
        [OperationContract]
        List<CityDto> FilterCities(string name);
    }
}