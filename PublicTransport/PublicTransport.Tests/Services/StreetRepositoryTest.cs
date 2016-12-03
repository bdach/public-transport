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
        private Mock<StreetFilter> _streetFilter;
        private StreetRepository _streetRepository;

        [SetUp]
        public void ServiceSetUp()
        {
            _streetFilter = new Mock<StreetFilter>();
            _streetRepository = new StreetRepository(DbContext);
        }

        [Test]
        public void FilterStreetsTest()
        {
            // given
            _streetFilter.Setup(sf => sf.CityNameFilter).Returns("");
            _streetFilter.Setup(sf => sf.StreetNameFilter).Returns("3 Maja");
            // when
            var streets = _streetRepository.FilterStreets(_streetFilter.Object);
            // then
            streets.Count.ShouldBeEquivalentTo(2);
            streets.Should().Contain(s => s.City.Name == "Zawiercie");
            streets.Should().Contain(s => s.City.Name == "Sosnowiec");
        }
    }
}
