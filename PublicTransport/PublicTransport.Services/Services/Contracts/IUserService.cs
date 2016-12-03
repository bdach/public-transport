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
    ///     Service interface specifying contract for managing user data.
    /// </summary>
    [ServiceContract]
    public interface IUserService : IDisposable
    {
        /// <summary>
        ///     Calls <see cref="UserRepository"/> create method.
        /// </summary>
        /// <param name="user"><see cref="UserDto"/> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="UserDto"/> object successfully inserted into the database.
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        UserDto CreateUser(UserDto user);

        /// <summary>
        ///     Calls <see cref="UserRepository"/> update method.
        /// </summary>
        /// <param name="user"><see cref="UserDto"/> object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="UserDto"/> object successfully updated in the database.
        /// </returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="UserDto" /> could not be found in the database.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        UserDto UpdateUser(UserDto user);

        /// <summary>
        ///     Calls <see cref="UserRepository"/> delete method.
        /// </summary>
        /// <param name="user"><see cref="UserDto"/> object to be deleted from the database.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="UserDto" /> could not be found in the database.
        /// </exception>
        [OperationContract]
        void DeleteUser(UserDto user);

        /// <summary>
        ///     Calls <see cref="UserRepository"/> filtering method.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="UserDto"/> objects matching the filtering query.
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
