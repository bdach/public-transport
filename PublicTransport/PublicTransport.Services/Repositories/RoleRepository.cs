using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Services.Repositories
{
    /// <summary>
    ///     Service for managing user roles.
    /// </summary>
    public class RoleRepository
    {
        /// <summary>
        ///     An instance of database context.
        /// </summary>
        private readonly PublicTransportContext _db;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="db"><see cref="PublicTransportContext" /> to use during service operations.</param>
        public RoleRepository(PublicTransportContext db)
        {
            _db = db;
        }

        /// <summary>
        ///     Inserts a <see cref="Role" /> record into the database.
        /// </summary>
        /// <param name="role"><see cref="Role" /> object to insert into the database.</param>
        /// <returns>The <see cref="Role" /> object corresponding to the inserted record.</returns>
        public Role Create(Role role)
        {
            _db.Roles.Add(role);
            _db.SaveChanges();
            return role;
        }

        /// <summary>
        ///     Returns the <see cref="Role" /> with the supplied <see cref="Role.Id" />.
        /// </summary>
        /// <param name="id">Identification number of the desired <see cref="Role" />.</param>
        /// <returns>
        ///     <see cref="Role" /> object with the supplied ID number, or null if the <see cref="Role" /> with the supplied ID
        ///     could not be found in the database.
        /// </returns>
        public Role Read(int id)
        {
            return _db.Roles.FirstOrDefault(u => u.Id == id);
        }

        /// <summary>
        ///     Updates all of the fields of the supplied <see cref="Role" />.
        /// </summary>
        /// <param name="role"><see cref="Role" /> object to update.</param>
        /// <returns>Updated <see cref="Role" /> object.</returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Role" /> could not be found in the database.
        /// </exception>
        public Role Update(Role role)
        {
            var old = Read(role.Id);
            if (old == null)
            {
                throw new EntryNotFoundException();
            }

            _db.Entry(old).CurrentValues.SetValues(role);
            _db.SaveChanges();
            return role;
        }

        /// <summary>
        ///     Deletes the supplied <see cref="Role" /> from the database.
        /// </summary>
        /// <param name="role"><see cref="Role" /> object to delete.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Role" /> could not be found in the database.
        /// </exception>
        public void Delete(Role role)
        {
            var old = Read(role.Id);
            if (old == null)
            {
                throw new EntryNotFoundException();
            }

            _db.Entry(old).State = EntityState.Deleted;
            _db.SaveChanges();
        }

        /// <summary>
        ///     Returns a list of all <see cref="Role"/> objects from the database.
        /// </summary>
        /// <returns>
        ///     Returns a list of all <see cref="Role"/> objects from the database.
        /// </returns>
        public List<Role> GetAllRoles()
        {
            return _db.Roles.ToList();
        }
    }
}
