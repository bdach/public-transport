using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Repositories;

namespace PublicTransport.Tests.Services
{
    [TestFixture]
    public class FareAttributeRepositoryTest : RepositoryTest
    {
        private FareAttributeRepository _fareAttributeRepository;
        private Mock<IFareFilter> _fareFilter;

        [SetUp]
        public void ServiceSetUp()
        {
            _fareFilter = new Mock<IFareFilter>();
            _fareAttributeRepository = new FareAttributeRepository(DbContext);
        }

        [Test]
        public void FilterFares()
        {
            // given
            _fareFilter.Setup(ff => ff.DestinationZoneNameFilter).Returns("");
            _fareFilter.Setup(ff => ff.OriginZoneNameFilter).Returns("");
            _fareFilter.Setup(ff => ff.RouteNameFilter).Returns("E-1");
            // when
            var fareAttributes = _fareAttributeRepository.FilterFares(_fareFilter.Object);
            // then
            fareAttributes.Count.ShouldBeEquivalentTo(2);
        }
    }
}
