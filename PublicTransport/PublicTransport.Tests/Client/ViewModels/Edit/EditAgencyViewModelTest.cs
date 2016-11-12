using System;
using FluentAssertions;
using Microsoft.Reactive.Testing;
using Moq;
using NUnit.Framework;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.UnitsOfWork;
using ReactiveUI.Testing;

namespace PublicTransport.Tests.Client.ViewModels.Edit
{
    public class EditAgencyViewModelTest : RoutableChildViewModelTest
    {
        private Mock<IAgencyUnitOfWork> _agencyUnitOfWork;
        private EditAgencyViewModel _viewModel;
        private Agency _agency;

        [SetUp]
        public void SetUp()
        {
            _agencyUnitOfWork = new Mock<IAgencyUnitOfWork>();
            _agency = new Agency();
        }

        [Test]
        public void SaveAgency_Created()
        {
            // given
            _viewModel = new EditAgencyViewModel(Screen.Object, _agencyUnitOfWork.Object);
            // when
            _viewModel.SaveAgency.ExecuteAsyncTask().Wait();
            // then
            _agencyUnitOfWork.Verify(a => a.CreateAgency(It.IsAny<Agency>()));
        }

        [Test]
        public void SaveAgency_Updated()
        {
            // given
            _viewModel = new EditAgencyViewModel(Screen.Object, _agencyUnitOfWork.Object, _agency);
            // when
            _viewModel.SaveAgency.ExecuteAsyncTask().Wait();
            // then
            _agencyUnitOfWork.Verify(a => a.UpdateAgency(_agency));
        }

        [Test]
        public void UpdateSuggestions_NotUpdatedIfEmpty()
        {
            // given
            _viewModel = new EditAgencyViewModel(Screen.Object, _agencyUnitOfWork.Object, _agency);
            // when
            _viewModel.StreetFilter.StreetNameFilter = "";
            // then
            _agencyUnitOfWork.Verify(a => a.FilterStreets(It.IsAny<IStreetFilter>()), Times.Never);
        }

        //[Test]
        public void UpdateSuggestions_AutomaticUpdates()
        {
            new TestScheduler().With(s =>
            {
                // given
                s.AdvanceByMs(100);
                _viewModel = new EditAgencyViewModel(Screen.Object, _agencyUnitOfWork.Object, _agency);
                // when
                _viewModel.StreetFilter.StreetNameFilter = "hello";
                // then
                s.AdvanceByMs(250);
                _agencyUnitOfWork.Verify(a => a.FilterStreets(It.IsAny<IStreetFilter>()), Times.Never);
                s.AdvanceByMs(250);
                _agencyUnitOfWork.Verify(a => a.FilterStreets(It.IsAny<IStreetFilter>()), Times.Once);
            });
        }

        //[Test]
        public void UpdateSuggestions_AutomaticUpdates_NotIfSame()
        {
            new TestScheduler().With(s =>
            {
                // given
                s.AdvanceByMs(100);
                _viewModel = new EditAgencyViewModel(Screen.Object, _agencyUnitOfWork.Object, _agency);
                _viewModel.SelectedStreet = new Street {Name = "hello"};
                // when
                _viewModel.StreetFilter.StreetNameFilter = "hello";
                s.AdvanceByMs(500);
                // then
                _agencyUnitOfWork.Verify(a => a.FilterStreets(It.IsAny<IStreetFilter>()), Times.Never);
            });
        }

        [Test]
        public void Close()
        {
            // given
            var navigatedBack = false;
            _viewModel = new EditAgencyViewModel(Screen.Object, _agencyUnitOfWork.Object, _agency);
            Router.NavigateBack.Subscribe(_ => navigatedBack = true);
            // when
            _viewModel.Close.Execute(null);
            // then
            navigatedBack.Should().BeTrue();
        }
    }
}
