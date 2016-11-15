﻿using System;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.UnitsOfWork;

namespace PublicTransport.Tests.Client.ViewModels.Edit
{
    [TestFixture]
    public class EditStopViewModelTest : RoutableChildViewModelTest
    {
        private Mock<IStopUnitOfWork> _stopUnitOfWork;
        private EditStopViewModel _viewModel;
        private Stop _stop;

        [SetUp]
        public void SetUp()
        {
            _stopUnitOfWork = new Mock<IStopUnitOfWork>();
            _stop = new Stop();
        }

        [Test]
        public void SaveStop_Created()
        {
            // given
            _viewModel = new EditStopViewModel(Screen.Object, _stopUnitOfWork.Object);
            // when
            _viewModel.SaveStop.ExecuteAsyncTask().Wait();
            // then
            _stopUnitOfWork.Verify(s => s.CreateStop(It.IsAny<Stop>()), Times.Once);
        }

        [Test]
        public void SaveStop_Updated()
        {
            // given
            _viewModel = new EditStopViewModel(Screen.Object, _stopUnitOfWork.Object, _stop);
            // when
            _viewModel.SaveStop.ExecuteAsyncTask().Wait();
            // then
            _stopUnitOfWork.Verify(s => s.UpdateStop(_stop), Times.Once);
        }

        [Test]
        public void UpdateStreetSuggestions_NotUpdatedIfEmpty()
        {
            // given
            _viewModel = new EditStopViewModel(Screen.Object, _stopUnitOfWork.Object, _stop);
            // when
            _viewModel.StreetFilter.CityNameFilter = "";
            _viewModel.StreetFilter.StreetNameFilter = "";
            // then
            _stopUnitOfWork.Verify(s => s.FilterStreets(It.IsAny<IStreetFilter>()), Times.Never);
        }

        [Test]
        public void UpdateZoneSuggestions_NotUpdatedIfEmpty()
        {
            // given
            _viewModel = new EditStopViewModel(Screen.Object, _stopUnitOfWork.Object, _stop);
            // when
            _viewModel.ZoneFilter = "";
            // then
            _stopUnitOfWork.Verify(s => s.FilterStreets(It.IsAny<IStreetFilter>()), Times.Never);
        }

        [Test]
        public void UpdateParentStationSuggestions_NotUpdatedIfEmpty()
        {
            // given
            _viewModel = new EditStopViewModel(Screen.Object, _stopUnitOfWork.Object, _stop);
            // when
            _viewModel.ParentStationFilter.StopNameFilter = "";
            _viewModel.ParentStationFilter.CityNameFilter = "";
            _viewModel.ParentStationFilter.StreetNameFilter = "";
            _viewModel.ParentStationFilter.ZoneNameFilter = "";
            _viewModel.ParentStationFilter.ParentStationNameFilter = "";
            // then
            _stopUnitOfWork.Verify(s => s.FilterStreets(It.IsAny<IStreetFilter>()), Times.Never);
        }

        [Test]
        public void Close()
        {
            // given
            var navigatedBack = false;
            _viewModel = new EditStopViewModel(Screen.Object, _stopUnitOfWork.Object, _stop);
            Router.NavigateBack.Subscribe(_ => navigatedBack = true);
            // when
            _viewModel.Close.Execute(null);
            // then
            navigatedBack.Should().BeTrue();
        }
    }
}