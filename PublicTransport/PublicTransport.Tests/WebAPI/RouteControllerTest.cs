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
    public class RouteControllerTest
    {
        private Mock<IRouteRepository> _routeRepository;
        private RouteController _routeController;

        [SetUp]
        public void SetUp()
        {
            _routeRepository = new Mock<IRouteRepository>();
            _routeController = new RouteController(_routeRepository.Object);
        }

        [Test]
        public void Filter()
        {
            // given
            var routeFilter = new RouteFilter
            {
                ShortNameFilter = "E-1"
            };
            var routes = new List<Route>
            {
                new Route { ShortName = "E-1" }
            };
            _routeRepository.Setup(rr => rr.FilterRoutes(routeFilter)).Returns(routes);
            // when
            var request = new HttpRequestMessage
            {
                Content = new ObjectContent(typeof(RouteFilter), routeFilter, new JsonMediaTypeFormatter()),
                Method = HttpMethod.Post
            };
            var response = _routeController.Filter(request, routeFilter) as OkNegotiatedContentResult<IEnumerable<RouteInfo>>;
            // then
            response.Should().NotBeNull();
            var items = response.Content.ToList();
            items.Count().ShouldBeEquivalentTo(1);
            items.Should().ContainSingle(ri => ri.ShortName == "E-1");
        }

        [Test]
        public void Filter_InvalidParameter()
        {
            // given
            var routeFilter = new RouteFilter();
            // when
            var request = new HttpRequestMessage
            {
                Content = new ObjectContent(routeFilter.GetType(), routeFilter, new JsonMediaTypeFormatter()),
                Method = HttpMethod.Post
            };
            var response = _routeController.Filter(request, routeFilter);
            // then
            response.Should().BeOfType<BadRequestErrorMessageResult>();
        }
    }
}