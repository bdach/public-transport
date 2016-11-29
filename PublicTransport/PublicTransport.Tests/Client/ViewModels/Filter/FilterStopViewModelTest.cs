using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Client.ViewModels.Filter;
using PublicTransport.Domain.Entities;
using PublicTransport.Services;
using PublicTransport.Services.DataTransfer.Filters;
using ReactiveUI;

namespace PublicTransport.Tests.Client.ViewModels.Filter
{
    [TestFixture]
    public class FilterStopViewModelTest : RoutableViewModelTest
    {
        private Mock<IStopService> _stopService;
        private FilterStopViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _stopService = new Mock<IStopService>();
            _viewModel = new FilterStopViewModel(Screen.Object, _stopService.Object);
        }

        [Test]
        public void FilterStops()
        {
            // given
            _stopService.Setup(s => s.FilterStops(It.IsAny<IStopFilter>())).Returns(new List<Stop> { new Stop() });
            // when
            _viewModel.FilterStops.ExecuteAsync().Wait();
            // then
            _stopService.Verify(s => s.FilterStops(_viewModel.StopFilter), Times.Once);
            _viewModel.Stops.Count.ShouldBeEquivalentTo(1);
        }

        [Test]
        public void FilterStops_InvalidFilter()
        {
            // given
            // when
            _viewModel.StopFilter.StopNameFilter = "";
            _viewModel.StopFilter.CityNameFilter = "";
            _viewModel.StopFilter.StreetNameFilter = "";
            _viewModel.StopFilter.ZoneNameFilter = "";
            _viewModel.StopFilter.ParentStationNameFilter = "";
            // then
            _stopService.Verify(s => s.FilterStops(It.IsAny<IStopFilter>()), Times.Never);
        }

        [Test]
        public void DeleteStop()
        {
            // given
            var stop = new Stop();
            // when
            _viewModel.SelectedStop = stop;
            _viewModel.DeleteStop.ExecuteAsync().Wait();
            // then
            _stopService.Verify(s => s.DeleteStop(stop), Times.Once);
        }

        [Test]
        public void AddStop()
        {
            // given
            var navigatedToEdit = false;
            _viewModel.SelectedStop = new Stop();
            Router.Navigate
                .Where(vm => vm is EditStopViewModel)
                .Subscribe(_ => navigatedToEdit = true);
            // when
            _viewModel.AddStop.Execute(null);
            // then
            navigatedToEdit.Should().BeTrue();
            var editStopViewModel = Router.GetCurrentViewModel() as EditStopViewModel;
            editStopViewModel.Should().NotBeNull();
            editStopViewModel.Stop.Should().NotBe(_viewModel.SelectedStop);
        }

        [Test]
        public void EditStop()
        {
            // given
            var navigatedToEdit = false;
            _viewModel.SelectedStop = new Stop();
            Router.Navigate
                .Where(vm => vm is EditStopViewModel)
                .Subscribe(_ => navigatedToEdit = true);
            // when
            _viewModel.AddStop.Execute(null);
            // then
            navigatedToEdit.Should().BeTrue();
            var editStopViewModel = Router.GetCurrentViewModel() as EditStopViewModel;
            editStopViewModel.Should().NotBeNull();
            editStopViewModel.Stop.ShouldBeEquivalentTo(_viewModel.SelectedStop);
        }

        [Test]
        public void EditDeleteStop_CannotExecuteIfNoStopSelected()
        {
            // given
            // when
            _viewModel.SelectedStop = null;
            // then
            _viewModel.EditStop.CanExecute(null).Should().BeFalse();
            _viewModel.DeleteStop.CanExecute(null).Should().BeFalse();
        }
    }
}
