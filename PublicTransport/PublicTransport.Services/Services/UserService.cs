﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Services
{
    /// <summary>
    ///     Service for managing users.
    /// </summary>
    public class UserService : IDisposable
    {
        /// <summary>
        ///     An instance of database context.
        /// </summary>
        private readonly PublicTransportContext _db;

        /// <summary>
        ///     Service providing password hashing capabilities.
        /// </summary>
        private readonly IPasswordService _passwordService;

        /// <summary>
        ///     Determines whether the database context has already been disposed.
        /// </summary>
        private bool _disposed;

        /// <summary>
        ///     Default constructor.
        /// </summary>
        public UserService()
        {
            _db = new PublicTransportContext();
            _passwordService = new PasswordService();
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="db">Database context to inject.</param>
        /// <param name="passwordService">Password service to use for generating and comparing hashes.</param>
        public UserService(PublicTransportContext db, IPasswordService passwordService)
        {
            _db = db;
            _passwordService = passwordService;
        }

        /// <summary>
        ///     Disposes database context if not disposed already.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            _db.Dispose();
            _disposed = true;
        }

        /// <summary>
        ///     Inserts a <see cref="User" /> record into the database.
        /// </summary>
        /// <param name="user"><see cref="User" /> object to insert into the database.</param>
        /// <returns>The <see cref="User" /> object corresponding to the inserted record.</returns>
        public User Create(User user)
        {
            user.Password = _passwordService.GenerateHash(user.Password);
            var roles = new List<Role>();
            foreach (var role in user.Roles)
            {
                var currentRole = _db.Roles.FirstOrDefault(r => (r.Id == role.Id) && (r.Name == role.Name));
                if (currentRole == null)
                {
                    return null;
                }
                roles.Add(currentRole);
            }
            user.Roles = roles;

            _db.Users.Add(user);
            _db.SaveChanges();
            return user;
        }

        /// <summary>
        ///     Returns the <see cref="User" /> with the supplied <see cref="User.Id" />.
        /// </summary>
        /// <param name="id">Identification number of the desired <see cref="User" />.</param>
        /// <returns>
        ///     <see cref="User" /> object with the supplied ID number, or null if the <see cref="User" /> with the supplied ID
        ///     could not be found in the database.
        /// </returns>
        public User Read(int id)
        {
            var user = _db.Users.Include(u => u.Roles).FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                user.Password = null;
            }
            return user;
        }

        /// <summary>
        ///     Updates all of the fields of the supplied <see cref="User" />.
        /// </summary>
        /// <param name="user"><see cref="User" /> object to update.</param>
        /// <returns>Updated <see cref="User" /> object.</returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="User" /> could not be found in the
        ///     database.
        /// </exception>
        public User Update(User user)
        {
            user.Password = _passwordService.GenerateHash(user.Password);
            var old = Read(user.Id);
            if (old == null)
            {
                throw new EntryNotFoundException();
            }

            var roles = new List<Role>();
            foreach (var role in user.Roles)
            {
                var currentRole = _db.Roles.FirstOrDefault(r => (r.Id == role.Id) && (r.Name == role.Name));
                if (currentRole == null)
                {
                    return null;
                }
                roles.Add(currentRole);
            }
            user.Roles = roles;

            var deletedRoles = old.Roles.Except(user.Roles).ToList();
            var addedRoles = user.Roles.Except(old.Roles).ToList();
            deletedRoles.ForEach(r => old.Roles.Remove(r));
            _db.Entry(old).CurrentValues.SetValues(user);

            foreach (var role in addedRoles)
            {
                if (_db.Entry(role).State == EntityState.Detached)
                {
                    _db.Roles.Attach(role);
                }
                old.Roles.Add(role);
            }

            _db.Entry(old).CurrentValues.SetValues(user);
            _db.SaveChanges();
            return user;
        }

        /// <summary>
        ///     Deletes the supplied <see cref="User" /> from the database.
        /// </summary>
        /// <param name="user"><see cref="User" /> object to delete.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="User" /> could not be found in the
        ///     database.
        /// </exception>
        public void Delete(User user)
        {
            var old = Read(user.Id);
            if (old == null)
            {
                throw new EntryNotFoundException();
            }

            _db.Entry(old).State = EntityState.Deleted;
            _db.SaveChanges();
        }

        /// <summary>
        ///     Selects all the <see cref="User" /> objects that match all the criteria specified by the
        ///     <see cref="IUserFilter" /> object. The returned name strings all contain the
        ///     parameters supplied in the <see cref="filter" /> parameter.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>List of items satisfying the supplied query.</returns>
        public List<User> FilterUsers(IUserFilter filter)
        {
            var users = _db.Users.Include(u => u.Roles)
                .Where(u => u.UserName.Contains(filter.UserNameFilter))
                .Where(u => !filter.RoleNameFilter.HasValue || u.Roles.Any(r => r.Name == filter.RoleNameFilter.Value))
                .Take(20)
                .ToList();
            users.ForEach(u => u.Password = null);
            return users;
        }
    }
}