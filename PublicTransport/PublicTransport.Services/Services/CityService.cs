﻿using System.Data.Entity;
using System.Linq;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Services
{
    /// <summary>
    ///     Service for managing cities.
    /// </summary>
    public class CityService
    {
        /// <summary>
        ///     Inserts an <see cref="City" /> record into the database.
        /// </summary>
        /// <param name="city"><see cref="City" /> object to insert into the database.</param>
        /// <returns>The <see cref="City" /> object corresponding to the inserted record.</returns>
        public City Create(City city)
        {
            using (var db = new PublicTransportContext())
            {
                db.Cities.Add(city);
                db.SaveChanges();
                return city;
            }
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
            using (var db = new PublicTransportContext())
            {
                return db.Cities.FirstOrDefault(u => u.Id == id);
            }
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
            using (var db = new PublicTransportContext())
            {
                var old = Read(city.Id);
                if (old == null)
                {
                    throw new EntryNotFoundException();
                }

                db.Entry(old).CurrentValues.SetValues(city);
                db.SaveChanges();
                return city;
            }
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
            using (var db = new PublicTransportContext())
            {
                var old = Read(city.Id);
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