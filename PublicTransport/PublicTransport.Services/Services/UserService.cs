using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.Contracts;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.DataTransfer.Converters;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Exceptions;
using PublicTransport.Services.Repositories;

namespace PublicTransport.Services
{
    /// <summary>
    ///     Service used to manage user data.
    /// </summary>
    public class UserService : IUserService
    {
        /// <summary>
        ///     Service used to fetch <see cref="User"/> data from the database.
        /// </summary>
        private readonly UserRepository _userRepository;

        /// <summary>
        ///     Service used to fetch <see cref="Role"/> data from the database.
        /// </summary>
        private readonly RoleRepository _roleRepository;

        /// <summary>
        ///     Used for converting <see cref="Domain.Entities.User" /> objects to <see cref="UserDto" /> objects and back.
        /// </summary>
        private readonly IConverter<User, UserDto> _userConverter;

        /// <summary>
        ///     Used for converting <see cref="Domain.Entities.Role" /> objects to <see cref="RoleDto" /> objects and back.
        /// </summary>
        private readonly IConverter<Role, RoleDto> _roleConverter;

        /// <summary>
        ///     Database context common for services in this service used to access data.
        /// </summary>
        private readonly PublicTransportContext _db;

        /// <summary>
        ///     Determines whether the database context has been disposed.
        /// </summary>
        private bool _disposed;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public UserService()
        {
            _db = new PublicTransportContext();
            _userRepository = new UserRepository(_db);
            _roleRepository = new RoleRepository(_db);
            _userConverter = new UserConverter();
            _roleConverter = new RoleConverter();
        }

        /// <summary>
        ///     Calls <see cref="UserRepository"/> create method.
        /// </summary>
        /// <param name="user"><see cref="User"/> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="User"/> object successfully inserted into the database.
        /// </returns>
        /// <exception cref="ValidationFaultException">
        ///     Thrown when the data contained in the received DTO contains validation errors.
        /// </exception>
        public UserDto CreateUser(UserDto user)
        {
            try
            {
                var result = _userRepository.Create(_userConverter.GetEntity(user));
                return _userConverter.GetDto(result);
            }
            catch (DbEntityValidationException ex)
            {
                throw new ValidationFaultException(ex);
            }
        }

        /// <summary>
        ///     Updates a <see cref="Domain.Entities.User" /> object in the database, using the data stored in the
        ///     <see cref="UserDto" /> object.
        /// </summary>
        /// <param name="user"><see cref="Domain.Entities.User" /> object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="Domain.Entities.User" /> object successfully updated in the database.
        /// </returns>
        /// <exception cref="ValidationFaultException">
        ///     Thrown when the data contained in the received DTO contains validation errors.
        /// </exception>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Domain.Entities.User" /> could not be found in the database.
        /// </exception>
        public UserDto UpdateUser(UserDto user)
        {
            try
            {
                var result = _userRepository.Update(_userConverter.GetEntity(user));
                return _userConverter.GetDto(result);
            }
            catch (DbEntityValidationException ex)
            {
                throw new ValidationFaultException(ex);
            }
        }

        /// <summary>
        ///     Calls <see cref="UserRepository"/> delete method.
        /// </summary>
        /// <param name="user"><see cref="User"/> object to be deleted from the database.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="User" /> could not be found in the database.
        /// </exception>
        public void DeleteUser(UserDto user)
        {
            _userRepository.Delete(_userConverter.GetEntity(user));
        }

        /// <summary>
        ///     Calls <see cref="UserRepository"/> filtering method.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="User"/> objects matching the filtering query.
        /// </returns>
        public List<UserDto> FilterUsers(UserFilter filter)
        {
            return _userRepository.FilterUsers(filter)
                .Select(_userConverter.GetDto)
                .ToList();
        }

        /// <summary>
        ///     Returns a list of all roles from the database.
        /// </summary>
        /// <returns>
        ///     List of all roles from the database.
        /// </returns>
        public List<RoleDto> GetAllRoles()
        {
            return _roleRepository.GetAllRoles()
                .Select(_roleConverter.GetDto)
                .ToList();
        }

        /// <summary>
        ///     Disposes the database context if not disposed already.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;
            _db.Dispose();
            _disposed = true;
        }
    }
}