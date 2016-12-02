using System;
using FluentAssertions;
using Microsoft.Reactive.Testing;
using Moq;
using NUnit.Framework;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Domain.Entities;
using PublicTransport.Services;
using PublicTransport.Services.DataTransfer.Filters;
using ReactiveUI.Testing;

namespace PublicTransport.Tests.Client.ViewModels.Edit
{
    [TestFixture]
    public class EditFareViewModelTest : RoutableChildViewModelTest
    {
        private Mock<IFareService> _fareUnitOfWork;
        private EditFareViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _fareUnitOfWork = new Mock<IFareService>();
        }

        [Test]
        public void SaveFare_CanSave()
        {
            // given
            _viewModel = new EditFareViewModel(Screen.Object, _fareUnitOfWork.Object);
            // then
            _viewModel.SaveFare.CanExecute(null).Should().BeFalse();
            _viewModel.SelectedRoute = new Route();
            _viewModel.SaveFare.CanExecute(null).Should().BeFalse();
            _viewModel.SelectedDestinationZone = new Zone();
            _viewModel.SaveFare.CanExecute(null).Should().BeFalse();
            _viewModel.SelectedOriginZone = new Zone();
            _viewModel.SaveFare.CanExecute(null).Should().BeTrue();
        }

        [Test]
        public void SaveFare_Created()
        {
            // given
            _fareUnitOfWork.Setup(f => f.CreateFareRule(It.IsAny<FareRule>()))
                .Returns(new FareRule {Id = 4});
            _viewModel = new EditFareViewModel(Screen.Object, _fareUnitOfWork.Object);
            // when
            _viewModel.SaveFare.ExecuteAsyncTask().Wait();
            // then
            _fareUnitOfWork.Verify(f => f.CreateFareAttribute(It.IsAny<FareAttribute>()), Times.Once);
            _fareUnitOfWork.Verify(f => f.CreateFareRule(It.IsAny<FareRule>()), Times.Once);
            _viewModel.FareAttribute.FareRuleId.ShouldBeEquivalentTo(4);
        }

        [Test]
        public void SaveFare_Updated()
        {
            // given
            var fareRule = new FareRule {Id = 10};
            var fareAttribute = new FareAttribute {FareRuleId = 10, FareRule = fareRule};
            _fareUnitOfWork.Setup(f => f.UpdateFareRule(It.IsAny<FareRule>())).Returns(fareRule);
            _viewModel = new EditFareViewModel(Screen.Object, _fareUnitOfWork.Object, fareAttribute);
            // when
            _viewModel.SaveFare.ExecuteAsyncTask().Wait();
            // then
            _fareUnitOfWork.Verify(f => f.UpdateFareAttribute(fareAttribute), Times.Once);
            _fareUnitOfWork.Verify(f => f.UpdateFareRule(fareRule), Times.Once);
        }

        [Test]
        public void UpdateRouteSuggestions_NotUpdatedIfEmpty()
        {
            // given
            _viewModel = new EditFareViewModel(Screen.Object, _fareUnitOfWork.Object);
            // when
            _viewModel.RouteFilter.ShortNameFilter = "";
            // then
            _fareUnitOfWork.Verify(f => f.FilterRoutes(It.IsAny<IRouteFilter>()), Times.Never);
        }

        //[Test]
        public void UpdateRouteSuggestions_AutomaticUpdates()
        {
            new TestScheduler().With(s =>
            {
                // given
                _viewModel = new EditFareViewModel(Screen.Object, _fareUnitOfWork.Object);
                s.AdvanceByMs(100);
                // when
                _viewModel.OriginZoneFilter = "hi";
                // then
                s.AdvanceByMs(250);
                _fareUnitOfWork.Verify(f => f.FilterZones(It.IsAny<string>()), Times.Never);
                s.AdvanceByMs(250);
                _fareUnitOfWork.Verify(f => f.FilterZones(It.IsAny<string>()), Times.Once);
                // when
                _viewModel.DestinationZoneFilter = "hi";
                s.AdvanceByMs(250);
                _fareUnitOfWork.Verify(f => f.FilterZones(It.IsAny<string>()), Times.Once);
                s.AdvanceByMs(250);
                _fareUnitOfWork.Verify(f => f.FilterZones(It.IsAny<string>()), Times.Exactly(2));
            });
        }

        [Test]
        public void UpdateZoneSuggestions_NotUpdatedIfEmpty()
        {
            // given
            _viewModel = new EditFareViewModel(Screen.Object, _fareUnitOfWork.Object);
            // when
            _viewModel.OriginZoneFilter = "";
            _viewModel.DestinationZoneFilter = "";
            // then
            _fareUnitOfWork.Verify(f => f.FilterRoutes(It.IsAny<IRouteFilter>()), Times.Never);
        }

        [Test]
        public void UpdateZoneSuggestions_AutomaticUpdates()
        {
            new TestScheduler().With(s =>
            {
                // given
                _viewModel = new EditFareViewModel(Screen.Object, _fareUnitOfWork.Object);
                s.AdvanceByMs(100);
                // when
                _viewModel.DestinationZoneFilter = "hi";
                // then
                s.AdvanceByMs(250);
                _fareUnitOfWork.Verify(f => f.FilterZones(It.IsAny<string>()), Times.Never);
                s.AdvanceByMs(250);
                _fareUnitOfWork.Verify(f => f.FilterZones(It.IsAny<string>()), Times.Once);
                _viewModel.RouteFilter.ShortNameFilter = "hi";
                s.AdvanceByMs(500);
                _fareUnitOfWork.Verify(f => f.FilterZones(It.IsAny<string>()), Times.Once);
            });
        }

        [Test]
        public void Close()
        {
            // given
            var navigatedBack = false;
            Router.NavigateBack.Subscribe(_ => navigatedBack = true);
            _viewModel = new EditFareViewModel(Screen.Object, _fareUnitOfWork.Object);
            // when
            _viewModel.Close.ExecuteAsyncTask().Wait();
            // then
            navigatedBack.Should().BeTrue();
        }
    }
}