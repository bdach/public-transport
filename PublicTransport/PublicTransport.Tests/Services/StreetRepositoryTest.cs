using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Repositories;

namespace PublicTransport.Tests.Services
{
    [TestFixture]
    public class StreetRepositoryTest : RepositoryTest
    {
        private StreetRepository _streetRepository;

        [SetUp]
        public void ServiceSetUp()
        {
            _streetRepository = new StreetRepository(DbContext);
        }

        [Test]
        public void FilterStreetsTest()
        {
            // given
            var streetFilter = new StreetFilter
            {
                CityNameFilter = "",
                StreetNameFilter = "3 Maja"
            };
            // when
            var streets = _streetRepository.FilterStreets(streetFilter);
            // then
            streets.Count.ShouldBeEquivalentTo(2);
            streets.Should().Contain(s => s.City.Name == "Zawiercie");
            streets.Should().Contain(s => s.City.Name == "Sosnowiec");
        }
    }
}
