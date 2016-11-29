using System;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Client.Services.Streets;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer;

namespace PublicTransport.Tests.Client.ViewModels.Edit
{
    [TestFixture]
    public class EditStreetViewModelTest : RoutableChildViewModelTest
    {
        private Mock<IStreetService> _streetService;
        private EditStreetViewModel _viewModel;
        private StreetDto _street;

        [SetUp]
        public void SetUp()
        {
            _streetService = new Mock<IStreetService>();
            _street = new StreetDto();
        }

        [Test]
        public void SaveStreet_Created()
        {
            // given
            _viewModel = new EditStreetViewModel(Screen.Object, _streetService.Object);
            // when
            _viewModel.SaveStreet.ExecuteAsyncTask().Wait();
            // then
            _streetService.Verify(s => s.CreateStreetAsync(It.IsAny<StreetDto>()), Times.Once);
        }

        [Test]
        public void SaveStreet_Updated()
        {
            // given
            _viewModel = new EditStreetViewModel(Screen.Object, _streetService.Object, _street);
            // when
            _viewModel.SaveStreet.ExecuteAsyncTask().Wait();
            // then
            _streetService.Verify(s => s.UpdateStreetAsync(_street), Times.Once);
        }

        [Test]
        public void UpdateSuggestions_NotUpdatedIfEmpty()
        {
            // given
            _viewModel = new EditStreetViewModel(Screen.Object, _streetService.Object, _street);
            // when
            _viewModel.CityName = "";
            // then
            _streetService.Verify(s => s.FilterCities(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void Close()
        {
            // given
            var navigatedBack = false;
            _viewModel = new EditStreetViewModel(Screen.Object, _streetService.Object, _street);
            Router.NavigateBack.Subscribe(_ => navigatedBack = true);
            // when
            _viewModel.Close.Execute(null);
            // then
            navigatedBack.Should().BeTrue();
        }
    }
}
