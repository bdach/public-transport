using System;
using System.Reactive.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Client.Services.Agencies;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Client.ViewModels.Filter;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.DataTransfer.Filters;
using ReactiveUI;

namespace PublicTransport.Tests.Client.ViewModels.Filter
{
    public class FilterAgencyViewModelTest : RoutableViewModelTest
    {
        private Mock<IAgencyService> _agencyService;
        private FilterAgencyViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _agencyService = new Mock<IAgencyService>();
            _viewModel = new FilterAgencyViewModel(Screen.Object, _agencyService.Object);
        }

        [Test]
        public void FilterAgencies()
        {
            // given
            _agencyService.Setup(a => a.FilterAgenciesAsync(It.IsAny<AgencyFilter>())).ReturnsAsync(new[] { new AgencyDto() });
            // when
            _viewModel.FilterAgencies.ExecuteAsyncTask().Wait();
            // then
            _agencyService.Verify(a => a.FilterAgencies(_viewModel.AgencyReactiveFilter.Convert()), Times.Once);
            _viewModel.Agencies.Count.ShouldBeEquivalentTo(1);
        }

        [Test]
        public void FilterAgencies_InvalidFilter()
        {
            // given
            // when
            _viewModel.AgencyReactiveFilter.AgencyNameFilter = "";
            _viewModel.AgencyReactiveFilter.CityNameFilter = "";
            _viewModel.AgencyReactiveFilter.StreetNameFilter = "";
            // then
            _agencyService.Verify(a => a.FilterAgencies(It.IsAny<AgencyFilter>()), Times.Never);
        }

        [Test]
        public void DeleteAgency()
        {
            // given
            var agency = new AgencyDto();
            // when
            _viewModel.SelectedAgency = agency;
            _viewModel.DeleteAgency.ExecuteAsyncTask().Wait();
            // then
            _agencyService.Verify(a => a.DeleteAgency(agency), Times.Once);
        }

        [Test]
        public void AddAgency()
        {
            // given
            _viewModel.SelectedAgency = new AgencyDto();
            var navigatedToEdit = false;
            Router.Navigate
                .Where(vm => vm is EditAgencyViewModel)
                .Subscribe(_ => navigatedToEdit = true);
            // when
            _viewModel.AddAgency.Execute(null);
            // then
            navigatedToEdit.Should().BeTrue();
            var editViewModel = Router.GetCurrentViewModel() as EditAgencyViewModel;
            editViewModel.Should().NotBeNull();
            editViewModel.Agency.Should().NotBe(_viewModel.SelectedAgency);
        }

        [Test]
        public void EditAgency()
        {
            // given
            _viewModel.SelectedAgency = new AgencyDto();
            var navigatedToEdit = false;
            Router.Navigate
                .Where(vm => vm is EditAgencyViewModel)
                .Subscribe(_ => navigatedToEdit = true);
            // when
            _viewModel.EditAgency.Execute(null);
            // then
            navigatedToEdit.Should().BeTrue();
            var editViewModel = Router.GetCurrentViewModel() as EditAgencyViewModel;
            editViewModel.Should().NotBeNull();
            editViewModel.Agency.ShouldBeEquivalentTo(_viewModel.SelectedAgency);
        }

        [Test]
        public void EditDeleteAgency_CannotExecuteIfNoAgencySelected()
        {
            // given
            // when
            _viewModel.SelectedAgency = null;
            // then
            _viewModel.EditAgency.CanExecute(null).Should().BeFalse();
            _viewModel.DeleteAgency.CanExecute(null).Should().BeFalse();
        }
    }
}