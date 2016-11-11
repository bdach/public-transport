using System;
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
    public class FilterZoneViewModelTest
    {
        private readonly Mock<IScreen> _screen = new Mock<IScreen>();
        private readonly RoutingState _router = new RoutingState();
        private readonly Mock<IZoneUnitOfWork> _zoneUnitOfWork = new Mock<IZoneUnitOfWork>();
        private FilterZoneViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _screen.Setup(s => s.Router).Returns(_router);
            _viewModel = new FilterZoneViewModel(_screen.Object, _zoneUnitOfWork.Object);
        }

        [Test]
        public void FilterZones()
        {
            // given
            _viewModel.NameFilter = "ZTM";
            // when
            _viewModel.FilterZones.Execute(null);
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
            _viewModel.DeleteZone.Execute(null);
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
            _router.Navigate
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
            _router.Navigate
                .Where(vm => vm is EditZoneViewModel)
                .Subscribe(_ => navigatedToEdit = true);
            // when
            _viewModel.AddZone.Execute(null);
            // then
            navigatedToEdit.Should().BeTrue();
            var editZoneViewModel = _router.GetCurrentViewModel() as EditZoneViewModel;
            editZoneViewModel.Should().NotBeNull();
            editZoneViewModel.Zone.ShouldBeEquivalentTo(_viewModel.SelectedZone);
        }
    }
}
