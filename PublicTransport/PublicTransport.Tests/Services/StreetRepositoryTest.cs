using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Services;
using PublicTransport.Services.DataTransfer.Filters;

namespace PublicTransport.Tests.Services
{
    [TestFixture]
    public class StreetRepositoryTest : RepositoryTest
    {
        private StreetRepository _streetRepository;
        private Mock<IStreetFilter> _streetFilter;

        [SetUp]
        public void ServiceSetUp()
        {
            _streetRepository = new StreetRepository(DbContext);
            _streetFilter = new Mock<IStreetFilter>();
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
