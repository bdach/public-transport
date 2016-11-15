using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Services;
using PublicTransport.Services.DataTransfer.Filters;

namespace PublicTransport.Tests.Services
{
    [TestFixture]
    public class StreetServiceTest : ServiceTest
    {
        private StreetService _streetService;
        private Mock<IStreetFilter> _streetFilter;

        [SetUp]
        public void ServiceSetUp()
        {
            _streetService = new StreetService(DbContext);
            _streetFilter = new Mock<IStreetFilter>();
        }

        [Test]
        public void FilterStreetsTest()
        {
            // given
            _streetFilter.Setup(sf => sf.CityNameFilter).Returns("");
            _streetFilter.Setup(sf => sf.StreetNameFilter).Returns("3 Maja");
            // when
            var streets = _streetService.FilterStreets(_streetFilter.Object);
            // then
            streets.Count.ShouldBeEquivalentTo(2);
            streets.Should().Contain(s => s.City.Name == "Zawiercie");
            streets.Should().Contain(s => s.City.Name == "Sosnowiec");
        }
    }
}
