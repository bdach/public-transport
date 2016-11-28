using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Services;
using PublicTransport.Services.DataTransfer.Filters;

namespace PublicTransport.Tests.Services
{
    [TestFixture]
    public class StopTimeRepositoryTest : RepositoryTest
    {
        private StopTimeRepository _stopTimeRepository;
        private Mock<IStopTimeFilter> _stopTimeFilter;

        [SetUp]
        public void ServiceSetUp()
        {
            _stopTimeRepository = new StopTimeRepository(DbContext);
            _stopTimeFilter = new Mock<IStopTimeFilter>();
        }

        [Test]
        public void GetRouteTimetableByStopIdTest_Weekday_NoTime()
        {
            // given
            _stopTimeFilter.Setup(stf => stf.RouteId).Returns(1);
            _stopTimeFilter.Setup(stf => stf.Date).Returns(new DateTime(2016, 11, 10));
            _stopTimeFilter.Setup(stf => stf.StopId).Returns(6);
            // when
            var times = _stopTimeRepository.GetRouteTimetableByStopId(_stopTimeFilter.Object);
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
            _stopTimeFilter.Setup(stf => stf.RouteId).Returns(1);
            _stopTimeFilter.Setup(stf => stf.Date).Returns(new DateTime(2016, 11, 10));
            _stopTimeFilter.Setup(stf => stf.StopId).Returns(5);
            _stopTimeFilter.Setup(stf => stf.Time).Returns(new TimeSpan(12, 00, 00));
            // when
            var times = _stopTimeRepository.GetRouteTimetableByStopId(_stopTimeFilter.Object);
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
            _stopTimeFilter.Setup(stf => stf.RouteId).Returns(1);
            _stopTimeFilter.Setup(stf => stf.Date).Returns(new DateTime(2016, 11, 13));
            _stopTimeFilter.Setup(stf => stf.StopId).Returns(5);
            // when
            var times = _stopTimeRepository.GetRouteTimetableByStopId(_stopTimeFilter.Object);
            // then
            times.Select(s => s.ArrivalTime).Should().BeEmpty();
        }

        [Test]
        public void GetRouteTimetableByStopIdTest_BeforeStartDate()
        {
            // given
            _stopTimeFilter.Setup(stf => stf.RouteId).Returns(1);
            _stopTimeFilter.Setup(stf => stf.Date).Returns(new DateTime(2015, 11, 13));
            _stopTimeFilter.Setup(stf => stf.StopId).Returns(1);
            // when
            var times = _stopTimeRepository.GetRouteTimetableByStopId(_stopTimeFilter.Object);
            // then
            times.Select(s => s.ArrivalTime).Should().BeEmpty();
        }

        [Test]
        public void GetRouteTimetableByStopIdTest_AfterEndDate()
        {
            // given
            _stopTimeFilter.Setup(stf => stf.RouteId).Returns(1);
            _stopTimeFilter.Setup(stf => stf.Date).Returns(new DateTime(2018, 11, 13));
            _stopTimeFilter.Setup(stf => stf.StopId).Returns(1);
            // when
            var times = _stopTimeRepository.GetRouteTimetableByStopId(_stopTimeFilter.Object);
            // then
            times.Select(s => s.ArrivalTime).Should().BeEmpty();
        }
    }
}