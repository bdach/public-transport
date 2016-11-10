using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using PublicTransport.Domain.Entities;

namespace PublicTransport.Services.Tests
{
    [TestFixture]
    public class TripServiceTest : ServiceTest
    {
        private TripService _tripService;

        [SetUp]
        public void ServiceSetUp()
        {
            _tripService = new TripService(DbContext);
        }

        [Test]
        public void GetTripStopsTest()
        {
            // given
            var trip = DbContext.Trips.Find(2);
            // when
            var stops = _tripService.GetTripStops(trip);
            // then
            stops.Count.ShouldBeEquivalentTo(9);
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
            _tripService.UpdateStops(1, stopTimes);
            // then
            var result = DbContext.StopTimes.Where(st => st.TripId == 1).ToList();
            result.Count.ShouldBeEquivalentTo(2);
            result.Select(s => s.ArrivalTime).ShouldAllBeEquivalentTo(new List<TimeSpan>
            {
                existingStopTime.ArrivalTime,
                newStopTime.ArrivalTime
            });
        }
    }
}