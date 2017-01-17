using System.Collections.Generic;
using System.Net.Http;
using Moq;
using NUnit.Framework;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.Repositories;
using PublicTransport.WebAPI.Controllers;

namespace PublicTransport.Tests.WebAPI
{
    [TestFixture]
    public class TimetableControllerTest
    {
        private Mock<IStopTimeRepository> _stopTimeRepository;
        private TimetableController _timetableController;

        [SetUp]
        public void SetUp()
        {
            _stopTimeRepository = new Mock<IStopTimeRepository>();
            _timetableController = new TimetableController(_stopTimeRepository.Object);
        }

        [Test]
        public void Stop()
        {
            // given
            var timetable = new Dictionary<Route, List<StopTime>>
            {
                {new Route {ShortName = "101"}, new List<StopTime> {new StopTime()} }
            };
            _stopTimeRepository.Setup(str => str.GetFullTimetableByStopId(10)).Returns(timetable);
            // when
            var request = new HttpRequestMessage {Method = HttpMethod.Get};
            _timetableController.Stop(request, 10);
            // then
            _stopTimeRepository.Verify(str => str.GetFullTimetableByStopId(10));;
        }

        [Test]
        public void Route()
        {
            // given
            var timetable = new Dictionary<Stop, List<StopTime>>();
            _stopTimeRepository.Setup(str => str.GetFullTimetableByRouteId(72)).Returns(timetable);
            // when
            var request = new HttpRequestMessage {Method = HttpMethod.Get};
            _timetableController.Route(request, 72);
            // then
            _stopTimeRepository.Verify(str => str.GetFullTimetableByRouteId(72));
        }
    }
}