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
    public class FareAttributeService
    {
        /// <summary>
        ///     Inserts an <see cref="FareAttribute" /> record into the database.
        /// </summary>
        /// <param name="fareAttribute"><see cref="FareAttribute" /> object to insert into the database.</param>
        /// <returns>The <see cref="FareAttribute" /> object corresponding to the inserted record.</returns>
        public FareAttribute Create(FareAttribute fareAttribute)
        {
            using (var db = new PublicTransportContext())
            {
                db.FareAttributes.Add(fareAttribute);
                db.SaveChanges();
                return fareAttribute;
            }
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
            using (var db = new PublicTransportContext())
            {
                return db.FareAttributes.FirstOrDefault(u => u.Id == id);
            }
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
            using (var db = new PublicTransportContext())
            {
                var old = Read(fareAttribute.Id);
                if (old == null)
                {
                    throw new EntryNotFoundException();
                }

                db.Entry(old).CurrentValues.SetValues(fareAttribute);
                db.SaveChanges();
                return fareAttribute;
            }
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
            using (var db = new PublicTransportContext())
            {
                var old = Read(fareAttribute.Id);
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