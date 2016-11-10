using System;
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
    ///     Service for managing fare attributes.
    /// </summary>
    public class FareAttributeService : IDisposable
    {
        /// <summary>
        ///     An instance of database context.
        /// </summary>
        private readonly PublicTransportContext _db = new PublicTransportContext();

        /// <summary>
        ///     Determines whether the database context has already been disposed.
        /// </summary>
        private bool _disposed;

        /// <summary>
        ///     Inserts an <see cref="FareAttribute" /> record into the database.
        /// </summary>
        /// <param name="fareAttribute"><see cref="FareAttribute" /> object to insert into the database.</param>
        /// <returns>The <see cref="FareAttribute" /> object corresponding to the inserted record.</returns>
        public FareAttribute Create(FareAttribute fareAttribute)
        {
            _db.FareAttributes.Add(fareAttribute);
            _db.SaveChanges();
            return fareAttribute;
        }

        /// <summary>
        ///     Returns the <see cref="FareAttribute" /> with the supplied <see cref="FareAttribute.Id" />.
        /// </summary>
        /// <param name="id">Identification number of the desired <see cref="FareAttribute" />.</param>
        /// <returns>
        ///     <see cref="FareAttribute" /> object with the supplied ID number, or null if the user with the supplied ID could not
        ///     be found in the database.
        /// </returns>
        public FareAttribute Read(int id)
        {
            return _db.FareAttributes.FirstOrDefault(u => u.Id == id);
        }

        /// <summary>
        ///     Updates all of the fields of the supplied <see cref="FareAttribute" />.
        /// </summary>
        /// <param name="fareAttribute"><see cref="FareAttribute" /> object to update.</param>
        /// <returns>Updated <see cref="FareAttribute" /> object.</returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="FareAttribute" /> could not be found in the database.
        /// </exception>
        public FareAttribute Update(FareAttribute fareAttribute)
        {
            var old = Read(fareAttribute.Id);
            if (old == null)
            {
                throw new EntryNotFoundException();
            }

            _db.Entry(old).CurrentValues.SetValues(fareAttribute);
            _db.SaveChanges();
            return fareAttribute;
        }

        /// <summary>
        ///     Deletes the supplied <see cref="FareAttribute" /> from the database.
        /// </summary>
        /// <param name="fareAttribute"><see cref="FareAttribute" /> object to delete.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="FareAttribute" /> could not be found in the database.
        /// </exception>
        public void Delete(FareAttribute fareAttribute)
        {
            var old = Read(fareAttribute.Id);
            if (old == null)
            {
                throw new EntryNotFoundException();
            }

            _db.Entry(old).State = EntityState.Deleted;
            _db.SaveChanges();
        }

        /// <summary>
        ///     Selects all the <see cref="FareAttribute" /> objects that match all the criteria specified by the
        ///     <see cref="IFareFilter" /> object. The returned name strings all contain the
        ///     parameters supplied in the <see cref="filter" /> parameter.
        /// </summary>
        /// <param name="filter">Object containing the query parameters.</param>
        /// <returns>List of items satisfying the supplied query.</returns>
        public List<FareAttribute> FilterFares(IFareFilter filter)
        {
            return _db.FareAttributes.Include(fa => fa.FareRule.Route.Agency)
                .Include(fa => fa.FareRule.Origin).Include(fa => fa.FareRule.Destination)
                .Where(fa => fa.FareRule.Route.ShortName.Contains(filter.RouteNameFilter))
                .Where(fa => fa.FareRule.Origin.Name.Contains(filter.OriginZoneNameFilter))
                .Where(fa => fa.FareRule.Destination.Name.Contains(filter.DestinationZoneNameFilter))
                .Take(10).ToList();
        }

        /// <summary>
        ///     Disposes database context if not disposed already.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;
            _db.Dispose();
            _disposed = true;
        }
    }
}