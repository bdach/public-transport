using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Repositories;

namespace PublicTransport.Tests.Services
{
    [TestFixture]
    public class StopRepositoryTest : RepositoryTest
    {
        private StopRepository _stopRepository;

        [SetUp]
        public void ServiceSetUp()
        {
            _stopRepository = new StopRepository(DbContext);
        }

        [Test]
        public void GetStopsByRouteIdTest()
        {
            // given
            // when
            var stops = _stopRepository.GetStopsByRouteId(2); // 101 Zawiercie-Gliwice
            // then
            stops.Count.ShouldBeEquivalentTo(6);
            stops.Select(s => s.Id).ToList().ShouldAllBeEquivalentTo(Enumerable.Range(10, 6));
        }

        [Test]
        public void FilterStopsTest()
        {
            // given
            var stopFilter = new StopFilter
            {
                CityNameFilter = "Warszawa",
                StreetNameFilter = "Bora-Komorowskiego",
                StopNameFilter = "kiego",
                ZoneNameFilter = ""
            };
            // when
            var stops = _stopRepository.FilterStops(stopFilter);
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
