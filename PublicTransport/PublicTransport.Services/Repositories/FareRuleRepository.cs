using System.Data.Entity;
using System.Linq;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Services
{
    /// <summary>
    ///     Service for managing fare rules.
    /// </summary>
    public class FareRuleRepository
    {
        /// <summary>
        ///     An instance of database context.
        /// </summary>
        private readonly PublicTransportContext _db;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="db"><see cref="PublicTransportContext" /> to use during service operations.</param>
        public FareRuleRepository(PublicTransportContext db)
        {
            _db = db;
        }

        /// <summary>
        ///     Inserts an <see cref="FareRule" /> record into the database.
        /// </summary>
        /// <param name="fareRule"><see cref="FareRule" /> object to insert into the database.</param>
        /// <returns>The <see cref="FareRule" /> object corresponding to the inserted record.</returns>
        public FareRule Create(FareRule fareRule)
        {
            _db.FareRules.Add(fareRule);
            _db.SaveChanges();
            return fareRule;
        }

        /// <summary>
        ///     Returns the <see cref="FareRule" /> with the supplied <see cref="FareRule.Id" />.
        /// </summary>
        /// <param name="id">Identification number of the desired <see cref="FareRule" />.</param>
        /// <returns>
        ///     <see cref="FareRule" /> object with the supplied ID number, or null if the user with the supplied ID could not be
        ///     found
        ///     in the database.
        /// </returns>
        public FareRule Read(int id)
        {
            return _db.FareRules.FirstOrDefault(u => u.Id == id);
        }

        /// <summary>
        ///     Updates all of the fields of the supplied <see cref="FareRule" />.
        /// </summary>
        /// <param name="fareRule"><see cref="FareRule" /> object to update.</param>
        /// <returns>Updated <see cref="FareRule" /> object.</returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="FareRule" /> could not be found in the database.
        /// </exception>
        public FareRule Update(FareRule fareRule)
        {
            var old = Read(fareRule.Id);
            if (old == null)
            {
                throw new EntryNotFoundException();
            }

            _db.Entry(old).CurrentValues.SetValues(fareRule);
            _db.SaveChanges();
            return fareRule;
        }

        /// <summary>
        ///     Deletes the supplied <see cref="FareRule" /> from the database.
        /// </summary>
        /// <param name="fareRule"><see cref="FareRule" /> object to delete.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="FareRule" /> could not be found in the database.
        /// </exception>
        public void Delete(FareRule fareRule)
        {
            var old = Read(fareRule.Id);
            if (old == null)
            {
                throw new EntryNotFoundException();
            }

            _db.Entry(old).State = EntityState.Deleted;
            _db.SaveChanges();
        }
    }
}