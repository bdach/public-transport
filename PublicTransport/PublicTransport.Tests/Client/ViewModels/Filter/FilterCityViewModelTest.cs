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
    public class FilterCityViewModelTest : RoutableViewModelTest
    {
        private readonly Mock<ICityUnitOfWork> _cityUnitOfWork = new Mock<ICityUnitOfWork>();
        private FilterCityViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _viewModel = new FilterCityViewModel(Screen.Object, _cityUnitOfWork.Object);
            _cityUnitOfWork.Setup(c => c.FilterCities(It.IsAny<string>())).Returns(new List<City>());
        }
        
        [Test]
        public void FilterCities()
        {
            // given
            _viewModel.NameFilter = "Warszawa";
            // when
            var task = _viewModel.FilterCities.ExecuteAsync();
            task.Wait();
            // then
            _cityUnitOfWork.Verify(c => c.FilterCities(_viewModel.NameFilter), Times.Once);
        }

        [Test]
        public void DeleteCity()
        {
            // given
            var city = new City();
            // when
            _viewModel.SelectedCity = city;
            var task = _viewModel.DeleteCity.ExecuteAsync();
            task.Wait();
            // then
            _cityUnitOfWork.Verify(c => c.DeleteCity(city), Times.Once);
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

        [Test]
        public void AddCity()
        {
            // given
            var navigatedToEdit = false;
            Router.Navigate
                .Where(vm => vm is EditCityViewModel)
                .Subscribe(_ => navigatedToEdit = true);
            // when
            _viewModel.AddCity.Execute(null);
            // then
            navigatedToEdit.Should().BeTrue();
        }

        [Test]
        public void EditCity()
        {
            // given
            var navigatedToEdit = false;
            _viewModel.SelectedCity = new City();
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
    }
}
