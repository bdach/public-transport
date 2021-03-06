﻿using System;
using System.Reactive.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Client.Services.Zones;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Client.ViewModels.Filter;
using PublicTransport.Services.DataTransfer;
using ReactiveUI;

namespace PublicTransport.Tests.Client.ViewModels.Filter
{
    [TestFixture]
    public class FilterZoneViewModelTest : RoutableViewModelTest
    {
        private Mock<IZoneService> _zoneService;
        private FilterZoneViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _zoneService = new Mock<IZoneService>();
            _viewModel = new FilterZoneViewModel(Screen.Object, _zoneService.Object);
        }

        [Test]
        public void FilterZones()
        {
            // given
            _zoneService.Setup(z => z.FilterZonesAsync(It.IsAny<string>())).ReturnsAsync(new[] { new ZoneDto() });
            // when
            _viewModel.FilterZones.ExecuteAsync().Wait();
            // then
            _zoneService.Verify(z => z.FilterZonesAsync(_viewModel.NameFilter), Times.Once);
            _viewModel.Zones.Count.ShouldBeEquivalentTo(1);
        }

        [Test]
        public void FilterZones_InvalidFilter()
        {
            // given
            // when
            _viewModel.NameFilter = "";
            // then
            _zoneService.Verify(z => z.FilterZonesAsync(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void DeleteZone()
        {
            // given
            var zone = new ZoneDto();
            // when
            _viewModel.SelectedZone = zone;
            _viewModel.DeleteZone.ExecuteAsync().Wait();
            // then
            _zoneService.Verify(z => z.DeleteZoneAsync(zone), Times.Once);
        }

        [Test]
        public void AddZone()
        {
            // given
            var navigatedToEdit = false;
            _viewModel.SelectedZone = new ZoneDto();
            Router.Navigate
                .Where(vm => vm is EditZoneViewModel)
                .Subscribe(_ => navigatedToEdit = true);
            // when
            _viewModel.AddZone.Execute(null);
            // then
            navigatedToEdit.Should().BeTrue();
            var editZoneViewModel = Router.GetCurrentViewModel() as EditZoneViewModel;
            editZoneViewModel.Should().NotBeNull();
            editZoneViewModel.Zone.Should().NotBe(_viewModel.SelectedZone);
        }

        [Test]
        public void EditZone()
        {
            // given
            var navigatedToEdit = false;
            _viewModel.SelectedZone = new ZoneDto();
            Router.Navigate
                .Where(vm => vm is EditZoneViewModel)
                .Subscribe(_ => navigatedToEdit = true);
            // when
            _viewModel.AddZone.Execute(null);
            // then
            navigatedToEdit.Should().BeTrue();
            var editZoneViewModel = Router.GetCurrentViewModel() as EditZoneViewModel;
            editZoneViewModel.Should().NotBeNull();
            editZoneViewModel.Zone.ShouldBeEquivalentTo(_viewModel.SelectedZone);
        }

        [Test]
        public void EditDeleteZone_CannotExecuteIfNoZoneSelected()
        {
            // given
            // when
            _viewModel.SelectedZone = null;
            // then
            _viewModel.EditZone.CanExecute(null).Should().BeFalse();
            _viewModel.DeleteZone.CanExecute(null).Should().BeFalse();
        }
    }
}
