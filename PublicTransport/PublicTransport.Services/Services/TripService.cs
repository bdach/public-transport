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
    ///     Service for managing trips.
    /// </summary>
    public class TripService : IDisposable
    {
        private readonly PublicTransportContext _db = new PublicTransportContext();
        private bool _disposed;

        /// <summary>
        ///     Disposed database context.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            _db.Dispose();
            _disposed = true;
        }

        /// <summary>
        ///     Inserts a <see cref="Trip" /> record into the database.
        /// </summary>
        /// <param name="trip"><see cref="Trip" /> object to insert into the database.</param>
        /// <returns>The <see cref="Trip" /> object corresponding to the inserted record.</returns>
        public Trip Create(Trip trip)
        {
            _db.Trips.Add(trip);
            _db.SaveChanges();
            return trip;
        }

        /// <summary>
        ///     Returns the <see cref="Trip" /> with the supplied <see cref="Trip.Id" />.
        /// </summary>
        /// <param name="id">Identification number of the desired <see cref="Trip" />.</param>
        /// <returns>
        ///     <see cref="Trip" /> object with the supplied ID number, or null if the <see cref="Trip" /> with the supplied ID
        ///     could not be found in the database.
        /// </returns>
        public Trip Read(int id)
        {
            return _db.Trips.FirstOrDefault(u => u.Id == id);
        }

        /// <summary>
        ///     Updates all of the fields of the supplied <see cref="Trip" />.
        /// </summary>
        /// <param name="trip"><see cref="Trip" /> object to update.</param>
        /// <returns>Updated <see cref="Trip" /> object.</returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Trip" /> could not be found in the
        ///     database.
        /// </exception>
        public Trip Update(Trip trip)
        {
            var old = Read(trip.Id);
            if (old == null)
            {
                throw new EntryNotFoundException();
            }

            _db.Entry(old).CurrentValues.SetValues(trip);
            _db.SaveChanges();
            return trip;
        }

        /// <summary>
        ///     Deletes the supplied <see cref="Trip" /> from the database.
        /// </summary>
        /// <param name="trip"><see cref="Trip" /> object to delete.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Trip" /> could not be found in the
        ///     database.
        /// </exception>
        public void Delete(Trip trip)
        {
            var old = Read(trip.Id);
            if (old == null)
            {
                throw new EntryNotFoundException();
            }

            _db.Entry(old).State = EntityState.Deleted;
            _db.SaveChanges();
        }

        /// <summary>
        ///     Updates the <see cref="StopTime" /> associated with this trip.
        /// </summary>
        /// <param name="tripId">ID number of trip to update</param>
        /// <param name="stops">The stops which should be saved.</param>
        /// <returns>List of stops after saving.</returns>
        public List<StopTime> UpdateStops(int tripId, List<StopTime> stops)
        {
            var currentStops = _db.StopTimes.Where(st => st.TripId == tripId).ToList();
            var newStops =
                stops.Where(st => _db.StopTimes.FirstOrDefault(existing => existing.Id == st.Id) == null)
                    .ToList();
            newStops.ForEach(st => st.TripId = tripId);
            var updatedStops = stops.Except(newStops).ToList();
            var deletedStops = currentStops.Except(updatedStops);

            _db.StopTimes.AddRange(newStops);
            _db.StopTimes.RemoveRange(deletedStops);
            foreach (var updatedStop in updatedStops)
            {
                var toUpdate = _db.StopTimes.Find(updatedStop.Id);
                _db.Entry(toUpdate).CurrentValues.SetValues(updatedStop);
            }
            _db.SaveChanges();
            return stops;
        }
    }
}