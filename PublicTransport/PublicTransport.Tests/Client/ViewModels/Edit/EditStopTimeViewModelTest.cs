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
    [TestFixture]
    public class EditStopTimeViewModelTest
    {
        private Mock<IRouteUnitOfWork> _routeUnitOfWork;
        private EditStopTimeViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _routeUnitOfWork = new Mock<IRouteUnitOfWork>();
        }

        [Test]
        public void UpdateSuggestions_InvalidFilter()
        {
            // given
            _viewModel = new EditStopTimeViewModel(_routeUnitOfWork.Object);
            // when
            _viewModel.StopFilter.StopNameFilter = "";
            // then
            _routeUnitOfWork.Verify(r => r.FilterStops(It.IsAny<IStopFilter>()), Times.Never);
        }

        //[Test]
        public void UpdateSuggestions_AutomaticUpdates()
        {
            new TestScheduler().With(s =>
            {
                // given
                _viewModel = new EditStopTimeViewModel(_routeUnitOfWork.Object);
                s.AdvanceByMs(100);
                // when
                _viewModel.StopFilter.StopNameFilter = "test";
                // then
                s.AdvanceByMs(250);
                _routeUnitOfWork.Verify(r => r.FilterStops(It.IsAny<IStopFilter>()), Times.Never);
                s.AdvanceByMs(250);
                _routeUnitOfWork.Verify(r => r.FilterStops(It.IsAny<IStopFilter>()), Times.Once);
            });
        }

        [Test]
        public void SelectStop_IdInserted()
        {
            // given
            _viewModel = new EditStopTimeViewModel(_routeUnitOfWork.Object);
            // when
            _viewModel.SelectedStop = new Stop {Id = 10};
            // then
            _viewModel.StopTime.StopId.ShouldBeEquivalentTo(10);
        }
    }
}