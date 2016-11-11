using System;
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
using PublicTransport.Services.UnitsOfWork;
using ReactiveUI;

namespace PublicTransport.Tests.Client.ViewModels.Filter
{
    [TestFixture]
    public class FilterRouteViewModelTest
    {
        private readonly Mock<IScreen> _screen = new Mock<IScreen>();
        private readonly RoutingState _router = new RoutingState();
        private readonly Mock<IRouteUnitOfWork> _routeUnitOfWork = new Mock<IRouteUnitOfWork>();
        private FilterRouteViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _screen.Setup(s => s.Router).Returns(_router);
            _viewModel = new FilterRouteViewModel(_screen.Object, _routeUnitOfWork.Object);
        }

        [Test]
        public void FilterRoutes()
        {
            // given
            _viewModel.RouteFilter = new RouteFilter();
            // when
            _viewModel.FilterRoutes.Execute(null);
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
            _router.Navigate
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
            _router.Navigate
                .Where(vm => vm is EditRouteViewModel)
                .Subscribe(_ => navigatedToEdit = true);
            // when
            _viewModel.AddRoute.Execute(null);
            // then
            navigatedToEdit.Should().BeTrue();
            var editRouteViewModel = _router.GetCurrentViewModel() as EditRouteViewModel;
            editRouteViewModel.Should().NotBeNull();
            editRouteViewModel.Route.ShouldBeEquivalentTo(_viewModel.SelectedRoute);
        }

        [Test]
        public void ShowTimeTable()
        {
            // given
            var navigatedToTimetable = false;
            _viewModel.SelectedRoute = new Route();
            _router.Navigate
                .Where(vm => vm is TimetableViewModel)
                .Subscribe(_ => navigatedToTimetable = true);
            // when
            _viewModel.ShowTimetable.Execute(null);
            // then
            navigatedToTimetable.Should().BeTrue();
            var timetableViewModel = _router.GetCurrentViewModel() as TimetableViewModel;
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