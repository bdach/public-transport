﻿using System;
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
    public class FilterAgencyViewModelTest : RoutableViewModelTest
    {
        private Mock<IAgencyUnitOfWork> _agencyUnitOfWork;
        private FilterAgencyViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _agencyUnitOfWork = new Mock<IAgencyUnitOfWork>();
            _viewModel = new FilterAgencyViewModel(Screen.Object, _agencyUnitOfWork.Object);
        }

        [Test]
        public void FilterAgencies()
        {
            // given
            _viewModel.AgencyFilter.AgencyNameFilter = "test";
            _agencyUnitOfWork.Setup(a => a.FilterAgencies(It.IsAny<IAgencyFilter>())).Returns(new List<Agency> {new Agency()});
            // when
            var task = _viewModel.FilterAgencies.ExecuteAsyncTask();
            task.Wait();
            // then
            _agencyUnitOfWork.Verify(c => c.FilterAgencies(_viewModel.AgencyFilter), Times.Once);
            _viewModel.Agencies.Count.ShouldBeEquivalentTo(1);
        }

        [Test]
        public void AutoFilterAgencies_InvalidFilter()
        {
            // given
            // when
            _viewModel.AgencyFilter.AgencyNameFilter = "";
            _viewModel.AgencyFilter.CityNameFilter = "";
            _viewModel.AgencyFilter.StreetNameFilter = "";
            // then
            _agencyUnitOfWork.Verify(c => c.FilterAgencies(It.IsAny<IAgencyFilter>()), Times.Never);
        }

        [Test]
        public void DeleteAgency()
        {
            // given
            var agency = new Agency();
            // when
            _viewModel.SelectedAgency = agency;
            _viewModel.DeleteAgency.Execute(null);
            // then
            _agencyUnitOfWork.Verify(c => c.DeleteAgency(agency), Times.Once);
        }

        [Test]
        public void AddAgency()
        {
            // given
            _viewModel.SelectedAgency = new Agency();
            var navigatedToEdit = false;
            Router.Navigate
                .Where(vm => vm is EditAgencyViewModel)
                .Subscribe(_ => navigatedToEdit = true);
            // when
            _viewModel.AddAgency.Execute(null);
            // then
            navigatedToEdit.Should().BeTrue();
            var editViewModel = Router.GetCurrentViewModel() as EditAgencyViewModel;
            editViewModel.Agency.Should().NotBe(_viewModel.SelectedAgency);
        }

        [Test]
        public void EditAgency()
        {
            // given
            _viewModel.SelectedAgency = new Agency();
            var navigatedToEdit = false;
            Router.Navigate
                .Where(vm => vm is EditAgencyViewModel)
                .Subscribe(_ => navigatedToEdit = true);
            // when
            _viewModel.EditAgency.Execute(null);
            // then
            navigatedToEdit.Should().BeTrue();
            var editViewModel = Router.GetCurrentViewModel() as EditAgencyViewModel;
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