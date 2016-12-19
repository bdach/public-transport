using FluentAssertions;
using NUnit.Framework;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Repositories;

namespace PublicTransport.Tests.Services
{
    [TestFixture]
    public class AgencyRepositoryTest : RepositoryTest
    {
        private AgencyRepository _agencyRepository;

        [SetUp]
        public void ServiceSetUp()
        {
            _agencyRepository = new AgencyRepository(DbContext);
        }

        [Test]
        public void FilterAgenciesTest()
        {
            // given
            var agencyFilter = new AgencyFilter
            {
                AgencyNameFilter = "PKP",
                CityNameFilter = "Warszawa",
                StreetNameFilter = "Żelazna"
            };
            // when
            var agencies = _agencyRepository.FilterAgencies(agencyFilter);
            // then
            agencies.Count.ShouldBeEquivalentTo(1);
            agencies.Should().ContainSingle(a => a.Name == "PKP Intercity");
        }
    }
}
