using System;
using FluentAssertions;
using Microsoft.Reactive.Testing;
using Moq;
using NUnit.Framework;
using PublicTransport.Client.Services.Fares;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.DataTransfer.Filters;
using ReactiveUI.Testing;

namespace PublicTransport.Tests.Client.ViewModels.Edit
{
    [TestFixture]
    public class EditFareViewModelTest : RoutableChildViewModelTest
    {
        private Mock<IFareService> _fareService;
        private EditFareViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _fareService = new Mock<IFareService>();
        }

        [Test]
        public void SaveFare_CanSave()
        {
            // given
            _viewModel = new EditFareViewModel(Screen.Object, _fareService.Object);
            // then
            _viewModel.SaveFare.CanExecute(null).Should().BeFalse();
            _viewModel.SelectedRoute = new RouteDto();
            _viewModel.SaveFare.CanExecute(null).Should().BeFalse();
            _viewModel.SelectedDestinationZone = new ZoneDto();
            _viewModel.SaveFare.CanExecute(null).Should().BeFalse();
            _viewModel.SelectedOriginZone = new ZoneDto();
            _viewModel.SaveFare.CanExecute(null).Should().BeTrue();
        }

        [Test]
        public void SaveFare_Created()
        {
            // given
            _fareService.Setup(f => f.CreateFareRuleAsync(It.IsAny<FareRuleDto>())).ReturnsAsync(new FareRuleDto());
            _viewModel = new EditFareViewModel(Screen.Object, _fareService.Object);
            // when
            _viewModel.SaveFare.ExecuteAsyncTask().Wait();
            // then
            _fareService.Verify(f => f.CreateFareAttributeAsync(It.IsAny<FareAttributeDto>()), Times.Once);
            _fareService.Verify(f => f.CreateFareRuleAsync(It.IsAny<FareRuleDto>()), Times.Once);
        }

        [Test]
        public void SaveFare_Updated()
        {
            // given
            var fareRule = new FareRuleDto();
            var fareAttribute = new FareAttributeDto { FareRule = fareRule };
            _fareService.Setup(f => f.UpdateFareRule(It.IsAny<FareRuleDto>())).Returns(fareRule);
            _viewModel = new EditFareViewModel(Screen.Object, _fareService.Object, fareAttribute);
            // when
            _viewModel.SaveFare.ExecuteAsyncTask().Wait();
            // then
            _fareService.Verify(f => f.UpdateFareAttributeAsync(fareAttribute), Times.Once);
            _fareService.Verify(f => f.UpdateFareRuleAsync(fareRule), Times.Once);
        }

        [Test]
        public void UpdateRouteSuggestions_NotUpdatedIfEmpty()
        {
            // given
            _viewModel = new EditFareViewModel(Screen.Object, _fareService.Object);
            // when
            _viewModel.RouteReactiveFilter.ShortNameFilter = "";
            // then
            _fareService.Verify(f => f.FilterRoutesAsync(It.IsAny<RouteFilter>()), Times.Never);
        }

        //[Test]
        public void UpdateRouteSuggestions_AutomaticUpdates()
        {
            new TestScheduler().With(s =>
            {
                // given
                _viewModel = new EditFareViewModel(Screen.Object, _fareService.Object);
                s.AdvanceByMs(100);
                // when
                _viewModel.OriginZoneFilter = "hi";
                // then
                s.AdvanceByMs(250);
                _fareService.Verify(f => f.FilterZonesAsync(It.IsAny<string>()), Times.Never);
                s.AdvanceByMs(250);
                _fareService.Verify(f => f.FilterZonesAsync(It.IsAny<string>()), Times.Once);
                // when
                _viewModel.DestinationZoneFilter = "hi";
                s.AdvanceByMs(250);
                _fareService.Verify(f => f.FilterZonesAsync(It.IsAny<string>()), Times.Once);
                s.AdvanceByMs(250);
                _fareService.Verify(f => f.FilterZonesAsync(It.IsAny<string>()), Times.Exactly(2));
            });
        }

        [Test]
        public void UpdateZoneSuggestions_NotUpdatedIfEmpty()
        {
            // given
            _viewModel = new EditFareViewModel(Screen.Object, _fareService.Object);
            // when
            _viewModel.OriginZoneFilter = "";
            _viewModel.DestinationZoneFilter = "";
            // then
            _fareService.Verify(f => f.FilterRoutesAsync(It.IsAny<RouteFilter>()), Times.Never);
        }

        [Test]
        public void UpdateZoneSuggestions_AutomaticUpdates()
        {
            new TestScheduler().With(s =>
            {
                // given
                _viewModel = new EditFareViewModel(Screen.Object, _fareService.Object);
                s.AdvanceByMs(100);
                // when
                _viewModel.DestinationZoneFilter = "hi";
                // then
                s.AdvanceByMs(250);
                _fareService.Verify(f => f.FilterZonesAsync(It.IsAny<string>()), Times.Never);
                s.AdvanceByMs(250);
                _fareService.Verify(f => f.FilterZonesAsync(It.IsAny<string>()), Times.Once);
                _viewModel.RouteReactiveFilter.ShortNameFilter = "hi";
                s.AdvanceByMs(500);
                _fareService.Verify(f => f.FilterZonesAsync(It.IsAny<string>()), Times.Once);
            });
        }

        [Test]
        public void Close()
        {
            // given
            var navigatedBack = false;
            Router.NavigateBack.Subscribe(_ => navigatedBack = true);
            _viewModel = new EditFareViewModel(Screen.Object, _fareService.Object);
            // when
            _viewModel.Close.ExecuteAsyncTask().Wait();
            // then
            navigatedBack.Should().BeTrue();
        }
    }
}