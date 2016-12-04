using System;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Domain.Entities;
using PublicTransport.Services;
using PublicTransport.Services.DataTransfer.Filters;

namespace PublicTransport.Tests.Client.ViewModels.Edit
{
    [TestFixture]
    public class EditStopViewModelTest : RoutableChildViewModelTest
    {
        private Mock<IStopService> _stopService;
        private EditStopViewModel _viewModel;
        private Stop _stop;

        [SetUp]
        public void SetUp()
        {
            _stopService = new Mock<IStopService>();
            _stop = new Stop();
        }

        [Test]
        public void SaveStop_Created()
        {
            // given
            _viewModel = new EditStopViewModel(Screen.Object, _stopService.Object);
            // when
            _viewModel.SaveStop.ExecuteAsyncTask().Wait();
            // then
            _stopService.Verify(s => s.CreateStop(It.IsAny<Stop>()), Times.Once);
        }

        [Test]
        public void SaveStop_Updated()
        {
            // given
            _viewModel = new EditStopViewModel(Screen.Object, _stopService.Object, _stop);
            // when
            _viewModel.SaveStop.ExecuteAsyncTask().Wait();
            // then
            _stopService.Verify(s => s.UpdateStop(_stop), Times.Once);
        }

        [Test]
        public void UpdateStreetSuggestions_NotUpdatedIfEmpty()
        {
            // given
            _viewModel = new EditStopViewModel(Screen.Object, _stopService.Object, _stop);
            // when
            _viewModel.StreetReactiveFilter.CityNameFilter = "";
            _viewModel.StreetReactiveFilter.StreetNameFilter = "";
            // then
            _stopService.Verify(s => s.FilterStreets(It.IsAny<StreetFilter>()), Times.Never);
        }

        [Test]
        public void UpdateZoneSuggestions_NotUpdatedIfEmpty()
        {
            // given
            _viewModel = new EditStopViewModel(Screen.Object, _stopService.Object, _stop);
            // when
            _viewModel.ZoneFilter = "";
            // then
            _stopService.Verify(s => s.FilterStreets(It.IsAny<StreetFilter>()), Times.Never);
        }

        [Test]
        public void UpdateParentStationSuggestions_NotUpdatedIfEmpty()
        {
            // given
            _viewModel = new EditStopViewModel(Screen.Object, _stopService.Object, _stop);
            // when
            _viewModel.ParentStationReactiveFilter.StopNameFilter = "";
            _viewModel.ParentStationReactiveFilter.CityNameFilter = "";
            _viewModel.ParentStationReactiveFilter.StreetNameFilter = "";
            _viewModel.ParentStationReactiveFilter.ZoneNameFilter = "";
            _viewModel.ParentStationReactiveFilter.ParentStationNameFilter = "";
            // then
            _stopService.Verify(s => s.FilterStreets(It.IsAny<StreetFilter>()), Times.Never);
        }

        [Test]
        public void Close()
        {
            // given
            var navigatedBack = false;
            _viewModel = new EditStopViewModel(Screen.Object, _stopService.Object, _stop);
            Router.NavigateBack.Subscribe(_ => navigatedBack = true);
            // when
            _viewModel.Close.Execute(null);
            // then
            navigatedBack.Should().BeTrue();
        }
    }
}