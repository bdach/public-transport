using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Services
{
    /// <summary>
    ///     Service for managing cities.
    /// </summary>
    public class CityRepository
    {
        /// <summary>
        ///     An instance of database context.
        /// </summary>
        private readonly PublicTransportContext _db;
        
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="db"><see cref="PublicTransportContext" /> to use during service operations.</param>
        public CityRepository(PublicTransportContext db)
        {
            _db = db;
        }

        /// <summary>
        ///     Inserts an <see cref="City" /> record into the database.
        /// </summary>
        /// <param name="city"><see cref="City" /> object to insert into the database.</param>
        /// <returns>The <see cref="City" /> object corresponding to the inserted record.</returns>
        public City Create(City city)
        {
            _db.Cities.Add(city);
            _db.SaveChanges();
            return city;
        }

        /// <summary>
        ///     Returns the <see cref="City" /> with the supplied <see cref="City.Id" />.
        /// </summary>
        /// <param name="id">Identification number of the desired <see cref="City" />.</param>
        /// <returns>
        ///     <see cref="City" /> object with the supplied ID number, or null if the user with the supplied ID could not be found
        ///     in the database.
        /// </returns>
        public City Read(int id)
        {
            return _db.Cities.FirstOrDefault(u => u.Id == id);
        }

        /// <summary>
        ///     Updates all of the fields of the supplied <see cref="City" />.
        /// </summary>
        /// <param name="city"><see cref="City" /> object to update.</param>
        /// <returns>Updated <see cref="City" /> object.</returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="City" /> could not be found in the database.
        /// </exception>
        public City Update(City city)
        {
            var old = Read(city.Id);
            if (old == null)
            {
                throw new EntryNotFoundException();
            }

            _db.Entry(old).CurrentValues.SetValues(city);
            _db.SaveChanges();
            return city;
        }

        /// <summary>
        ///     Deletes the supplied <see cref="City" /> from the database.
        /// </summary>
        /// <param name="city"><see cref="City" /> object to delete.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="City" /> could not be found in the database.
        /// </exception>
        public void Delete(City city)
        {
            var old = Read(city.Id);
            if (old == null)
            {
                throw new EntryNotFoundException();
            }

            _db.Entry(old).State = EntityState.Deleted;
            _db.SaveChanges();
        }

        /// <summary>
        ///     Return a list of <see cref="City" /> whose names contain provided string.
        /// </summary>
        /// <param name="str">String which has to be present in the name.</param>
        /// <returns>
        ///     Return a list of <see cref="City" /> whose names contain provided string.
        /// </returns>
        public List<City> GetCitiesContainingString(string str)
        {
            return _db.Cities.Where(c => c.Name.Contains(str)).Take(10).ToList();
        }
    }
}