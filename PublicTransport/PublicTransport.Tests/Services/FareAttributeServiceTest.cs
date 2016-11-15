using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Services;
using PublicTransport.Services.DataTransfer.Filters;

namespace PublicTransport.Tests.Services
{
    [TestFixture]
    public class FareAttributeServiceTest : ServiceTest
    {
        private FareAttributeService _fareAttributeService;
        private Mock<IFareFilter> _fareFilter;

        [SetUp]
        public void ServiceSetUp()
        {
            _fareFilter = new Mock<IFareFilter>();
            _fareAttributeService = new FareAttributeService(DbContext);
        }

        [Test]
        public void FilterFares()
        {
            // given
            _fareFilter.Setup(ff => ff.DestinationZoneNameFilter).Returns("");
            _fareFilter.Setup(ff => ff.OriginZoneNameFilter).Returns("");
            _fareFilter.Setup(ff => ff.RouteNameFilter).Returns("E-1");
            // when
            var fareAttributes = _fareAttributeService.FilterFares(_fareFilter.Object);
            // then
            fareAttributes.Count.ShouldBeEquivalentTo(2);
        }
    }
}
