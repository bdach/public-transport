using System;
using System.Collections.Generic;
using System.ServiceModel;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Services.Interfaces
{
    /// <summary>
    ///     Service interface specifying contract for managing city data.
    /// </summary>
    [ServiceContract]
    public interface ICityService : IDisposable
    {
        /// <summary>
        ///     Creates a <see cref="Domain.Entities.City" /> object in the database.
        /// </summary>
        /// <param name="city"><see cref="Domain.Entities.City" /> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="Domain.Entities.City" /> object successfully inserted into the database.
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        CityDto CreateCity(CityDto city);

        /// <summary>
        ///     Updates a <see cref="Domain.Entities.City" /> object in the database, using the data stored in the
        ///     <see cref="CityDto" /> object.
        /// </summary>
        /// <param name="city"><see cref="Domain.Entities.City" /> object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="Domain.Entities.City" /> object successfully updated in the database.
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        CityDto UpdateCity(CityDto city);

        /// <summary>
        ///     Deletes a <see cref="Domain.Entities.City" /> from the system.
        /// </summary>
        /// <param name="city"><see cref="Domain.Entities.City" /> object to be deleted from the database.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Domain.Entities.City" /> could not be found in the database.
        /// </exception>
        [OperationContract]
        void DeleteCity(CityDto city);

        /// <summary>
        ///     Filters the stored cities.
        /// </summary>
        /// <param name="name">Filtering parameter.</param>
        /// <returns>
        ///     List of <see cref="Domain.Entities.City" /> objects matching the filtering query.
        /// </returns>
        [OperationContract]
        List<CityDto> FilterCities(string str);
    }
}