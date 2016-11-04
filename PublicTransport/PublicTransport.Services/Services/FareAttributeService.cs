using System;
using System.Data.Entity;
using System.Linq;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Services
{
    /// <summary>
    ///     Service for managing fare attributes.
    /// </summary>
    public class FareAttributeService : IDisposable
    {
        private readonly PublicTransportContext _db = new PublicTransportContext();
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