using System;
using System.Reactive.Linq;
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
    public class EditRouteViewModelTest : RoutableChildViewModelTest
    {
        private Mock<IRouteService> _routeService;
        private EditRouteViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _routeService = new Mock<IRouteService>();
        }

        [Test]
        public void SaveRoute_CanSave()
        {
            // given
            _viewModel = new EditRouteViewModel(Screen.Object, _routeService.Object);
            // then
            _viewModel.SaveRoute.CanExecute(null).Should().BeFalse();
            _viewModel.SelectedAgency = new Agency();
            _viewModel.SaveRoute.CanExecute(null).Should().BeTrue();
        }

        [Test]
        public void SaveRoute_Create()
        {
            // given
            _viewModel = new EditRouteViewModel(Screen.Object, _routeService.Object);
            // when
            _viewModel.SaveRoute.ExecuteAsync().Wait();
            // then
            _routeService.Verify(r => r.CreateRoute(It.IsAny<Route>()), Times.Once);
        }

        [Test]
        public void SaveRoute_Update()
        {
            // given
            var route = new Route();
            _viewModel = new EditRouteViewModel(Screen.Object, _routeService.Object, route);
            // when
            _viewModel.SaveRoute.ExecuteAsync().Wait();
            // then
            _routeService.Verify(r => r.UpdateRoute(route), Times.Once);
        }

        [Test]
        public void UpdateSuggestions_InvalidFilter()
        {
            // given
            _viewModel = new EditRouteViewModel(Screen.Object, _routeService.Object);
            // when
            _viewModel.AgencyReactiveFilter.AgencyNameFilter = "";
            // then
            _routeService.Verify(r => r.FilterAgencies(It.IsAny<AgencyFilter>()), Times.Never);
        }

        //[Test]
        public void UpdateSuggestions()
        {
            new TestScheduler().With(s =>
            {
                // given
                s.AdvanceByMs(100);
                _viewModel = new EditRouteViewModel(Screen.Object, _routeService.Object);
                // when
                _viewModel.AgencyReactiveFilter.AgencyNameFilter = "test";
                // then
                s.AdvanceByMs(250);
                _routeService.Verify(r => r.FilterAgencies(It.IsAny<AgencyFilter>()), Times.Never);
                s.AdvanceByMs(250);
                _routeService.Verify(r => r.FilterAgencies(It.IsAny<AgencyFilter>()), Times.Once);
            });
        }

        [Test]
        public void Close()
        {
            // given
            var navigatedBack = false;
            Router.NavigateBack.Subscribe(_ => navigatedBack = true);
            _viewModel = new EditRouteViewModel(Screen.Object, _routeService.Object);
            // when
            _viewModel.Close.ExecuteAsyncTask().Wait();
            // then
            navigatedBack.Should().BeTrue();
        }
    }
}