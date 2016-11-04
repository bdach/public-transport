﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Services
{
    /// <summary>
    ///     Service for managing routes.
    /// </summary>
    public class RouteService : IDisposable
    {
        private readonly PublicTransportContext _db = new PublicTransportContext();
        private bool _disposed;

        /// <summary>
        ///     Inserts a <see cref="Route" /> record into the database.
        /// </summary>
        /// <param name="route"><see cref="Route" /> object to insert into the database.</param>
        /// <returns>The <see cref="Route" /> object corresponding to the inserted record.</returns>
        public Route Create(Route route)
        {
            _db.Routes.Add(route);
            _db.SaveChanges();
            return route;
        }

        /// <summary>
        ///     Returns the <see cref="Route" /> with the supplied <see cref="Route.Id" />.
        /// </summary>
        /// <param name="id">Identification number of the desired <see cref="Route" />.</param>
        /// <returns>
        ///     <see cref="Route" /> object with the supplied ID number, or null if the <see cref="Route" /> with the supplied ID
        ///     could not be found in the database.
        /// </returns>
        public Route Read(int id)
        {
            return _db.Routes.FirstOrDefault(u => u.Id == id);
        }

        /// <summary>
        ///     Updates all of the fields of the supplied <see cref="Route" />.
        /// </summary>
        /// <param name="route"><see cref="Route" /> object to update.</param>
        /// <returns>Updated <see cref="Route" /> object.</returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Route" /> could not be found in the
        ///     database.
        /// </exception>
        public Route Update(Route route)
        {
            var old = Read(route.Id);
            if (old == null)
            {
                throw new EntryNotFoundException();
            }

            _db.Entry(old).CurrentValues.SetValues(route);
            _db.SaveChanges();
            return route;
        }

        /// <summary>
        ///     Deletes the supplied <see cref="Route" /> from the database.
        /// </summary>
        /// <param name="route"><see cref="Route" /> object to delete.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Route" /> could not be found in the
        ///     database.
        /// </exception>
        public void Delete(Route route)
        {
            var old = Read(route.Id);
            if (old == null)
            {
                throw new EntryNotFoundException();
            }

            _db.Entry(old).State = EntityState.Deleted;
            _db.SaveChanges();
        }

        /// <summary>
        ///     Returns a list of <see cref="Route" />s associated with a certain <see cref="Agency" />.
        /// </summary>
        /// <param name="agencyId">Id of the <see cref="Agency" />.</param>
        /// <returns>
        ///     Returns a list of <see cref="Route" />s associated with a certain <see cref="Agency" />.
        /// </returns>
        public List<Route> GetRoutesByAgencyId(int agencyId)
        {
                return _db.Routes.Where(x => x.AgencyId == agencyId).ToList();
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
