using System;
using System.Reactive.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Client.DataTransfer;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Client.ViewModels.Filter;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.UnitsOfWork;
using ReactiveUI;

namespace PublicTransport.Tests.Client.ViewModels.Filter
{
    [TestFixture]
    public class FilterStopViewModelTest
    {
        private readonly Mock<IScreen> _screen = new Mock<IScreen>();
        private readonly RoutingState _router = new RoutingState();
        private readonly Mock<IStopUnitOfWork> _stopUnitOfWork = new Mock<IStopUnitOfWork>();
        private FilterStopViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _screen.Setup(s => s.Router).Returns(_router);
            _viewModel = new FilterStopViewModel(_screen.Object, _stopUnitOfWork.Object);
        }

        [Test]
        public void FilterStops()
        {
            // given
            _viewModel.StopFilter = new StopFilter();
            // when
            _viewModel.FilterStops.Execute(null);
            // then
            _stopUnitOfWork.Verify(s => s.FilterStops(_viewModel.StopFilter), Times.Once);
        }

        [Test]
        public void DeleteStop()
        {
            // given
            var stop = new Stop();
            // when
            _viewModel.SelectedStop = stop;
            _viewModel.DeleteStop.Execute(null);
            // then
            _stopUnitOfWork.Verify(s => s.DeleteStop(stop), Times.Once);
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

        [Test]
        public void AddStop()
        {
            // given
            var navigatedToEdit = false;
            _router.Navigate
                .Where(vm => vm is EditStopViewModel)
                .Subscribe(_ => navigatedToEdit = true);
            // when
            _viewModel.AddStop.Execute(null);
            // then
            navigatedToEdit.Should().BeTrue();
        }

        [Test]
        public void EditStop()
        {
            // given
            var navigatedToEdit = false;
            _viewModel.SelectedStop = new Stop();
            _router.Navigate
                .Where(vm => vm is EditStopViewModel)
                .Subscribe(_ => navigatedToEdit = true);
            // when
            _viewModel.AddStop.Execute(null);
            // then
            navigatedToEdit.Should().BeTrue();
            var editStopViewModel = _router.GetCurrentViewModel() as EditStopViewModel;
            editStopViewModel.Should().NotBeNull();
            editStopViewModel.Stop.ShouldBeEquivalentTo(_viewModel.SelectedStop);
        }
    }
}
