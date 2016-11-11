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
        private readonly Mock<IZoneUnitOfWork> _zoneUnitOfWork = new Mock<IZoneUnitOfWork>();
        private FilterZoneViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _viewModel = new FilterZoneViewModel(Screen.Object, _zoneUnitOfWork.Object);
            _zoneUnitOfWork.Setup(z => z.FilterZones(It.IsAny<string>())).Returns(new List<Zone>());
        }

        [Test]
        public void FilterZones()
        {
            // given
            _viewModel.NameFilter = "ZTM";
            // when
            _viewModel.FilterZones.ExecuteAsync().Wait();
            // then
            _zoneUnitOfWork.Verify(z => z.FilterZones(_viewModel.NameFilter), Times.Once);
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
