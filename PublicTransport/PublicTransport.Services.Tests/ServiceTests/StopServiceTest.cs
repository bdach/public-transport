using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Services.DataTransfer.Filters;

namespace PublicTransport.Services.Tests
{
    [TestFixture]
    public class StopServiceTest : ServiceTest
    {
        private StopService _stopService;
        private Mock<IStopFilter> _stopFilter;

        [SetUp]
        public void ServiceSetUp()
        {
            _stopService = new StopService(DbContext);
            _stopFilter = new Mock<IStopFilter>();
        }

        [Test]
        public void GetStopsByRouteIdTest()
        {
            // given
            // when
            var stops = _stopService.GetStopsByRouteId(2); // 101 Zawiercie-Gliwice
            // then
            stops.Count.ShouldBeEquivalentTo(6);
            stops.Select(s => s.Id).ToList().ShouldAllBeEquivalentTo(Enumerable.Range(10, 6));
        }

        [Test]
        public void FilterStopsTest()
        {
            // given
            _stopFilter.Setup(sf => sf.CityNameFilter).Returns("Warszawa");
            _stopFilter.Setup(sf => sf.StreetNameFilter).Returns("Bora-Komorowskiego");
            _stopFilter.Setup(sf => sf.StopNameFilter).Returns("kiego");
            _stopFilter.Setup(sf => sf.ZoneNameFilter).Returns("");
            // when
            var stops = _stopService.FilterStops(_stopFilter.Object);
            // then
            stops.Count.ShouldBeEquivalentTo(2);
            stops.Select(s => s.Name)
                .ToList()
                .ShouldAllBeEquivalentTo(new List<string>
                {
                    "Horbaczewskiego",
                    "Bora-Komorowskiego"
                });
        }
    }
}
