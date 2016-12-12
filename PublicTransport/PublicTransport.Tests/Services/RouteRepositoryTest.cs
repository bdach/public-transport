using FluentAssertions;
using NUnit.Framework;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Repositories;

namespace PublicTransport.Tests.Services
{
    [TestFixture]
    public class RouteRepositoryTest : RepositoryTest
    {
        private RouteRepository _routeRepository;
        private RouteSearchFilter _routeSearchFilter;

        [SetUp]
        public void RepositorySetUp()
        {
            _routeRepository = new RouteRepository(DbContext);
            _routeSearchFilter = new RouteSearchFilter();
        }

        [Test]
        public void FindRoutes()
        {
            // given
            _routeSearchFilter.DestinationStopIdFilter = 1;
            _routeSearchFilter.OriginStopIdFilter = 3;
            // when
            var result = _routeRepository.FindRoutes(_routeSearchFilter);
            // then
            result.Count.ShouldBeEquivalentTo(1);
        }

        [Test]
        public void FindRoutes_NoRouteFound()
        {
            // given
            _routeSearchFilter.OriginStopIdFilter = 1;
            _routeSearchFilter.DestinationStopIdFilter = 11;
            // when
            var result = _routeRepository.FindRoutes(_routeSearchFilter);
            // then
            result.Should().BeEmpty();
        }
    }
}