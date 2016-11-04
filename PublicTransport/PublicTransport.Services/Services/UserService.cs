using System;
using System.Data.Entity;
using System.Linq;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Services
{
    /// <summary>
    ///     Service for managing users.
    /// </summary>
    public class UserService : IDisposable
    {
        private readonly PublicTransportContext _db = new PublicTransportContext();
        private bool _disposed;

        /// <summary>
        ///     Inserts a <see cref="User" /> record into the database.
        /// </summary>
        /// <param name="user"><see cref="User" /> object to insert into the database.</param>
        /// <returns>The <see cref="User" /> object corresponding to the inserted record.</returns>
        public User Create(User user)
        {
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
            return _db.Users.FirstOrDefault(u => u.Id == id);
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
            var old = Read(user.Id);
            if (old == null)
            {
                throw new EntryNotFoundException();
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
        ///     Disposed database context.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;
            _db.Dispose();
            _disposed = true;
        }
    }
}