using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Repositories;

namespace PublicTransport.Tests.Services
{
    [TestFixture]
    public class StopTimeRepositoryTest : RepositoryTest
    {
        private StopTimeRepository _stopTimeRepository;
        private StopTimeFilter _stopTimeFilter;

        [SetUp]
        public void ServiceSetUp()
        {
            _stopTimeRepository = new StopTimeRepository(DbContext);
            _stopTimeFilter = new StopTimeFilter();
        }

        [Test]
        public void GetRouteTimetableByStopIdTest_Weekday_NoTime()
        {
            // given
            _stopTimeFilter.RouteId = 1;
            _stopTimeFilter.Date = new DateTime(2016, 11, 10);
            _stopTimeFilter.StopId = 6;
            // when
            var times = _stopTimeRepository.GetRouteTimetableByStopId(_stopTimeFilter);
            // then
            times.Count.ShouldBeEquivalentTo(2);
            times.Select(s => s.ArrivalTime).ShouldBeEquivalentTo(new List<TimeSpan>
            {
                new TimeSpan(9, 11, 00),
                new TimeSpan(15, 07, 00)
            });
        }

        [Test]
        public void GetRouteTimetableByStopIdTest_Weekday_TimeSpecified()
        {
            // given
            _stopTimeFilter.RouteId = 1;
            _stopTimeFilter.Date = new DateTime(2016, 11, 10);
            _stopTimeFilter.StopId = 5;
            _stopTimeFilter.Time = new TimeSpan(12, 00, 00);
            // when
            var times = _stopTimeRepository.GetRouteTimetableByStopId(_stopTimeFilter);
            // then
            times.Select(s => s.ArrivalTime).ShouldAllBeEquivalentTo(new List<TimeSpan>
            {
                new TimeSpan(15, 08, 00)
            });
        }

        [Test]
        public void GetRouteTimetableByStopIdTest_Weekends()
        {
            // given
            _stopTimeFilter.RouteId = 1;
            _stopTimeFilter.Date = new DateTime(2016, 11, 13);
            _stopTimeFilter.StopId = 5;
            // when
            var times = _stopTimeRepository.GetRouteTimetableByStopId(_stopTimeFilter);
            // then
            times.Select(s => s.ArrivalTime).Should().BeEmpty();
        }

        [Test]
        public void GetRouteTimetableByStopIdTest_BeforeStartDate()
        {
            // given
            _stopTimeFilter.RouteId = 1;
            _stopTimeFilter.Date = new DateTime(2015, 11, 13);
            _stopTimeFilter.StopId = 1;
            // when
            var times = _stopTimeRepository.GetRouteTimetableByStopId(_stopTimeFilter);
            // then
            times.Select(s => s.ArrivalTime).Should().BeEmpty();
        }

        [Test]
        public void GetRouteTimetableByStopIdTest_AfterEndDate()
        {
            // given
            _stopTimeFilter.RouteId = 1;
            _stopTimeFilter.Date = new DateTime(2018, 11, 13);
            _stopTimeFilter.StopId = 1;
            // when
            var times = _stopTimeRepository.GetRouteTimetableByStopId(_stopTimeFilter);
            // then
            times.Select(s => s.ArrivalTime).Should().BeEmpty();
        }
    }
}