using System.Data.Entity;
using NUnit.Framework;
using PublicTransport.Domain.Context;

namespace PublicTransport.Tests.Services
{
    /// <summary>
    ///     Base test class for all tests that should hit the database.
    /// </summary>
    public abstract class RepositoryTest
    {
        /// <summary>
        ///     Transaction to run the test in.
        /// </summary>
        private DbContextTransaction _transaction;

        /// <summary>
        ///     DB context to use during test.
        /// </summary>
        protected PublicTransportContext DbContext;

        [SetUp]
        public void SetUp()
        {
            DbContext = new PublicTransportContext();
            _transaction = DbContext.Database.BeginTransaction();
        }

        [TearDown]
        public void TearDown()
        {
            _transaction.Rollback();
            _transaction.Dispose();
            DbContext.Dispose();
        }
    }
}