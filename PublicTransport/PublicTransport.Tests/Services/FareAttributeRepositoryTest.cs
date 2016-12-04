using FluentAssertions;
using NUnit.Framework;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Repositories;

namespace PublicTransport.Tests.Services
{
    [TestFixture]
    public class FareAttributeRepositoryTest : RepositoryTest
    {
        private FareAttributeRepository _fareAttributeRepository;
        private FareFilter _fareFilter;

        [SetUp]
        public void ServiceSetUp()
        {
            _fareFilter = new FareFilter();
            _fareAttributeRepository = new FareAttributeRepository(DbContext);
        }

        [Test]
        public void FilterFares()
        {
            // given
            _fareFilter.DestinationZoneNameFilter = "";
            _fareFilter.OriginZoneNameFilter = "";
            _fareFilter.RouteNameFilter = "E-1";
            // when
            var fareAttributes = _fareAttributeRepository.FilterFares(_fareFilter);
            // then
            fareAttributes.Count.ShouldBeEquivalentTo(2);
        }
    }
}
