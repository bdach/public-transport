using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Client.DataTransfer;
using PublicTransport.Client.ViewModels.Browse;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Client.ViewModels.Filter;
using PublicTransport.Domain.Entities;
using PublicTransport.Domain.Enums;
using PublicTransport.Services;
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
            _routeService.Setup(r => r.FilterRoutes(It.IsAny<IRouteFilter>())).Returns(new List<Route> { new Route() });
            // when
            _viewModel.FilterRoutes.ExecuteAsync().Wait();
            // then
            _routeService.Verify(r => r.FilterRoutes(_viewModel.RouteFilter), Times.Once);
            _viewModel.Routes.Count.ShouldBeEquivalentTo(1);
        }

        [Test]
        public void FilterRoutes_InvalidFilter()
        {
            // given
            // when
            _viewModel.RouteFilter.AgencyNameFilter = "";
            _viewModel.RouteFilter.LongNameFilter = "";
            _viewModel.RouteFilter.ShortNameFilter = "";
            _viewModel.RouteFilter.RouteTypeFilter = null;
            // then
            _routeService.Verify(r => r.FilterRoutes(It.IsAny<IRouteFilter>()), Times.Never);
        }

        [Test]
        public void DeleteRoute()
        {
            // given
            var route = new Route();
            // when
            _viewModel.SelectedRoute = route;
            _viewModel.DeleteRoute.Execute(null);
            // then
            _routeService.Verify(r => r.DeleteRoute(route), Times.Once);
        }

        [Test]
        public void AddRoute()
        {
            // given
            var navigatedToEdit = false;
            _viewModel.SelectedRoute = new Route();
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
            _viewModel.SelectedRoute = new Route();
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
            _viewModel.SelectedRoute = new Route();
            Router.Navigate
                .Where(vm => vm is TimetableViewModel)
                .Subscribe(_ => navigatedToTimetable = true);
            // when
            _viewModel.ShowTimetable.Execute(null);
            // then
            navigatedToTimetable.Should().BeTrue();
            var timetableViewModel = Router.GetCurrentViewModel() as TimetableViewModel;
            timetableViewModel.Should().NotBeNull();
            timetableViewModel.Route.ShouldBeEquivalentTo(_viewModel.SelectedRoute);
        }

        [Test]
        public void ClearRouteType()
        {
            // given
            _viewModel.RouteFilter = new RouteFilter { RouteTypeFilter = RouteType.Bus };
            // when
            _viewModel.ClearRouteTypeChoice.Execute(null);
            // then
            _viewModel.RouteFilter.RouteTypeFilter.Should().BeNull();
        }
    }
}