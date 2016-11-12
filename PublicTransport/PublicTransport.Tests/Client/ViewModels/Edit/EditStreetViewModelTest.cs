using System;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.UnitsOfWork;

namespace PublicTransport.Tests.Client.ViewModels.Edit
{
    [TestFixture]
    public class EditStreetViewModelTest : RoutableChildViewModelTest
    {
        private Mock<IStreetUnitOfWork> _streetUnitOfWork;
        private EditStreetViewModel _viewModel;
        private Street _street;

        [SetUp]
        public void SetUp()
        {
            _streetUnitOfWork = new Mock<IStreetUnitOfWork>();
            _street = new Street();
        }

        [Test]
        public void SaveStreet_Created()
        {
            // given
            _viewModel = new EditStreetViewModel(Screen.Object, _streetUnitOfWork.Object);
            // when
            _viewModel.SaveStreet.ExecuteAsyncTask().Wait();
            // then
            _streetUnitOfWork.Verify(s => s.CreateStreet(It.IsAny<Street>()), Times.Once);
        }

        [Test]
        public void SaveStreet_Updated()
        {
            // given
            _viewModel = new EditStreetViewModel(Screen.Object, _streetUnitOfWork.Object, _street);
            // when
            _viewModel.SaveStreet.ExecuteAsyncTask().Wait();
            // then
            _streetUnitOfWork.Verify(s => s.UpdateStreet(_street), Times.Once);
        }

        [Test]
        public void UpdateSuggestions_NotUpdatedIfEmpty()
        {
            // given
            _viewModel = new EditStreetViewModel(Screen.Object, _streetUnitOfWork.Object, _street);
            // when
            _viewModel.CityName = "";
            // then
            _streetUnitOfWork.Verify(s => s.FilterCities(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void Close()
        {
            // given
            var navigatedBack = false;
            _viewModel = new EditStreetViewModel(Screen.Object, _streetUnitOfWork.Object, _street);
            Router.NavigateBack.Subscribe(_ => navigatedBack = true);
            // when
            _viewModel.Close.Execute(null);
            // then
            navigatedBack.Should().BeTrue();
        }
    }
}
