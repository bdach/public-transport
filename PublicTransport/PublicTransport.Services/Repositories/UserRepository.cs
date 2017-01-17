using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Services.Repositories
{
    /// <summary>
    ///     Service for managing users.
    /// </summary>
    public class UserRepository
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
        ///     Constructor.
        /// </summary>
        /// <param name="db"><see cref="PublicTransportContext" /> to use during service operations.</param>
        public UserRepository(PublicTransportContext db)
        {
            _db = db;
            _passwordService = new PasswordService();
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="db"><see cref="PublicTransportContext" /> to use during service operations.</param>
        /// <param name="passwordService">Password service to use for generating and comparing hashes.</param>
        public UserRepository(PublicTransportContext db, IPasswordService passwordService)
        {
            _db = db;
            _passwordService = passwordService;
        }

        /// <summary>
        ///     Inserts a <see cref="User" /> record into the database.
        /// </summary>
        /// <param name="user"><see cref="User" /> object to insert into the database.</param>
        /// <returns>The <see cref="User" /> object corresponding to the inserted record.</returns>
        public User Create(User user)
        {
            var existingUser = _db.Users.FirstOrDefault(u => u.UserName == user.UserName);
            if (existingUser != null)
            {
                throw new UserAlreadyExistsException();
            }

            user.Password = _passwordService.GenerateHash(user.Password);
            var roles = new List<Role>();
            foreach (var role in user.Roles)
            {
                var currentRole = _db.Roles.FirstOrDefault(r => r.Id == role.Id && r.Name == role.Name);
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
        ///     Thrown when the supplied <see cref="User" /> could not be found in the database.
        /// </exception>
        public User Update(User user)
        {
            user.Password = _passwordService.GenerateHash(user.Password);
            var old = _db.Users.Find(user.Id);
            if (old == null)
            {
                throw new EntryNotFoundException();
            }

            var roles = new List<Role>();
            foreach (var role in user.Roles)
            {
                var currentRole = _db.Roles.FirstOrDefault(r => r.Id == role.Id && r.Name == role.Name);
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
        ///     Updates all of the fields of the supplied <see cref="User" /> skipping password hashing and roles.
        /// </summary>
        /// <param name="user"><see cref="User" /> object to update.</param>
        /// <returns>Updated <see cref="User" /> object.</returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="User" /> could not be found in the database.
        /// </exception>
        public User SimpleUpdate(User user)
        {
            var old = _db.Users.Find(user.Id);
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
        ///     Thrown when the supplied <see cref="User" /> could not be found in the database.
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
        ///     <see cref="UserFilter" /> object. The returned name strings all contain the
        ///     parameters supplied in the <see cref="filter" /> parameter.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>List of items satisfying the supplied query.</returns>
        public List<User> FilterUsers(UserFilter filter)
        {
            var users = _db.Users.Include(u => u.Roles)
                .Where(u => u.UserName.Contains(filter.UserNameFilter))
                .Where(u => !filter.RoleTypeFilter.HasValue || u.Roles.Any(r => r.Name == filter.RoleTypeFilter.Value))
                .Take(20)
                .ToList();
            users.ForEach(u => u.Password = null);
            return users;
        }

        /// <summary>
        ///     Retrieves information about <see cref="Stop" />s favourited by the user.
        /// </summary>
        /// <param name="userName">Username (login) of the user that exists in the database.</param>
        /// <returns>List of favourited stops.</returns>
        public List<Stop> GetFavouriteStopsByUserName(string userName)
        {
            return _db.Users.Include(u => u.FavouriteStops.Select(s => s.Street.City))
                .First(u => u.UserName == userName)
                .FavouriteStops.ToList();
        }

        /// <summary>
        ///     Retrieves information about <see cref="Route" />s favourited by the user.
        /// </summary>
        /// <param name="userName">Username (login) of the user that exists in the database.</param>
        /// <returns>List of favourited routes.</returns>
        public List<Route> GetFavouriteRoutesByUserName(string userName)
        {
            return _db.Users.Include(u => u.FavouriteRoutes.Select(s => s.Agency.Street.City))
                .First(u => u.UserName == userName)
                .FavouriteRoutes.ToList();
        }

        /// <summary>
        ///     Updates a user's list of favourite <see cref="Stop" />s.
        /// </summary>
        /// <param name="changes">
        ///     A dictionary describing changes made by the user. The keys are stop IDs, and the values indicate whether the stop
        ///     was added (true) or removed (false)
        /// </param>
        /// <param name="userName">The username of the user whose favourites are to be updated.</param>
        /// <returns>Updated list of favorite stops.</returns>
        public List<Stop> UpdateFavouriteStops(Dictionary<int, bool> changes, string userName)
        {
            var user = _db.Users.Include(u => u.FavouriteStops.Select(s => s.Street.City))
                .FirstOrDefault(u => u.UserName == userName);
            if (user == null)
            {
                throw new EntryNotFoundException();
            }
            var addedStops = changes.Where(kv => kv.Value)
                .Select(kv => _db.Stops.Include(s => s.Street.City).First(s => s.Id == kv.Key))
                .ToList();
            var removedStops = user.FavouriteStops
                .Where(s => changes.ContainsKey(s.Id));
            var result = user.FavouriteStops
                .Except(removedStops)
                .Concat(addedStops)
                .Distinct()
                .ToList();
            user.FavouriteStops = result;
            _db.SaveChanges();
            return result;
        }

        /// <summary>
        ///     Updates a user's list of favourite <see cref="Route" />s.
        /// </summary>
        /// <param name="changes">
        ///     A dictionary describing changes made by the user. The keys are route IDs, and the values indicate whether the route
        ///     was added (true) or removed (false)
        /// </param>
        /// <param name="userName">The username of the user whose favourites are to be updated.</param>
        /// <returns>Updated list of favorite routes.</returns>
        public List<Route> UpdateFavouriteRoutes(Dictionary<int, bool> changes, string userName)
        {
            var user = _db.Users.Include(u => u.FavouriteRoutes.Select(r => r.Agency.Street.City))
                .FirstOrDefault(u => u.UserName == userName);
            if (user == null)
            {
                throw new EntryNotFoundException();
            }
            var addedRoutes = changes.Where(kv => kv.Value)
                .Select(kv => _db.Routes.Include(r => r.Agency.Street.City).First(s => s.Id == kv.Key))
                .ToList();
            var removedRoutes = user.FavouriteRoutes
                .Where(s => changes.ContainsKey(s.Id));
            var result = user.FavouriteRoutes
                .Except(removedRoutes)
                .Concat(addedRoutes)
                .Distinct()
                .ToList();
            user.FavouriteRoutes = result;
            _db.SaveChanges();
            return result;
        }
    }
}