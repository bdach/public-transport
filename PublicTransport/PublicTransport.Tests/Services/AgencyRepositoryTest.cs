using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Repositories;

namespace PublicTransport.Tests.Services
{
    [TestFixture]
    public class AgencyRepositoryTest : RepositoryTest
    {
        private Mock<IAgencyFilter> _agencyFilter;
        private AgencyRepository _agencyRepository;

        [SetUp]
        public void ServiceSetUp()
        {
            _agencyFilter = new Mock<IAgencyFilter>();
            _agencyRepository = new AgencyRepository(DbContext);
        }

        [Test]
        public void FilterAgenciesTest()
        {
            // given
            _agencyFilter.Setup(af => af.CityNameFilter).Returns("Warszawa");
            _agencyFilter.Setup(af => af.AgencyNameFilter).Returns("PKP");
            _agencyFilter.Setup(af => af.StreetNameFilter).Returns("Żelazna");
            // when
            var agencies = _agencyRepository.FilterAgencies(_agencyFilter.Object);
            // then
            agencies.Count.ShouldBeEquivalentTo(1);
            agencies.Should().ContainSingle(a => a.Name == "PKP Intercity");
        }
    }
}
