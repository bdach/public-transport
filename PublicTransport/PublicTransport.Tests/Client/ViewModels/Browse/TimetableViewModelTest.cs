using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Client.ViewModels.Browse;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.UnitsOfWork;
using ReactiveUI;

namespace PublicTransport.Tests.Client.ViewModels.Browse
{
    [TestFixture]
    public class TimetableViewModelTest : RoutableViewModelTest
    {
        private Mock<IRouteUnitOfWork> _routeUnitOfWork;
        private TimetableViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _routeUnitOfWork = new Mock<IRouteUnitOfWork>();
            _viewModel = new TimetableViewModel(Screen.Object, _routeUnitOfWork.Object, new Route());
        }

        [Test]
        public void DeleteTrip()
        {
            // given
            var stopTime = new StopTime { Trip = new Trip() };
            // when
            _viewModel.SelectedStopTime = stopTime;
            _viewModel.DeleteTrip.ExecuteAsyncTask().Wait();
            // then
            _routeUnitOfWork.Verify(r => r.DeleteTrip(stopTime.Trip), Times.Once);
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
            _routeUnitOfWork.Setup(r => r.GetTripStops(It.IsAny<Trip>())).Returns(new List<StopTime> { new StopTime() });
            var navigatedToEdit = false;
            _viewModel.SelectedStopTime = new StopTime { Trip = new Trip() };
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
            _routeUnitOfWork.Setup(r => r.GetStopsByRouteId(It.IsAny<int>())).Returns(new List<Stop> { new Stop() });
            // when
            _viewModel.GetStops.ExecuteAsyncTask().Wait();
            // then
            _routeUnitOfWork.Verify(u => u.GetStopsByRouteId(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void UpdateStopTimes()
        {
            // given
            _routeUnitOfWork.Setup(r => r.GetRouteTimetableByStopId(It.IsAny<IStopTimeFilter>())).Returns(new List<StopTime> { new StopTime() });
            // when
            _viewModel.UpdateStopTimes.ExecuteAsyncTask().Wait();
            // then
            _routeUnitOfWork.Verify(u => u.GetRouteTimetableByStopId(It.IsAny<IStopTimeFilter>()), Times.Once);
        }
    }
}