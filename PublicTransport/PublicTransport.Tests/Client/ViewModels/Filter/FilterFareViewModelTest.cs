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
    public class FilterFareViewModelTest
    {
        private readonly Mock<IScreen> _screen = new Mock<IScreen>();
        private readonly RoutingState _router = new RoutingState();
        private readonly Mock<IFareUnitOfWork> _fareUnitOfWork = new Mock<IFareUnitOfWork>();
        private FilterFareViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _screen.Setup(s => s.Router).Returns(_router);
            _viewModel = new FilterFareViewModel(_screen.Object, _fareUnitOfWork.Object);
        }

        [Test]
        public void FilterFares()
        {
            // given
            _viewModel.FareFilter = new FareFilter();
            // when
            _viewModel.FilterFares.Execute(null);
            // then
            _fareUnitOfWork.Verify(f => f.FilterFares(_viewModel.FareFilter), Times.Once);
        }

        [Test]
        public void DeleteFare()
        {
            // given
            var fare = new FareAttribute();
            // when
            _viewModel.SelectedFare = fare;
            _viewModel.DeleteFare.Execute(null);
            // then
            _fareUnitOfWork.Verify(f => f.DeleteFareAttribute(fare), Times.Never);
            _fareUnitOfWork.Verify(f => f.DeleteFareRule(fare.FareRule), Times.Once);
        }

        [Test]
        public void EditDeleteFare_CannotExecuteIfNoFareSelected()
        {
            // given
            // when
            _viewModel.SelectedFare = null;
            // then
            _viewModel.EditFare.CanExecute(null).Should().BeFalse();
            _viewModel.DeleteFare.CanExecute(null).Should().BeFalse();
        }

        [Test]
        public void AddFare()
        {
            // given
            var navigatedToEdit = false;
            _router.Navigate
                .Where(vm => vm is EditFareViewModel)
                .Subscribe(_ => navigatedToEdit = true);
            // when
            _viewModel.AddFare.Execute(null);
            // then
            navigatedToEdit.Should().BeTrue();
        }

        [Test]
        public void EditFare()
        {
            // given
            var navigatedToEdit = false;
            _viewModel.SelectedFare = new FareAttribute();
            _router.Navigate
                .Where(vm => vm is EditFareViewModel)
                .Subscribe(_ => navigatedToEdit = true);
            // when
            _viewModel.AddFare.Execute(null);
            // then
            navigatedToEdit.Should().BeTrue();
            var editFareViewModel = _router.GetCurrentViewModel() as EditFareViewModel;
            editFareViewModel.Should().NotBeNull();
            editFareViewModel.FareAttribute.ShouldBeEquivalentTo(_viewModel.SelectedFare);
        }
    }
}
