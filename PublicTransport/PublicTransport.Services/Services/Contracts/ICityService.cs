using System;
using System.Collections.Generic;
using System.ServiceModel;
using PublicTransport.Domain.Entities;
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
        ///     Creates a <see cref="City"/> object in the database.
        /// </summary>
        /// <param name="city"><see cref="CityDto" /> object containing <see cref="City"/> data.</param>
        /// <returns>
        ///     <see cref="CityDto" /> representing the inserted <see cref="City"/>.
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        CityDto CreateCity(CityDto city);

        /// <summary>
        ///     Updates a <see cref="City"/> object in the database, using the data stored in the
        ///     <see cref="CityDto" /> object.
        /// </summary>
        /// <param name="city"><see cref="CityDto" /> representing the object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="CityDto" /> object containing the updated <see cref="City"/> data.
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        CityDto UpdateCity(CityDto city);

        /// <summary>
        ///     Deletes a <see cref="City"/> from the system.
        /// </summary>
        /// <param name="city"><see cref="CityDto" /> representing the <see cref="City"/> to be deleted from the database.</param>
        [OperationContract]
        void DeleteCity(CityDto city);

        /// <summary>
        ///     Filters <see cref="City" /> objects using the supplied string.
        /// </summary>
        /// <param name="name">String to filter cities by.</param>
        /// <returns>
        ///     List of <see cref="CityDto" /> objects matching the filtering query.
        /// </returns>
        [OperationContract]
        List<CityDto> FilterCities(string name);
    }
}