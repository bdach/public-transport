using System;
using System.Reactive.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Client.Services.Streets;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Client.ViewModels.Filter;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.DataTransfer.Filters;
using ReactiveUI;

namespace PublicTransport.Tests.Client.ViewModels.Filter
{
    [TestFixture]
    public class FilterStreetViewModelTest : RoutableViewModelTest
    {
        private Mock<IStreetService> _streetService;
        private FilterStreetViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _streetService = new Mock<IStreetService>();
            _viewModel = new FilterStreetViewModel(Screen.Object, _streetService.Object);
        }

        [Test]
        public void FilterStreets()
        {
            // given
            _streetService.Setup(s => s.FilterStreetsAsync(It.IsAny<StreetFilter>())).ReturnsAsync(new[] { new StreetDto() });
            // when
            _viewModel.FilterStreets.ExecuteAsync().Wait();
            // then
            _streetService.Verify(s => s.FilterStreetsAsync(It.IsAny<StreetFilter>()), Times.Once);
            _viewModel.Streets.Count.ShouldBeEquivalentTo(1);
        }

        [Test]
        public void FilterStreets_InvalidFilter()
        {
            // given
            // when
            _viewModel.StreetReactiveFilter.CityNameFilter = "";
            _viewModel.StreetReactiveFilter.StreetNameFilter = "";
            // then
            _streetService.Verify(s => s.FilterStreetsAsync(It.IsAny<StreetFilter>()), Times.Never);
        }

        [Test]
        public void DeleteStreet()
        {
            // given
            var street = new StreetDto();
            // when
            _viewModel.SelectedStreet = street;
            _viewModel.DeleteStreet.ExecuteAsync().Wait();
            // then
            _streetService.Verify(s => s.DeleteStreetAsync(street), Times.Once);
        }

        [Test]
        public void AddStreet()
        {
            // given
            var navigatedToEdit = false;
            _viewModel.SelectedStreet = new StreetDto();
            Router.Navigate
                .Where(vm => vm is EditStreetViewModel)
                .Subscribe(_ => navigatedToEdit = true);
            // when
            _viewModel.AddStreet.Execute(null);
            // then
            navigatedToEdit.Should().BeTrue();
            var editStreetViewModel = Router.GetCurrentViewModel() as EditStreetViewModel;
            editStreetViewModel.Should().NotBeNull();
            editStreetViewModel.Street.Should().NotBe(_viewModel.SelectedStreet);
        }

        [Test]
        public void EditStreet()
        {
            // given
            var navigatedToEdit = false;
            _viewModel.SelectedStreet = new StreetDto();
            Router.Navigate
                .Where(vm => vm is EditStreetViewModel)
                .Subscribe(_ => navigatedToEdit = true);
            // when
            _viewModel.AddStreet.Execute(null);
            // then
            navigatedToEdit.Should().BeTrue();
            var editStreetViewModel = Router.GetCurrentViewModel() as EditStreetViewModel;
            editStreetViewModel.Should().NotBeNull();
            editStreetViewModel.Street.ShouldBeEquivalentTo(_viewModel.SelectedStreet);
        }

        [Test]
        public void EditDeleteStreet_CannotExecuteIfNoStreetSelected()
        {
            // given
            // when
            _viewModel.SelectedStreet = null;
            // then
            _viewModel.EditStreet.CanExecute(null).Should().BeFalse();
            _viewModel.DeleteStreet.CanExecute(null).Should().BeFalse();
        }
    }
}
