using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
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
        private Mock<IStreetUnitOfWork> _streetUnitOfWork;
        private FilterStreetViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _streetUnitOfWork = new Mock<IStreetUnitOfWork>();
            _viewModel = new FilterStreetViewModel(Screen.Object, _streetUnitOfWork.Object);
        }

        [Test]
        public void FilterStreets()
        {
            // given
            _streetUnitOfWork.Setup(s => s.FilterStreets(It.IsAny<IStreetFilter>())).Returns(new List<Street> { new Street() });
            // when
            _viewModel.FilterStreets.ExecuteAsync().Wait();
            // then
            _streetUnitOfWork.Verify(s => s.FilterStreets(_viewModel.StreetFilter), Times.Once);
            _viewModel.Streets.Count.ShouldBeEquivalentTo(1);
        }

        [Test]
        public void FilterStreets_InvalidFilter()
        {
            // given
            // when
            _viewModel.StreetFilter.CityNameFilter = "";
            _viewModel.StreetFilter.StreetNameFilter = "";
            // then
            _streetUnitOfWork.Verify(s => s.FilterStreets(It.IsAny<IStreetFilter>()), Times.Never);
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
        public void AddStreet()
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
            editStreetViewModel.Street.Should().NotBe(_viewModel.SelectedStreet);
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
