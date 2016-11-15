using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using FluentAssertions;
using Microsoft.Reactive.Testing;
using Moq;
using NUnit.Framework;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.UnitsOfWork;
using ReactiveUI.Testing;

namespace PublicTransport.Tests.Client.ViewModels.Edit
{
    public class EditTripViewModelTest : RoutableChildViewModelTest
    {
        private Mock<IRouteUnitOfWork> _routeUnitOfWork;
        private EditTripViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _routeUnitOfWork = new Mock<IRouteUnitOfWork>();
        }

        [Test]
        public void AddingNewTrip()
        {
            // given
            var stops = new List<Stop> { new Stop {Id = 3}, new Stop {Id = 5} };
            var route = new Route {Id = 10};
            // when
            _viewModel = new EditTripViewModel(Screen.Object, _routeUnitOfWork.Object, route, stops);
            // then
            _viewModel.Trip.Route.ShouldBeEquivalentTo(route);
            _viewModel.Trip.RouteId.ShouldBeEquivalentTo(10);
            _viewModel.StopTimes.Select(s => s.StopTime.Stop).ShouldAllBeEquivalentTo(stops);
        }

        public void EditingExistingTrip()
        {
            // given
            var stopTimes = new List<StopTime> {new StopTime(), new StopTime()};
            var trip = new Trip();
            _routeUnitOfWork.Setup(r => r.GetTripStops(trip)).Returns(stopTimes);
            // when
            _viewModel = new EditTripViewModel(Screen.Object, _routeUnitOfWork.Object, trip);
            // then
            _viewModel.Trip.ShouldBeEquivalentTo(trip);
            _viewModel.StopTimes.Select(s => s.StopTime).ShouldAllBeEquivalentTo(stopTimes);
        }

        [Test]
        public void Save_CanSave()
        {
            // given
            var stops = new List<Stop> { new Stop { Id = 3 }, new Stop { Id = 5 } };
            var route = new Route { Id = 10 };
            _viewModel = new EditTripViewModel(Screen.Object, _routeUnitOfWork.Object, route, stops);
            // when
            _viewModel.SelectedRoute = null;
            _viewModel.Trip.Service = null;
            // then
            _viewModel.SaveTrip.CanExecute(null).Should().BeFalse();
            // when
            _viewModel.SelectedRoute = new Route();
            _viewModel.ServiceCalendar = new Calendar();
            _viewModel.SaveTrip.CanExecute(null).Should().BeTrue();
        }
        
        [Test]
        public void AddingNewTrip_Save()
        {
            // given
            var stops = new List<Stop> { new Stop { Id = 3 }, new Stop { Id = 5 } };
            var route = new Route { Id = 10 };
            _routeUnitOfWork.Setup(r => r.CreateTrip(It.IsAny<Trip>())).Returns(new Trip { Id = 42 });
            _viewModel = new EditTripViewModel(Screen.Object, _routeUnitOfWork.Object, route, stops);
            // when
            _viewModel.SaveTrip.ExecuteAsyncTask().Wait();
            // then
            _routeUnitOfWork.Verify(r => r.CreateTrip(It.IsAny<Trip>()), Times.Once);
            _routeUnitOfWork.Verify(r => r.UpdateStops(42, It.IsAny<List<StopTime>>()), Times.Once);
        }

        [Test]
        public void UpdatingTrip_Save()
        {
            // given
            var stopTimes = new List<StopTime> { new StopTime(), new StopTime() };
            var trip = new Trip { Id = 666 };
            _routeUnitOfWork.Setup(r => r.GetTripStops(trip)).Returns(stopTimes);
            _routeUnitOfWork.Setup(r => r.UpdateTrip(It.IsAny<Trip>())).Returns(trip);
            _viewModel = new EditTripViewModel(Screen.Object, _routeUnitOfWork.Object, trip);
            // when
            _viewModel.SaveTrip.ExecuteAsyncTask().Wait();
            // then
            _routeUnitOfWork.Verify(r => r.UpdateTrip(trip), Times.Once);
            _routeUnitOfWork.Verify(r => r.UpdateStops(666, It.IsAny<List<StopTime>>()), Times.Once);
        }

        [Test]
        public void UpdatingSuggestions_InvalidFilter()
        {
            // given
            var trip = new Trip();
            _routeUnitOfWork.Setup(r => r.GetTripStops(trip)).Returns(new List<StopTime>());
            _viewModel = new EditTripViewModel(Screen.Object, _routeUnitOfWork.Object, trip);
            // when
            _viewModel.RouteFilter.ShortNameFilter = "";
            // then
            _routeUnitOfWork.Verify(r => r.FilterStops(It.IsAny<IStopFilter>()), Times.Never);
        }

        //[Test]
        public void UpdatingSuggestions_AutomaticUpdates()
        {
            new TestScheduler().With(s =>
            {
                // given
                var trip = new Trip();
                _routeUnitOfWork.Setup(r => r.GetTripStops(trip)).Returns(new List<StopTime>());
                _viewModel = new EditTripViewModel(Screen.Object, _routeUnitOfWork.Object, trip);
                s.AdvanceByMs(100);
                // when
                _viewModel.RouteFilter.ShortNameFilter = "test";
                // then
                s.AdvanceByMs(250);
                _routeUnitOfWork.Verify(r => r.FilterRoutes(It.IsAny<IRouteFilter>()), Times.Never);
                s.AdvanceByMs(250);
                _routeUnitOfWork.Verify(r => r.FilterRoutes(It.IsAny<IRouteFilter>()), Times.Once);
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
            var trip = new Trip();
            _routeUnitOfWork.Setup(r => r.GetTripStops(trip)).Returns(new List<StopTime>());
            _viewModel = new EditTripViewModel(Screen.Object, _routeUnitOfWork.Object, trip);
            // when
            _viewModel.NavigateToCalendar.ExecuteAsyncTask().Wait();
            // then
            navigatedToCalendar.Should().BeTrue();
        }

        [Test]
        public void AddStop()
        {
            // given
            var stops = new List<Stop> { new Stop { Id = 3 }, new Stop { Id = 5 } };
            var route = new Route { Id = 10 };
            _routeUnitOfWork.Setup(r => r.CreateTrip(It.IsAny<Trip>())).Returns(new Trip { Id = 42 });
            _viewModel = new EditTripViewModel(Screen.Object, _routeUnitOfWork.Object, route, stops);
            // when
            _viewModel.AddStop.ExecuteAsync().Wait();
            // then
            _viewModel.StopTimes.Count.ShouldBeEquivalentTo(3);
        }

        [Test]
        public void DeleteStop()
        {
            // given
            var stops = new List<Stop> { new Stop { Id = 3 }, new Stop { Id = 5 } };
            var route = new Route { Id = 10 };
            _routeUnitOfWork.Setup(r => r.CreateTrip(It.IsAny<Trip>())).Returns(new Trip { Id = 42 });
            _viewModel = new EditTripViewModel(Screen.Object, _routeUnitOfWork.Object, route, stops);
            _viewModel.SelectedStopTime = _viewModel.StopTimes[0];
            // when
            _viewModel.DeleteStop.ExecuteAsync().Wait();
            // then
            _viewModel.StopTimes.Count.ShouldBeEquivalentTo(1);
            _viewModel.StopTimes.Select(s => s.StopTime.StopId).Should().NotContain(3);
        }
    }
}