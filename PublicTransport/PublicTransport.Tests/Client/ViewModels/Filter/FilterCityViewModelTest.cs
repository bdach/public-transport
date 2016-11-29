using System;
using System.Reactive.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Client.Services.CityService;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Client.ViewModels.Filter;
using PublicTransport.Services.DataTransfer;
using ReactiveUI;

namespace PublicTransport.Tests.Client.ViewModels.Filter
{
    [TestFixture]
    public class FilterCityViewModelTest : RoutableViewModelTest
    {
        private Mock<ICityService> _cityUnitOfWork;
        private FilterCityViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _cityUnitOfWork = new Mock<ICityService>();
            _viewModel = new FilterCityViewModel(Screen.Object, _cityUnitOfWork.Object);
        }

        [Test]
        public void FilterCities()
        {
            // given
            _cityUnitOfWork.Setup(c => c.FilterCitiesAsync(It.IsAny<string>())).ReturnsAsync(new[] { new CityDto() });
            // when
            _viewModel.FilterCities.ExecuteAsync().Wait();
            // then
            _cityUnitOfWork.Verify(c => c.FilterCitiesAsync(_viewModel.NameFilter), Times.Once);
            _viewModel.Cities.Count.ShouldBeEquivalentTo(1);
        }

        [Test]
        public void FilterCities_InvalidFilter()
        {
            // given
            // when
            _viewModel.NameFilter = "";
            // then
            _cityUnitOfWork.Verify(c => c.FilterCitiesAsync(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void DeleteCity()
        {
            // given
            var city = new CityDto();
            // when
            _viewModel.SelectedCity = city;
            _viewModel.DeleteCity.ExecuteAsync().Wait();
            // then
            _cityUnitOfWork.Verify(c => c.DeleteCityAsync(city), Times.Once);
        }

        [Test]
        public void AddCity()
        {
            // given
            var navigatedToEdit = false;
            _viewModel.SelectedCity = new CityDto();
            Router.Navigate
                .Where(vm => vm is EditCityViewModel)
                .Subscribe(_ => navigatedToEdit = true);
            // when
            _viewModel.AddCity.Execute(null);
            // then
            navigatedToEdit.Should().BeTrue();
            var editCityViewModel = Router.GetCurrentViewModel() as EditCityViewModel;
            editCityViewModel.Should().NotBeNull();
            editCityViewModel.City.Should().NotBe(_viewModel.SelectedCity);
        }

        [Test]
        public void EditCity()
        {
            // given
            var navigatedToEdit = false;
            _viewModel.SelectedCity = new CityDto();
            Router.Navigate
                .Where(vm => vm is EditCityViewModel)
                .Subscribe(_ => navigatedToEdit = true);
            // when
            _viewModel.AddCity.Execute(null);
            // then
            navigatedToEdit.Should().BeTrue();
            var editCityViewModel = Router.GetCurrentViewModel() as EditCityViewModel;
            editCityViewModel.Should().NotBeNull();
            editCityViewModel.City.ShouldBeEquivalentTo(_viewModel.SelectedCity);
        }

        [Test]
        public void EditDeleteCity_CannotExecuteIfNoCitySelected()
        {
            // given
            // when
            _viewModel.SelectedCity = null;
            // then
            _viewModel.EditCity.CanExecute(null).Should().BeFalse();
            _viewModel.DeleteCity.CanExecute(null).Should().BeFalse();
        }
    }
}
