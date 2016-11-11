using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Client.DataTransfer;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Client.ViewModels.Filter;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.UnitsOfWork;
using ReactiveUI;

namespace PublicTransport.Tests.Client.ViewModels.Filter
{
    [TestFixture]
    public class FilterStreetViewModelTest : RoutableViewModelTest
    {
        private readonly Mock<IStreetUnitOfWork> _streetUnitOfWork = new Mock<IStreetUnitOfWork>();
        private FilterStreetViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _viewModel = new FilterStreetViewModel(Screen.Object, _streetUnitOfWork.Object);
            _streetUnitOfWork.Setup(s => s.FilterCities(It.IsAny<string>())).Returns(new List<City>());
            _streetUnitOfWork.Setup(s => s.FilterStreets(It.IsAny<IStreetFilter>())).Returns(new List<Street>());
        }

        [Test]
        public void FilterStreets()
        {
            // given
            _viewModel.StreetFilter = new StreetFilter();
            // when
            _viewModel.FilterStreets.ExecuteAsync().Wait();
            // then
            _streetUnitOfWork.Verify(s => s.FilterStreets(_viewModel.StreetFilter), Times.Once);
        }

        [Test]
        public void DeleteStreet()
        {
            // given
            var street = new Street();
            // when
            _viewModel.SelectedStreet = street;
            _viewModel.DeleteStreet.ExecuteAsync().Wait();
            // then
            _streetUnitOfWork.Verify(s => s.DeleteStreet(street), Times.Once);
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

        [Test]
        public void AddStreet()
        {
            // given
            var navigatedToEdit = false;
            Router.Navigate
                .Where(vm => vm is EditStreetViewModel)
                .Subscribe(_ => navigatedToEdit = true);
            // when
            _viewModel.AddStreet.Execute(null);
            // then
            navigatedToEdit.Should().BeTrue();
        }

        [Test]
        public void EditStreet()
        {
            // given
            var navigatedToEdit = false;
            _viewModel.SelectedStreet = new Street();
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
    }
}
