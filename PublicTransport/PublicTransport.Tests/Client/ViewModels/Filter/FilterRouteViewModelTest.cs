using System;
using System.Reactive.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Client.DataTransfer;
using PublicTransport.Client.Services.Routes;
using PublicTransport.Client.ViewModels.Browse;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Client.ViewModels.Filter;
using PublicTransport.Domain.Enums;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.DataTransfer.Filters;
using ReactiveUI;

namespace PublicTransport.Tests.Client.ViewModels.Filter
{
    [TestFixture]
    public class FilterRouteViewModelTest : RoutableViewModelTest
    {
        private Mock<IRouteService> _routeService;
        private FilterRouteViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _routeService = new Mock<IRouteService>();
            _viewModel = new FilterRouteViewModel(Screen.Object, _routeService.Object);
        }

        [Test]
        public void FilterRoutes()
        {
            // given
            _routeService.Setup(r => r.FilterRoutesAsync(It.IsAny<RouteFilter>())).ReturnsAsync(new[] { new RouteDto() });
            // when
            _viewModel.FilterRoutes.ExecuteAsync().Wait();
            // then
            _routeService.Verify(r => r.FilterRoutesAsync(It.IsAny<RouteFilter>()), Times.Once);
            _viewModel.Routes.Count.ShouldBeEquivalentTo(1);
        }

        [Test]
        public void FilterRoutes_InvalidFilter()
        {
            // given
            // when
            _viewModel.RouteReactiveFilter.AgencyNameFilter = "";
            _viewModel.RouteReactiveFilter.LongNameFilter = "";
            _viewModel.RouteReactiveFilter.ShortNameFilter = "";
            _viewModel.RouteReactiveFilter.RouteTypeFilter = null;
            // then
            _routeService.Verify(r => r.FilterRoutesAsync(It.IsAny<RouteFilter>()), Times.Never);
        }

        [Test]
        public void DeleteRoute()
        {
            // given
            var route = new RouteDto();
            // when
            _viewModel.SelectedRoute = route;
            _viewModel.DeleteRoute.Execute(null);
            // then
            _routeService.Verify(r => r.DeleteRouteAsync(route), Times.Once);
        }

        [Test]
        public void AddRoute()
        {
            // given
            var navigatedToEdit = false;
            _viewModel.SelectedRoute = new RouteDto();
            Router.Navigate
                .Where(vm => vm is EditRouteViewModel)
                .Subscribe(_ => navigatedToEdit = true);
            // when
            _viewModel.AddRoute.Execute(null);
            // then
            navigatedToEdit.Should().BeTrue();
            var editRouteViewModel = Router.GetCurrentViewModel() as EditRouteViewModel;
            editRouteViewModel.Should().NotBeNull();
            editRouteViewModel.Route.Should().NotBe(_viewModel.SelectedRoute);
        }

        [Test]
        public void EditRoute()
        {
            // given
            var navigatedToEdit = false;
            _viewModel.SelectedRoute = new RouteDto();
            Router.Navigate
                .Where(vm => vm is EditRouteViewModel)
                .Subscribe(_ => navigatedToEdit = true);
            // when
            _viewModel.AddRoute.Execute(null);
            // then
            navigatedToEdit.Should().BeTrue();
            var editRouteViewModel = Router.GetCurrentViewModel() as EditRouteViewModel;
            editRouteViewModel.Should().NotBeNull();
            editRouteViewModel.Route.ShouldBeEquivalentTo(_viewModel.SelectedRoute);
        }

        [Test]
        public void EditDeleteRoute_CannotExecuteIfNoRouteSelected()
        {
            // given
            // when
            _viewModel.SelectedRoute = null;
            // then
            _viewModel.EditRoute.CanExecute(null).Should().BeFalse();
            _viewModel.DeleteRoute.CanExecute(null).Should().BeFalse();
        }

        [Test]
        public void ShowTimeTable()
        {
            // given
            var navigatedToTimetable = false;
            _viewModel.SelectedRoute = new RouteDto();
            Router.Navigate
                .Where(vm => vm is RouteTimetableViewModel)
                .Subscribe(_ => navigatedToTimetable = true);
            // when
            _viewModel.ShowTimetable.Execute(null);
            // then
            navigatedToTimetable.Should().BeTrue();
            var timetableViewModel = Router.GetCurrentViewModel() as RouteTimetableViewModel;
            timetableViewModel.Should().NotBeNull();
            timetableViewModel.Route.ShouldBeEquivalentTo(_viewModel.SelectedRoute);
        }

        [Test]
        public void ClearRouteType()
        {
            // given
            _viewModel.RouteReactiveFilter = new RouteReactiveFilter { RouteTypeFilter = RouteType.Bus };
            // when
            _viewModel.ClearRouteTypeChoice.Execute(null);
            // then
            _viewModel.RouteReactiveFilter.RouteTypeFilter.Should().BeNull();
        }
    }
}