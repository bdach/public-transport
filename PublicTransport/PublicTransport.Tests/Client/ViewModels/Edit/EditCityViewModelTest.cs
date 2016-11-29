using System;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Client.Services.CityService;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Services.DataTransfer;

namespace PublicTransport.Tests.Client.ViewModels.Edit
{
    [TestFixture]
    public class EditCityViewModelTest : RoutableChildViewModelTest
    {
        private Mock<ICityService> _cityService;
        private EditCityViewModel _viewModel;
        private CityDto _city;

        [SetUp]
        public void SetUp()
        {
            _cityService = new Mock<ICityService>();
            _city = new CityDto();
        }

        [Test]
        public void SaveCity_Created()
        {
            // given
            _viewModel = new EditCityViewModel(Screen.Object, _cityService.Object);
            // when
            _viewModel.SaveCity.ExecuteAsyncTask().Wait();
            // then
            _cityService.Verify(c => c.CreateCityAsync(It.IsAny<CityDto>()), Times.Once);
        }

        [Test]
        public void SaveCity_Updated()
        {
            // given
            _viewModel = new EditCityViewModel(Screen.Object, _cityService.Object, _city);
            // when
            _viewModel.SaveCity.ExecuteAsyncTask().Wait();
            // then
            _cityService.Verify(c => c.UpdateCityAsync(_city), Times.Once);
        }

        [Test]
        public void Close()
        {
            // given
            var navigatedBack = false;
            _viewModel = new EditCityViewModel(Screen.Object, _cityService.Object, _city);
            Router.NavigateBack.Subscribe(_ => navigatedBack = true);
            // when
            _viewModel.Close.Execute(null);
            // then
            navigatedBack.Should().BeTrue();
        }
    }
}
