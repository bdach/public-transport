using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using FluentAssertions;
using Microsoft.Reactive.Testing;
using Moq;
using NUnit.Framework;
using PublicTransport.Client.Services.Routes;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.DataTransfer.Filters;
using ReactiveUI.Testing;

namespace PublicTransport.Tests.Client.ViewModels.Edit
{
    public class EditTripViewModelTest : RoutableChildViewModelTest
    {
        private Mock<IRouteService> _routeService;
        private EditTripViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _routeService = new Mock<IRouteService>();
        }

        [Test]
        public void AddingNewTrip()
        {
            // given
            var stops = new List<StopDto> { new StopDto {Id = 3}, new StopDto {Id = 5} };
            var route = new RouteDto {Id = 10};
            // when
            _viewModel = new EditTripViewModel(Screen.Object, _routeService.Object, route, stops);
            // then
            _viewModel.Trip.Route.ShouldBeEquivalentTo(route);
            _viewModel.Trip.Route.Id.ShouldBeEquivalentTo(10);
            _viewModel.StopTimes.Select(s => s.StopTime.Stop).ShouldAllBeEquivalentTo(stops);
        }

        public void EditingExistingTrip()
        {
            // given
            var stopTimes = new[] {new StopTimeDto(), new StopTimeDto()};
            var trip = new TripDto();
            _routeService.Setup(r => r.GetTripStopsAsync(trip)).ReturnsAsync(stopTimes);
            // when
            _viewModel = new EditTripViewModel(Screen.Object, _routeService.Object, trip);
            // then
            _viewModel.Trip.ShouldBeEquivalentTo(trip);
            _viewModel.StopTimes.Select(s => s.StopTime).ShouldAllBeEquivalentTo(stopTimes);
        }

        [Test]
        public void Save_CanSave()
        {
            // given
            var stops = new[] { new StopDto { Id = 3 }, new StopDto { Id = 5 } };
            var route = new RouteDto{ Id = 10 };
            _viewModel = new EditTripViewModel(Screen.Object, _routeService.Object, route, stops);
            // when
            _viewModel.SelectedRoute = null;
            _viewModel.Trip.Service = null;
            // then
            _viewModel.SaveTrip.CanExecute(null).Should().BeFalse();
            // when
            _viewModel.SelectedRoute = new RouteDto();
            _viewModel.ServiceCalendar = new CalendarDto();
            _viewModel.SaveTrip.CanExecute(null).Should().BeTrue();
        }
        
        [Test]
        public void AddingNewTrip_Save()
        {
            // given
            var stops = new[] { new StopDto { Id = 3 }, new StopDto { Id = 5 } };
            var route = new RouteDto { Id = 10 };
            _routeService.Setup(r => r.CreateTripAsync(It.IsAny<TripDto>())).ReturnsAsync(new TripDto { Id = 42 });
            _viewModel = new EditTripViewModel(Screen.Object, _routeService.Object, route, stops);
            // when
            _viewModel.SaveTrip.ExecuteAsyncTask().Wait();
            // then
            _routeService.Verify(r => r.CreateTripAsync(It.IsAny<TripDto>()), Times.Once);
            _routeService.Verify(r => r.UpdateStopsAsync(42, It.IsAny<StopTimeDto[]>()), Times.Once);
        }

        [Test]
        public void UpdatingTrip_Save()
        {
            // given
            var stopTimes = new[] { new StopTimeDto(), new StopTimeDto() };
            var trip = new TripDto { Id = 666 };
            _routeService.Setup(r => r.GetTripStopsAsync(trip)).ReturnsAsync(stopTimes);
            _routeService.Setup(r => r.UpdateTripAsync(It.IsAny<TripDto>())).ReturnsAsync(trip);
            _viewModel = new EditTripViewModel(Screen.Object, _routeService.Object, trip);
            // when
            _viewModel.SaveTrip.ExecuteAsyncTask().Wait();
            // then
            _routeService.Verify(r => r.UpdateTripAsync(trip), Times.Once);
            _routeService.Verify(r => r.UpdateStopsAsync(666, It.IsAny<StopTimeDto[]>()), Times.Once);
        }

        [Test]
        public void UpdatingSuggestions_InvalidFilter()
        {
            // given
            var trip = new TripDto();
            _routeService.Setup(r => r.GetTripStopsAsync(trip)).ReturnsAsync(new StopTimeDto[0]);
            _viewModel = new EditTripViewModel(Screen.Object, _routeService.Object, trip);
            // when
            _viewModel.RouteReactiveFilter.ShortNameFilter = "";
            // then
            _routeService.Verify(r => r.FilterStopsAsync(It.IsAny<StopFilter>()), Times.Never);
        }

        //[Test]
        public void UpdatingSuggestions_AutomaticUpdates()
        {
            new TestScheduler().With(s =>
            {
                // given
                var trip = new TripDto();
                _routeService.Setup(r => r.GetTripStopsAsync(trip)).ReturnsAsync(new StopTimeDto[0]);
                _viewModel = new EditTripViewModel(Screen.Object, _routeService.Object, trip);
                s.AdvanceByMs(100);
                // when
                _viewModel.RouteReactiveFilter.ShortNameFilter = "test";
                // then
                s.AdvanceByMs(250);
                _routeService.Verify(r => r.FilterRoutesAsync(It.IsAny<RouteFilter>()), Times.Never);
                s.AdvanceByMs(250);
                _routeService.Verify(r => r.FilterRoutesAsync(It.IsAny<RouteFilter>()), Times.Once);
            });
        }

        [Test]
        public void NavigateToCalendar()
        {
            // given
            var navigatedToCalendar = false;
            Router.CurrentViewModel
                .Where(vm => vm is EditCalendarViewModel)
                .Subscribe(_ => navigatedToCalendar = true);
            var trip = new TripDto();
            _routeService.Setup(r => r.GetTripStopsAsync(trip)).ReturnsAsync(new StopTimeDto[0]);
            _viewModel = new EditTripViewModel(Screen.Object, _routeService.Object, trip);
            // when
            _viewModel.NavigateToCalendar.ExecuteAsyncTask().Wait();
            // then
            navigatedToCalendar.Should().BeTrue();
        }

        [Test]
        public void AddStop()
        {
            // given
            var stops = new[] { new StopDto { Id = 3 }, new StopDto { Id = 5 } };
            var route = new RouteDto { Id = 10 };
            _routeService.Setup(r => r.CreateTripAsync(It.IsAny<TripDto>())).ReturnsAsync(new TripDto { Id = 42 });
            _viewModel = new EditTripViewModel(Screen.Object, _routeService.Object, route, stops);
            // when
            _viewModel.AddStop.ExecuteAsync().Wait();
            // then
            _viewModel.StopTimes.Count.ShouldBeEquivalentTo(3);
        }

        [Test]
        public void DeleteStop()
        {
            // given
            var stops = new[] { new StopDto { Id = 3 }, new StopDto { Id = 5 } };
            var route = new RouteDto { Id = 10 };
            _routeService.Setup(r => r.CreateTripAsync(It.IsAny<TripDto>())).ReturnsAsync(new TripDto { Id = 42 });
            _viewModel = new EditTripViewModel(Screen.Object, _routeService.Object, route, stops);
            _viewModel.SelectedStopTime = _viewModel.StopTimes[0];
            // when
            _viewModel.DeleteStop.ExecuteAsync().Wait();
            // then
            _viewModel.StopTimes.Count.ShouldBeEquivalentTo(1);
            _viewModel.StopTimes.Select(s => s.StopTime.Stop.Id).Should().NotContain(3);
        }
    }
}