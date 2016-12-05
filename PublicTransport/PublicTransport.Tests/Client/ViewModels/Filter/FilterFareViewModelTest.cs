using System;
using System.Reactive.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Client.Services.Fares;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Client.ViewModels.Filter;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.DataTransfer.Filters;
using ReactiveUI;

namespace PublicTransport.Tests.Client.ViewModels.Filter
{
    [TestFixture]
    public class FilterFareViewModelTest : RoutableViewModelTest
    {
        private Mock<IFareService> _fareService;
        private FilterFareViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _fareService = new Mock<IFareService>();
            _viewModel = new FilterFareViewModel(Screen.Object, _fareService.Object);
        }

        [Test]
        public void FilterFares()
        {
            // given
            _fareService.Setup(f => f.FilterFaresAsync(It.IsAny<FareFilter>())).ReturnsAsync(new[] { new FareAttributeDto() });
            // when
            _viewModel.FilterFares.ExecuteAsync().Wait();
            // then
            _fareService.Verify(f => f.FilterFaresAsync(It.IsAny<FareFilter>()), Times.Once);
            _viewModel.FareAttributes.Count.ShouldBeEquivalentTo(1);
        }

        [Test]
        public void FilterFares_InvalidFilter()
        {
            // given
            // when
            _viewModel.FareReactiveFilter.RouteNameFilter = "";
            _viewModel.FareReactiveFilter.OriginZoneNameFilter = "";
            _viewModel.FareReactiveFilter.DestinationZoneNameFilter = "";
            // then
            _fareService.Verify(f => f.FilterFaresAsync(It.IsAny<FareFilter>()), Times.Never);
        }

        [Test]
        public void DeleteFare()
        {
            // given
            var fare = new FareAttributeDto();
            // when
            _viewModel.SelectedFare = fare;
            _viewModel.DeleteFare.ExecuteAsync().Wait();
            // then
            _fareService.Verify(f => f.DeleteFareAttributeAsync(fare), Times.Never);
            _fareService.Verify(f => f.DeleteFareRuleAsync(fare.FareRule), Times.Once);
        }

        [Test]
        public void AddFare()
        {
            // given
            var navigatedToEdit = false;
            _viewModel.SelectedFare = new FareAttributeDto();
            Router.Navigate
                .Where(vm => vm is EditFareViewModel)
                .Subscribe(_ => navigatedToEdit = true);
            // when
            _viewModel.AddFare.Execute(null);
            // then
            navigatedToEdit.Should().BeTrue();
            var editFareViewModel = Router.GetCurrentViewModel() as EditFareViewModel;
            editFareViewModel.Should().NotBeNull();
            editFareViewModel.FareAttribute.ShouldBeEquivalentTo(_viewModel.SelectedFare);
        }

        [Test]
        public void EditFare()
        {
            // given
            var navigatedToEdit = false;
            _viewModel.SelectedFare = new FareAttributeDto();
            Router.Navigate
                .Where(vm => vm is EditFareViewModel)
                .Subscribe(_ => navigatedToEdit = true);
            // when
            _viewModel.AddFare.Execute(null);
            // then
            navigatedToEdit.Should().BeTrue();
            var editFareViewModel = Router.GetCurrentViewModel() as EditFareViewModel;
            editFareViewModel.Should().NotBeNull();
            editFareViewModel.FareAttribute.ShouldBeEquivalentTo(_viewModel.SelectedFare);
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
    }
}
