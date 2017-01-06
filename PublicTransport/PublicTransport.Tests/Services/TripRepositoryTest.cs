using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Repositories;

namespace PublicTransport.Tests.Services
{
    [TestFixture]
    public class TripRepositoryTest : RepositoryTest
    {
        private TripRepository _tripRepository;

        [SetUp]
        public void ServiceSetUp()
        {
            _tripRepository = new TripRepository(DbContext);
        }

        [Test]
        public void GetTripStopsTest()
        {
            // given
            var trip = DbContext.Trips.Find(2);
            // when
            var stops = _tripRepository.GetTripStops(trip);
            // then
            stops.Count.ShouldBeEquivalentTo(10);
            stops.Select(s => s.StopId).Should().ContainInOrder(Enumerable.Range(1, 9).Reverse());
        }

        [Test]
        public void UpdateStopsTest()
        {
            // given
            var existingStopTime = DbContext.StopTimes.Find(5);
            var newStopTime = new StopTime
            {
                ArrivalTime = new TimeSpan(23, 44, 00),
                DepartureTime = new TimeSpan(23, 44, 00),
                StopId = 4,
                TripId = 1,
                StopSequence = 3
            };
            var stopTimes = new List<StopTime> {existingStopTime, newStopTime};
            // when
            _tripRepository.UpdateStops(1, stopTimes);
            // then
            var result = DbContext.StopTimes.Where(st => st.TripId == 1).ToList();
            result.Count.ShouldBeEquivalentTo(2);
            result.Select(s => s.ArrivalTime).ShouldAllBeEquivalentTo(new List<TimeSpan>
            {
                existingStopTime.ArrivalTime,
                newStopTime.ArrivalTime
            });
        }

        [Test]
        public void GetTripSegment()
        {
            // given
            var tripSegmentFilter = new TripSegmentFilter
            {
                TripId = 1,
                OriginSequenceNumber = 3,
                DestinationSequenceNumber = 8
            };
            // when
            var tripSegment = _tripRepository.GetTripSegment(tripSegmentFilter);
            // then
            tripSegment.Count.ShouldBeEquivalentTo(6);
            tripSegment.First().StopSequence.ShouldBeEquivalentTo(3);
            tripSegment.Last().StopSequence.ShouldBeEquivalentTo(8);
            tripSegment.Should().BeInAscendingOrder(st => st.StopSequence);
        }

        [Test]
        public void GetTripSegment_InvalidSequenceNumbers()
        {
            // given
            var tripSegmentFilter = new TripSegmentFilter
            {
                TripId = 2,
                OriginSequenceNumber = 8,
                DestinationSequenceNumber = 5
            };
            // when
            var tripSegment = _tripRepository.GetTripSegment(tripSegmentFilter);
            // then
            tripSegment.Count.ShouldBeEquivalentTo(0);
        }
    }
}