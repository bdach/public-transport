using System;
using System.Collections.Generic;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer.Filters;

namespace PublicTransport.Services.UnitsOfWork
{
    /// <summary>
    ///     Unit of work used to manage user data.
    /// </summary>
    public class UserUnitOfWork : IDisposable
    {
        /// <summary>
        ///     Service used to fetch <see cref="User"/> data from the database.
        /// </summary>
        private readonly UserService _userService;

        /// <summary>
        ///     Service used to fetch <see cref="Role"/> data from the database.
        /// </summary>
        private readonly RoleService _roleService;

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
        public UserUnitOfWork()
        {
            _db = new PublicTransportContext();
            _userService = new UserService(_db);
            _roleService = new RoleService(_db);
        }

        /// <summary>
        ///     Calls <see cref="UserService"/> create method.
        /// </summary>
        /// <param name="user"><see cref="User"/> object to be inserted into the database.</param>
        /// <returns>
        ///     <see cref="User"/> object successfully inserted into the database.
        /// </returns>
        public User CreateUser(User user)
        {
            return _userService.Create(user);
        }

        /// <summary>
        ///     Calls <see cref="UserService"/> delete method.
        /// </summary>
        /// <param name="user"><see cref="User"/> object to be deleted from the database.</param>
        public void DeleteUser(User user)
        {
            _userService.Delete(user);
        }

        /// <summary>
        ///     Calls <see cref="UserService"/> update method.
        /// </summary>
        /// <param name="user"><see cref="User"/> object to be updated in the database.</param>
        /// <returns>
        ///     <see cref="User"/> object successfully updated in the database.
        /// </returns>
        public User UpdateUser(User user)
        {
            return _userService.Update(user);
        }

        /// <summary>
        ///     Calls <see cref="UserService"/> filtering method.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>
        ///     List of <see cref="User"/> objects matching the filtering query.
        /// </returns>
        public List<User> FilterUsers(IUserFilter filter)
        {
            return _userService.FilterUsers(filter);
        }

        /// <summary>
        ///     Returns a list of all roles from the database.
        /// </summary>
        /// <returns>
        ///     List of all roles from the database.
        /// </returns>
        public List<Role> GetAllRoles()
        {
            return _roleService.GetAllRoles();
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