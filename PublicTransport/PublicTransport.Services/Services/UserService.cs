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
    public class UserService
    {
        /// <summary>
        ///     Inserts a <see cref="User" /> record into the database.
        /// </summary>
        /// <param name="user"><see cref="User" /> object to insert into the database.</param>
        /// <returns>The <see cref="User" /> object corresponding to the inserted record.</returns>
        public User Create(User user)
        {
            using (var db = new PublicTransportContext())
            {
                db.Users.Add(user);
                db.SaveChanges();
                return user;
            }
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
            using (var db = new PublicTransportContext())
            {
                return db.Users.FirstOrDefault(u => u.Id == id);
            }
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
            using (var db = new PublicTransportContext())
            {
                var old = Read(user.Id);
                if (old == null)
                {
                    throw new EntryNotFoundException();
                }

                db.Entry(old).CurrentValues.SetValues(user);
                db.SaveChanges();
                return user;
            }
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
            using (var db = new PublicTransportContext())
            {
                var old = Read(user.Id);
                if (old == null)
                {
                    throw new EntryNotFoundException();
                }

                db.Entry(old).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }
    }
}