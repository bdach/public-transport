using System;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.UnitsOfWork;
using ReactiveUI;

namespace PublicTransport.Tests.Client.ViewModels.Edit
{
    [TestFixture]
    public class EditCityViewModelTest : RoutableViewModelTest
    {
        private readonly Mock<ICityUnitOfWork> _cityUnitOfWork = new Mock<ICityUnitOfWork>();
        private EditCityViewModel _viewModel;
        private City _city;

        [SetUp]
        public void SetUp()
        {
            _city = new City();
            var mockRoutable = new Mock<IRoutableViewModel>();
            Router.NavigationStack.Add(mockRoutable.Object);
        }

        [Test]
        public void SaveCity_Created()
        {
            // given
            _viewModel = new EditCityViewModel(Screen.Object, _cityUnitOfWork.Object);
            // when
            _viewModel.SaveCity.ExecuteAsyncTask().Wait();
            // then
            _cityUnitOfWork.Verify(c => c.CreateCity(It.IsAny<City>()));
        }

        [Test]
        public void SaveCity_Updated()
        {
            // given
            _viewModel = new EditCityViewModel(Screen.Object, _cityUnitOfWork.Object, _city);
            // when
            _viewModel.SaveCity.ExecuteAsyncTask().Wait();
            // then
            _cityUnitOfWork.Verify(c => c.UpdateCity(_city));
        }

        [Test]
        public void Close()
        {
            // given
            var navigatedBack = false;
            Router.NavigateBack.Subscribe(s => navigatedBack = true);
            _viewModel = new EditCityViewModel(Screen.Object, _cityUnitOfWork.Object, _city);
            // when
            _viewModel.Close.Execute(null);
            // then
            navigatedBack.Should().BeTrue();
        }
    }
}
