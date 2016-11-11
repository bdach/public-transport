using System.Data.Entity;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using NUnit.Framework;
using PublicTransport.Domain.Context;
using PublicTransport.Tests.Properties;
using Database = System.Data.Entity.Database;

namespace PublicTransport.Tests.Services
{
    /// <summary>
    ///     Main class responsible for setting up the persistence tests.
    /// </summary>
    [SetUpFixture]
    public class PersistenceTests
    {
        /// <summary>
        ///     Sets up the localdb instance for testing purposes.
        /// </summary>
        [OneTimeSetUp]
        public void BeforeTests()
        {
            // Always drop the DB
            Database.SetInitializer(new DropCreateDatabaseAlways<PublicTransportContext>());
            // Fetch script
            var scriptContents = Resources.TestingDataScript;
            using (var db = new PublicTransportContext())
            {
                // Actually dropping the DB happens here
                db.Database.Initialize(true);
                // Get a connection and execute the script
                var connection = (SqlConnection) db.Database.Connection;
                connection.Open();
                var serverConnection = new ServerConnection(connection);
                var server = new Server(serverConnection);
                server.ConnectionContext.ExecuteNonQuery(scriptContents);
                connection.Close();
            }
        }

        [OneTimeTearDown]
        public void AfterTests()
        {
            using (var db = new PublicTransportContext())
            {
                db.Database.Delete();
            }
        }
    }
}