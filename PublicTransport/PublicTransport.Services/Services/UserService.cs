using System;
using System.Collections.Generic;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Repositories;

namespace PublicTransport.Services
{
    public interface IUserService : IDisposable
    {
        /// <summary>
        ///     Calls <see cref="UserRepository"/> create method.
        /// </summary>
        /// <param name="user"><see cref="User"/> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="User"/> object successfully inserted into the database.
        /// </returns>
        User CreateUser(User user);

        /// <summary>
        ///     Calls <see cref="UserRepository"/> update method.
        /// </summary>
        /// <param name="user"><see cref="User"/> object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="User"/> object successfully updated in the database.
        /// </returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="User" /> could not be found in the database.
        /// </exception>
        User UpdateUser(User user);

        /// <summary>
        ///     Calls <see cref="UserRepository"/> delete method.
        /// </summary>
        /// <param name="user"><see cref="User"/> object to be deleted from the database.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="User" /> could not be found in the database.
        /// </exception>
        void DeleteUser(User user);

        /// <summary>
        ///     Calls <see cref="UserRepository"/> filtering method.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="User"/> objects matching the filtering query.
        /// </returns>
        List<User> FilterUsers(IUserFilter filter);

        /// <summary>
        ///     Returns a list of all roles from the database.
        /// </summary>
        /// <returns>
        ///     List of all roles from the database.
        /// </returns>
        List<Role> GetAllRoles();
    }

    /// <summary>
    ///     Unit of work used to manage user data.
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
        ///     Database context common for services in this unit of work used to access data.
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
        }

        /// <summary>
        ///     Calls <see cref="UserRepository"/> create method.
        /// </summary>
        /// <param name="user"><see cref="User"/> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="User"/> object successfully inserted into the database.
        /// </returns>
        public User CreateUser(User user)
        {
            return _userRepository.Create(user);
        }

        /// <summary>
        ///     Calls <see cref="UserRepository"/> update method.
        /// </summary>
        /// <param name="user"><see cref="User"/> object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="User"/> object successfully updated in the database.
        /// </returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="User" /> could not be found in the database.
        /// </exception>
        public User UpdateUser(User user)
        {
            return _userRepository.Update(user);
        }

        /// <summary>
        ///     Calls <see cref="UserRepository"/> delete method.
        /// </summary>
        /// <param name="user"><see cref="User"/> object to be deleted from the database.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="User" /> could not be found in the database.
        /// </exception>
        public void DeleteUser(User user)
        {
            _userRepository.Delete(user);
        }

        /// <summary>
        ///     Calls <see cref="UserRepository"/> filtering method.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="User"/> objects matching the filtering query.
        /// </returns>
        public List<User> FilterUsers(IUserFilter filter)
        {
            return _userRepository.FilterUsers(filter);
        }

        /// <summary>
        ///     Returns a list of all roles from the database.
        /// </summary>
        /// <returns>
        ///     List of all roles from the database.
        /// </returns>
        public List<Role> GetAllRoles()
        {
            return _roleRepository.GetAllRoles();
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