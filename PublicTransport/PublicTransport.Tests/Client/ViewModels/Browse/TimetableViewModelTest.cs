using System;
using System.Reactive.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Client.Services.Routes;
using PublicTransport.Client.ViewModels.Browse;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.DataTransfer.Filters;
using ReactiveUI;

namespace PublicTransport.Tests.Client.ViewModels.Browse
{
    [TestFixture]
    public class TimetableViewModelTest : RoutableViewModelTest
    {
        private Mock<IRouteService> _routeService;
        private TimetableViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _routeService = new Mock<IRouteService>();
            _viewModel = new TimetableViewModel(Screen.Object, _routeService.Object, new RouteDto());
        }

        [Test]
        public void DeleteTrip()
        {
            // given
            var stopTime = new StopTimeDto { Trip = new TripDto() };
            // when
            _viewModel.SelectedStopTime = stopTime;
            _viewModel.DeleteTrip.ExecuteAsyncTask().Wait();
            // then
            _routeService.Verify(r => r.DeleteTripAsync(stopTime.Trip), Times.Once);
        }

        [Test]
        public void AddTrip()
        {
            // given
            var navigatedToEdit = false;
            Router.Navigate
                .Where(vm => vm is EditTripViewModel)
                .Subscribe(_ => navigatedToEdit = true);
            // when
            _viewModel.AddTrip.Execute(null);
            // then
            navigatedToEdit.Should().BeTrue();
            var editViewModel = Router.GetCurrentViewModel() as EditTripViewModel;
            editViewModel.Should().NotBeNull();
            editViewModel.Trip.Route.Should().Be(_viewModel.Route);
        }

        [Test]
        public void EditTrip()
        {
            // given
            _routeService.Setup(r => r.GetTripStopsAsync(It.IsAny<TripDto>())).ReturnsAsync(new[] { new StopTimeDto() });
            var navigatedToEdit = false;
            _viewModel.SelectedStopTime = new StopTimeDto { Trip = new TripDto() };
            Router.CurrentViewModel
                .Where(vm => vm is EditTripViewModel)
                .Subscribe(_ => navigatedToEdit = true);
            // when
            _viewModel.EditTrip.ExecuteAsyncTask().Wait();
            // then
            navigatedToEdit.Should().BeTrue();
            var editViewModel = Router.GetCurrentViewModel() as EditTripViewModel;
            editViewModel.Should().NotBeNull();
            editViewModel.Trip.ShouldBeEquivalentTo(_viewModel.SelectedStopTime.Trip);
        }

        [Test]
        public void EditDeleteAgency_CannotExecuteIfNoAgencySelected()
        {
            // given
            // when
            _viewModel.SelectedStopTime = null;
            // then
            _viewModel.EditTrip.CanExecute(null).Should().BeFalse();
            _viewModel.DeleteTrip.CanExecute(null).Should().BeFalse();
        }

        [Test]
        public void GetStops()
        {
            // given
            _routeService.Setup(r => r.GetStopsByRouteIdAsync(It.IsAny<int>())).ReturnsAsync(new[] { new StopDto() });
            // when
            _viewModel.GetStops.ExecuteAsyncTask().Wait();
            // then
            _routeService.Verify(u => u.GetStopsByRouteIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void UpdateStopTimes()
        {
            // given
            _routeService.Setup(r => r.GetRouteTimetableByStopIdAsync(It.IsAny<StopTimeFilter>())).ReturnsAsync(new[] { new StopTimeDto() });
            // when
            _viewModel.UpdateStopTimes.ExecuteAsyncTask().Wait();
            // then
            _routeService.Verify(u => u.GetRouteTimetableByStopIdAsync(It.IsAny<StopTimeFilter>()), Times.Once);
        }
    }
}