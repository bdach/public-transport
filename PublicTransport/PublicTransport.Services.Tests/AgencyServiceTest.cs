using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Services.DataTransfer.Filters;

namespace PublicTransport.Services.Tests
{
    [TestFixture]
    public class AgencyServiceTest : ServiceTest
    {
        private Mock<IAgencyFilter> _agencyFilter;
        private AgencyService _agencyService;

        [SetUp]
        public void ServiceSetUp()
        {
            _agencyFilter = new Mock<IAgencyFilter>();
            _agencyService = new AgencyService(DbContext);
        }

        [Test]
        public void FilterAgenciesTest()
        {
            // given
            _agencyFilter.Setup(af => af.CityNameFilter).Returns("Warszawa");
            _agencyFilter.Setup(af => af.AgencyNameFilter).Returns("PKP");
            _agencyFilter.Setup(af => af.StreetNameFilter).Returns("Żelazna");
            // when
            var agencies = _agencyService.FilterAgencies(_agencyFilter.Object);
            // then
            agencies.Count.ShouldBeEquivalentTo(1);
            agencies.Should().ContainSingle(a => a.Name == "PKP Intercity");
        }
    }
}
