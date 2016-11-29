using System;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Domain.Entities;
using PublicTransport.Services;

namespace PublicTransport.Tests.Client.ViewModels.Edit
{
    [TestFixture]
    public class EditZoneViewModelTest : RoutableChildViewModelTest
    {
        private Mock<IZoneService> _zoneService;
        private EditZoneViewModel _viewModel;
        private Zone _zone;

        [SetUp]
        public void SetUp()
        {
            _zoneService = new Mock<IZoneService>();
            _zone = new Zone();
        }

        [Test]
        public void SaveZone_Created()
        {
            // given
            _viewModel = new EditZoneViewModel(Screen.Object, _zoneService.Object);
            // when
            _viewModel.SaveZone.ExecuteAsyncTask().Wait();
            // then
            _zoneService.Verify(z => z.CreateZone(It.IsAny<Zone>()), Times.Once);
        }

        [Test]
        public void SaveZone_Updated()
        {
            // given
            _viewModel = new EditZoneViewModel(Screen.Object, _zoneService.Object, _zone);
            // when
            _viewModel.SaveZone.ExecuteAsyncTask().Wait();
            // then
            _zoneService.Verify(z => z.UpdateZone(_zone), Times.Once);
        }

        [Test]
        public void Close()
        {
            // given
            var navigatedBack = false;
            _viewModel = new EditZoneViewModel(Screen.Object, _zoneService.Object, _zone);
            Router.NavigateBack.Subscribe(_ => navigatedBack = true);
            // when
            _viewModel.Close.Execute(null);
            // then
            navigatedBack.Should().BeTrue();
        }
    }
}