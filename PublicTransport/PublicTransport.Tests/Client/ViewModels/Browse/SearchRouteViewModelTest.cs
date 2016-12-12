using System.Reactive.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Client.Services.Search;
using PublicTransport.Client.ViewModels.Browse;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.DataTransfer.Filters;

namespace PublicTransport.Tests.Client.ViewModels.Browse
{
    [TestFixture]
    public class SearchRouteViewModelTest : RoutableViewModelTest
    {
        private Mock<ISearchService> _searchService;
        private SearchRouteViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _searchService = new Mock<ISearchService>();
            _viewModel = new SearchRouteViewModel(Screen.Object, _searchService.Object);
        }

        [Test]
        public void FindRoutes()
        {
            // given
            _searchService.Setup(r => r.FindRoutesAsync(It.IsAny<RouteSearchFilter>())).ReturnsAsync(new[] { new RouteDto() });
            // when
            _viewModel.FindRoutes.ExecuteAsync().Wait();
            // then
            _searchService.Verify(r => r.FindRoutesAsync(It.IsAny<RouteSearchFilter>()), Times.Once);
            _viewModel.Routes.Count.ShouldBeEquivalentTo(1);
        }

        [Test]
        public void FindRoutes_InvalidFilter()
        {
            // given
            // when
            _viewModel.RouteSearchReactiveFilter.OriginStopIdFilter = 0;
            _viewModel.RouteSearchReactiveFilter.DestinationStopIdFilter = 0;
            // then
            _searchService.Verify(r => r.FindRoutesAsync(It.IsAny<RouteSearchFilter>()), Times.Never);
        }
    }
}