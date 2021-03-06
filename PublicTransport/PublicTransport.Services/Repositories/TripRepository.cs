﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PublicTransport.Domain.Context;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.Exceptions;
using PublicTransport.Services.DataTransfer.Filters;

namespace PublicTransport.Services.Repositories
{
    public interface ITripRepository
    {
        /// <summary>
        ///     Inserts a <see cref="Trip" /> record into the database.
        /// </summary>
        /// <param name="trip"><see cref="Trip" /> object to insert into the database.</param>
        /// <returns>The <see cref="Trip" /> object corresponding to the inserted record.</returns>
        Trip Create(Trip trip);

        /// <summary>
        ///     Returns the <see cref="Trip" /> with the supplied <see cref="Trip.Id" />.
        /// </summary>
        /// <param name="id">Identification number of the desired <see cref="Trip" />.</param>
        /// <returns>
        ///     <see cref="Trip" /> object with the supplied ID number, or null if the <see cref="Trip" /> with the supplied ID
        ///     could not be found in the database.
        /// </returns>
        Trip Read(int id);

        /// <summary>
        ///     Updates all of the fields of the supplied <see cref="Trip" />.
        /// </summary>
        /// <param name="trip"><see cref="Trip" /> object to update.</param>
        /// <returns>Updated <see cref="Trip" /> object.</returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Trip" /> could not be found in the database.
        /// </exception>
        Trip Update(Trip trip);

        /// <summary>
        ///     Deletes the supplied <see cref="Trip" /> from the database.
        /// </summary>
        /// <param name="trip"><see cref="Trip" /> object to delete.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Trip" /> could not be found in the database.
        /// </exception>
        void Delete(Trip trip);

        /// <summary>
        ///     Updates the <see cref="StopTime" /> associated with this trip.
        /// </summary>
        /// <param name="tripId">ID number of trip to update</param>
        /// <param name="stops">The stops which should be saved.</param>
        /// <returns>List of stops after saving.</returns>
        List<StopTime> UpdateStops(int tripId, List<StopTime> stops);

        /// <summary>
        ///     Return a list of <see cref="Stop"/>s that are assinged to the provided <see cref="Trip"/>.
        /// </summary>
        /// <param name="trip"><see cref="Trip"/> to filter stops by.</param>
        /// <returns>
        ///     Return a list of <see cref="Stop"/>s that are assinged to the provided <see cref="Trip"/>.
        /// </returns>
        List<StopTime> GetTripStops(Trip trip);

        /// <summary>
        ///     Finds <see cref="Trip"/>s passing through the two <see cref="Stop"/>s supplied inside the 
        ///     <see cref="RouteSearchFilter"/>, along with their origin and destination <see cref="StopTime"/>s.
        /// </summary>
        /// <param name="filter">Search filter for the trips.</param>
        /// <returns>List of <see cref="Trip"/>s passing through the specified stops.</returns>
        List<Tuple<Trip, StopTime, StopTime>> FindTrips(RouteSearchFilter filter);

        /// <summary>
        ///     Returns a list of <see cref="StopTime"/>s from a <see cref="Trip"/> with the supplied IDs, whose sequence numbers begin with <see cref="originSequenceNumber"/> and end with <see cref="destinationSequenceNumber"/>.
        /// </summary>
        /// <param name="filter">Object containing filtering parameters.</param>
        /// <returns>List of <see cref="StopTime"/> representing the desired segment of the trip.</returns>
        List<StopTime> GetTripSegment(TripSegmentFilter filter);
    }

    /// <summary>
    ///     Service for managing trips.
    /// </summary>
    public class TripRepository : ITripRepository
    {
        /// <summary>
        ///     An instance of database context.
        /// </summary>
        private readonly PublicTransportContext _db;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="db"><see cref="PublicTransportContext" /> to use during service operations.</param>
        public TripRepository(PublicTransportContext db)
        {
            _db = db;
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
            return _db.Trips
                .Include(t => t.Route)
                .Include(t => t.Service)
                .FirstOrDefault(u => u.Id == id);
        }

        /// <summary>
        ///     Updates all of the fields of the supplied <see cref="Trip" />.
        /// </summary>
        /// <param name="trip"><see cref="Trip" /> object to update.</param>
        /// <returns>Updated <see cref="Trip" /> object.</returns>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Trip" /> could not be found in the database.
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
            return old;
        }

