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
    ///     Service interface specifying contract for managing user data.
    /// </summary>
    [ServiceContract]
    public interface IUserService : IDisposable
    {
        /// <summary>
        ///     Creates a <see cref="User" /> object in the database.
        /// </summary>
        /// <param name="user"><see cref="UserDto" /> object containing <see cref="User" /> data.</param>
        /// <returns>
        ///     <see cref="UserDto" /> representing the inserted <see cref="User" />.
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        UserDto CreateUser(UserDto user);

        /// <summary>
        ///     Updates a <see cref="User" /> object in the database, using the data stored in the
        ///     <see cref="UserDto" /> object.
        /// </summary>
        /// <param name="user"><see cref="UserDto" /> representing the object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="UserDto" /> object containing the updated <see cref="User" /> data.
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        UserDto UpdateUser(UserDto user);

        /// <summary>
        ///     Deletes a <see cref="User" /> from the system.
        /// </summary>
        /// <param name="user"><see cref="UserDto" /> representing the <see cref="User" /> to be deleted from the database.</param>
        [OperationContract]
        void DeleteUser(UserDto user);

        /// <summary>
        ///     Filters <see cref="User" /> objects using the supplied <see cref="UserFilter" />.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="UserDto" /> objects matching the filtering query.
        /// </returns>
        [OperationContract]
        List<UserDto> FilterUsers(UserFilter filter);

        /// <summary>
        ///     Returns a list of all roles from the database.
        /// </summary>
        /// <returns>
        ///     List of all roles from the database.
        /// </returns>
        [OperationContract]
        List<RoleDto> GetAllRoles();
    }
}
