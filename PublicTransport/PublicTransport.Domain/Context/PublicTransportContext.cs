using System.Data.Entity;
using PublicTransport.Domain.Entities;

namespace PublicTransport.Domain.Context
{
    /// <summary>
    ///     Represents the context of the database.
    /// </summary>
    public class PublicTransportContext : DbContext
    {
        /// <summary>
        ///     Constructor with explicit database name.
        /// </summary>
        public PublicTransportContext() : this("PublicTransport") { }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="nameOrConnectionString">Name of the database or connection string.</param>
        public PublicTransportContext(string nameOrConnectionString) : base(nameOrConnectionString) { }

        /// <summary>
        ///     Returns a DbSet of <see cref="Agency" /> records contained in the databse.
        /// </summary>
        public DbSet<Agency> Agencies { get; set; }

        /// <summary>
        ///     Returns a DbSet of <see cref="Calendar" /> records contained in the databse.
        /// </summary>
        public DbSet<Calendar> Calendars { get; set; }

        /// <summary>
        ///     Returns a DbSet of <see cref="CalendarDate" /> records contained in the databse.
        /// </summary>
        public DbSet<CalendarDate> CalendarDates { get; set; }

        /// <summary>
        ///     Returns a DbSet of <see cref="City" /> records contained in the databse.
        /// </summary>
        public DbSet<City> Cities { get; set; }

        /// <summary>
        ///     Returns a DbSet of <see cref="FareAttribute" /> records contained in the databse.
        /// </summary>
        public DbSet<FareAttribute> FareAttributes { get; set; }

        /// <summary>
        ///     Returns a DbSet of <see cref="FareRule" /> records contained in the databse.
        /// </summary>
        public DbSet<FareRule> FareRules { get; set; }

        /// <summary>
        ///     Returns a DbSet of <see cref="Route" /> records contained in the databse.
        /// </summary>
        public DbSet<Route> Routes { get; set; }

        /// <summary>
        ///     Returns a DbSet of <see cref="Stop" /> records contained in the databse.
        /// </summary>
        public DbSet<Stop> Stops { get; set; }

        /// <summary>
        ///     Returns a DbSet of <see cref="StopTime" /> records contained in the databse.
        /// </summary>
        public DbSet<StopTime> StopTimes { get; set; }

        /// <summary>
        ///     Returns a DbSet of <see cref="Street" /> records contained in the databse.
        /// </summary>
        public DbSet<Street> Streets { get; set; }

        /// <summary>
        ///     Returns a DbSet of <see cref="Trip" /> records contained in the databse.
        /// </summary>
        public DbSet<Trip> Trips { get; set; }

        /// <summary>
        ///     Returns a DbSet of <see cref="Zone" /> records contained in the databse.
        /// </summary>
        public DbSet<Zone> Zones { get; set; }
    }
}
