using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http.Results;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Repositories;
using PublicTransport.WebAPI.Controllers;
using PublicTransport.WebAPI.Models;

namespace PublicTransport.Tests.WebAPI
{
    [TestFixture]
    public class StopControllerTest
    {
        private Mock<IStopRepository> _stopRepository;
        private StopController _stopController;

        [SetUp]
        public void SetUp()
        {
            _stopRepository = new Mock<IStopRepository>();
            _stopController = new StopController(_stopRepository.Object);
        }

        [Test]
        public void Filter()
        {
            // given
            var stopFilter = new StopFilter {StopNameFilter = "Metro"};
            var results = new List<Stop>
            {
                new Stop {Name = "Metro Stadion Narodowy"},
                new Stop {Name = "Metro Politechnika"}
            };
            _stopRepository.Setup(sr => sr.FilterStops(stopFilter)).Returns(results);
            // when
            var request = new HttpRequestMessage
            {
                Content = new ObjectContent(stopFilter.GetType(), stopFilter, new JsonMediaTypeFormatter()),
                Method = HttpMethod.Post
            };
            var response = _stopController.Filter(request, stopFilter) as OkNegotiatedContentResult<IEnumerable<StopInfo>>;
            // then
            response.Should().NotBeNull();
            response.Content.Count().ShouldBeEquivalentTo(2);
            response.Content.Should().ContainSingle(si => si.Name == "Metro Stadion Narodowy");
            response.Content.Should().ContainSingle(si => si.Name == "Metro Politechnika");
        }

        [Test]
        public void Filter_InvalidParameter()
        {
            // given
            var stopFilter = new StopFilter();
            // when
            var request = new HttpRequestMessage
            {
                Content = new ObjectContent(stopFilter.GetType(), stopFilter, new JsonMediaTypeFormatter()),
                Method = HttpMethod.Post
            };
            var response = _stopController.Filter(request, stopFilter);
            // then
            response.Should().BeOfType<BadRequestErrorMessageResult>();
        }
    }
}