        /// <summary>
        ///     Deletes the supplied <see cref="Trip" /> from the database.
        /// </summary>
        /// <param name="trip"><see cref="Trip" /> object to delete.</param>
        /// <exception cref="EntryNotFoundException">
        ///     Thrown when the supplied <see cref="Trip" /> could not be found in the database.
        /// </exception>
        public void Delete(Trip trip)
        {
            var old = Read(trip.Id);
            if (old == null)
            {
                throw new EntryNotFoundException();
            }

            _db.Trips.Remove(old);
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
            var sequence = 0;
            stops.ForEach(stopTime =>
            {
                stopTime.StopSequence = sequence++;
                stopTime.TripId = tripId;
            });

            var currentStops = _db.StopTimes.Where(st => st.TripId == tripId).ToList();
            var newStops =
                stops.Where(st => _db.StopTimes.FirstOrDefault(existing => existing.Id == st.Id) == null)
                    .ToList();
            var updatedStops = stops.Except(newStops).ToList();
            var deletedStops = currentStops
                //.Except(updatedStops)
                .Where(st => stops.Find(st2 => st.Id == st2.Id) == null)
                .ToList();

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

        /// <summary>
        ///     Return a list of <see cref="Stop"/>s that are assinged to the provided <see cref="Trip"/>.
        /// </summary>
        /// <param name="trip"><see cref="Trip"/> to filter stops by.</param>
        /// <returns>
        ///     Return a list of <see cref="Stop"/>s that are assinged to the provided <see cref="Trip"/>.
        /// </returns>
        public List<StopTime> GetTripStops(Trip trip)
        {
            return _db.StopTimes
                .Include(st => st.Stop)
                .Where(t => t.TripId == trip.Id)
                .OrderBy(t => t.StopSequence)
                .ToList();
        }

        /// <summary>
        ///     Finds <see cref="Trip"/>s passing through the two <see cref="Stop"/>s supplied inside the 
        ///     <see cref="RouteSearchFilter"/>, along with their origin and destination <see cref="StopTime"/>s.
        /// </summary>
        /// <param name="filter">Search filter for the trips.</param>
        /// <returns>List of <see cref="Trip"/>s passing through the specified stops.</returns>
        public List<Tuple<Trip, StopTime, StopTime>> FindTrips(RouteSearchFilter filter)
        {
            var originTimes = _db.StopTimes
                .Include(st => st.Stop.Street.City)
                .Include(st => st.Stop.ParentStation)
                .Where(st => st.StopId == filter.OriginStopIdFilter)
                .ToList();
            var destinationTimes = _db.StopTimes
                .Include(st => st.Stop.Street.City)
                .Include(st => st.Stop.ParentStation)
                .Where(st => st.StopId == filter.DestinationStopIdFilter)
                .ToList();
            return originTimes.Join(destinationTimes, st => st.TripId, st => st.TripId,
                    (t1, t2) => new Tuple<StopTime, StopTime>(t1, t2))
                .Where(tuple => tuple.Item1.StopSequence < tuple.Item2.StopSequence)
                .Take(20)
                .Select(tuple => new Tuple<Trip, StopTime, StopTime>(_db.Trips.Include(t => t.Service).Include(t => t.Route.Agency.Street.City).First(t => t.Id == tuple.Item1.TripId), tuple.Item1, tuple.Item2))
                .ToList();
        }

        /// <summary>
        ///     Returns a list of <see cref="StopTime"/>s from a <see cref="Trip"/> with the supplied IDs, whose sequence numbers begin with <see cref="originSequenceNumber"/> and end with <see cref="destinationSequenceNumber"/>.
        /// </summary>
        /// <param name="filter">Object containing filtering parameters.</param>
        /// <returns>List of <see cref="StopTime"/> representing the desired segment of the trip.</returns>
        public List<StopTime> GetTripSegment(TripSegmentFilter filter)
        {
            return _db.StopTimes
                .Include(st => st.Stop.Street.City)
                .Include(st => st.Shape)
                .Where(st => st.TripId == filter.TripId && 
                       st.StopSequence >= filter.OriginSequenceNumber && 
                       st.StopSequence <= filter.DestinationSequenceNumber)
                .OrderBy(st => st.StopSequence)
                .ToList();
        }
    }
}