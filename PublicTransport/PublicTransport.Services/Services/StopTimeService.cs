using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Services
{
    /// <summary>
    ///     Service for managing stop times.
    /// </summary>
    public class StopTimeService : IDisposable
    {
        private readonly PublicTransportContext _db = new PublicTransportContext();
        private bool _disposed;

        /// <summary>
        ///     Inserts a <see cref="StopTime" /> record into the database.
        /// </summary>
        /// <param name="stopTime"><see cref="StopTime" /> object to insert into the database.</param>
        /// <returns>The <see cref="StopTime" /> object corresponding to the inserted record.</returns>
        public StopTime Create(StopTime stopTime)
        {
            _db.StopTimes.Add(stopTime);
            _db.SaveChanges();
            return stopTime;
        }

        /// <summary>
        ///     Returns the <see cref="StopTime" /> with the supplied <see cref="StopTime.Id" />.
        /// </summary>
        /// <param name="id">Identification number of the desired <see cref="StopTime" />.</param>
        /// <returns>
        ///     <see cref="StopTime" /> object with the supplied ID number, or null if the <see cref="StopTime" /> with the supplied ID
        ///     could not be found in the database.
        /// </returns>
        public StopTime Read(int id)
        {
            return _db.StopTimes.FirstOrDefault(u => u.Id == id);
        }

        /// <summary>
        ///     Updates all of the fields of the supplied <see cref="StopTime" />.
        /// </summary>
        /// <param name="stopTime"><see cref="StopTime" /> object to update.</param>
        /// <returns>Updated <see cref="StopTime" /> object.</returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="StopTime" /> could not be found in the
        ///     database.
        /// </exception>
        public StopTime Update(StopTime stopTime)
        {
            var old = Read(stopTime.Id);
            if (old == null)
            {
                throw new EntryNotFoundException();
            }

            _db.Entry(old).CurrentValues.SetValues(stopTime);
            _db.SaveChanges();
            return stopTime;
        }

        /// <summary>
        ///     Deletes the supplied <see cref="StopTime" /> from the database.
        /// </summary>
        /// <param name="stopTime"><see cref="StopTime" /> object to delete.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="StopTime" /> could not be found in the
        ///     database.
        /// </exception>
        public void Delete(StopTime stopTime)
        {
            var old = Read(stopTime.Id);
            if (old == null)
            {
                throw new EntryNotFoundException();
            }

            _db.Entry(old).State = EntityState.Deleted;
            _db.SaveChanges();
        }

        /// <summary>
        ///     Returns a list of <see cref="StopTime" />s for a certain <see cref="Stop"/>.
        /// </summary>
        /// <param name="stopId">Id of the <see cref="Stop" />.</param>
        /// <returns>
        ///     Returns a list of <see cref="StopTime" />s for a certain <see cref="Stop"/>.
        /// </returns>
        public List<StopTime> GetFullTimetableByStopId(int stopId)
        {
            return _db.StopTimes.Where(x => x.StopId == stopId).ToList();
        }

        /// <summary>
        ///     Returns a list of <see cref="StopTime" />s for a certain <see cref="Stop"/> which are associated with a specific <see cref="Route"/>.
        /// </summary>
        /// <param name="stopId">Id of the <see cref="Stop" />.</param>
        /// <param name="routeId">Id of the <see cref="Route" />.</param>
        /// <returns>
        ///     Returns a list of <see cref="StopTime" />s for a certain <see cref="Stop"/> which are associated with a specific <see cref="Route"/>.
        /// </returns>
        public List<StopTime> GetRouteTimetableByStopId(int stopId, int routeId)
        {
            return _db.StopTimes.Where(x => x.StopId == stopId && x.Trip.RouteId == routeId).ToList();
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
