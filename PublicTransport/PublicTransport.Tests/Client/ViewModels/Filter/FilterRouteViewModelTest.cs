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
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.UnitsOfWork;
using ReactiveUI;

namespace PublicTransport.Tests.Client.ViewModels.Filter
{
    [TestFixture]
    public class FilterRouteViewModelTest : RoutableViewModelTest
    {
        private readonly Mock<IRouteUnitOfWork> _routeUnitOfWork = new Mock<IRouteUnitOfWork>();
        private FilterRouteViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _viewModel = new FilterRouteViewModel(Screen.Object, _routeUnitOfWork.Object);
            _routeUnitOfWork.Setup(r => r.FilterAgencies(It.IsAny<IAgencyFilter>())).Returns(new List<Agency>());
            _routeUnitOfWork.Setup(r => r.FilterRoutes(It.IsAny<IRouteFilter>())).Returns(new List<Route>());
            _routeUnitOfWork.Setup(r => r.FilterStops(It.IsAny<IStopFilter>())).Returns(new List<Stop>());
        }

        [Test]
        public void FilterRoutes()
        {
            // given
            _viewModel.RouteFilter = new RouteFilter();
            // when
            _viewModel.FilterRoutes.ExecuteAsync().Wait();
            // then
            _routeUnitOfWork.Verify(r => r.FilterRoutes(_viewModel.RouteFilter), Times.Once);
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
            _routeUnitOfWork.Verify(r => r.DeleteRoute(route), Times.Once);
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
        public void AddRoute()
        {
            // given
            var navigatedToEdit = false;
            Router.Navigate
                .Where(vm => vm is EditRouteViewModel)
                .Subscribe(_ => navigatedToEdit = true);
            // when
            _viewModel.AddRoute.Execute(null);
            // then
            navigatedToEdit.Should().BeTrue();
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