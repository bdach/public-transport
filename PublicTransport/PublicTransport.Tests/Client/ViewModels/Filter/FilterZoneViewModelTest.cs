using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Client.ViewModels.Filter;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.UnitsOfWork;
using ReactiveUI;

namespace PublicTransport.Tests.Client.ViewModels.Filter
{
    [TestFixture]
    public class FilterZoneViewModelTest : RoutableViewModelTest
    {
        private Mock<IZoneUnitOfWork> _zoneUnitOfWork;
        private FilterZoneViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _zoneUnitOfWork = new Mock<IZoneUnitOfWork>();
            _viewModel = new FilterZoneViewModel(Screen.Object, _zoneUnitOfWork.Object);
        }

        [Test]
        public void FilterZones()
        {
            // given
            _zoneUnitOfWork.Setup(z => z.FilterZones(It.IsAny<string>())).Returns(new List<Zone> { new Zone() });
            // when
            _viewModel.FilterZones.ExecuteAsync().Wait();
            // then
            _zoneUnitOfWork.Verify(z => z.FilterZones(_viewModel.NameFilter), Times.Once);
            _viewModel.Zones.Count.ShouldBeEquivalentTo(1);
        }

        [Test]
        public void FilterZones_InvalidFilter()
        {
            // given
            // when
            _viewModel.NameFilter = "";
            // then
            _zoneUnitOfWork.Verify(z => z.FilterZones(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void DeleteZone()
        {
            // given
            var zone = new Zone();
            // when
            _viewModel.SelectedZone = zone;
            _viewModel.DeleteZone.ExecuteAsync().Wait();
            // then
            _zoneUnitOfWork.Verify(z => z.DeleteZone(zone), Times.Once);
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

        [Test]
        public void AddZone()
        {
            // given
            var navigatedToEdit = false;
            Router.Navigate
                .Where(vm => vm is EditZoneViewModel)
                .Subscribe(_ => navigatedToEdit = true);
            // when
            _viewModel.AddZone.Execute(null);
            // then
            navigatedToEdit.Should().BeTrue();
        }

        [Test]
        public void EditZone()
        {
            // given
            var navigatedToEdit = false;
            _viewModel.SelectedZone = new Zone();
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
    }
}
