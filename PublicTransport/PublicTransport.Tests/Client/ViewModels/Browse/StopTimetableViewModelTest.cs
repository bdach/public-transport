using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Client.Services.Stops;
using PublicTransport.Client.ViewModels.Browse;
using PublicTransport.Services.DataTransfer;

namespace PublicTransport.Tests.Client.ViewModels.Browse
{
    [TestFixture]
    public class StopTimetableViewModelTest : RoutableChildViewModelTest
    {
        private Mock<IStopService> _stopService;
        private StopTimetableViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _stopService = new Mock<IStopService>();
            _viewModel = new StopTimetableViewModel(Screen.Object, _stopService.Object, new StopDto {Id = 10});
        }

        [Test]
        public void Close()
        {
            // given
            var navigatedBack = false;
            Router.NavigateBack.Subscribe(_ => navigatedBack = true);
            // when
            _viewModel.Close.ExecuteAsyncTask().Wait();
            // then
            navigatedBack.Should().BeTrue();
        }

        [Test]
        public void GetTimetable()
        {
            // given
            var timetable = new Dictionary<RouteDto, StopTimeDto[]> {{new RouteDto {Id = 22}, new StopTimeDto[0]}};
            _stopService.Setup(service => service.GetStopTimetableAsync(It.IsAny<int>())).ReturnsAsync(timetable);
            // when
            _viewModel.GetTimetable.ExecuteAsyncTask().Wait();
            // then
            _stopService.Verify(service => service.GetStopTimetableAsync(10));
            _viewModel.Routes.Count.ShouldBeEquivalentTo(1);
        }

        [Test]
        public void ShowStopTimes()
        {
            // given
            _viewModel.Timetable = new Dictionary<RouteDto, StopTimeDto[]>
            {
                {new RouteDto(), new[] { new StopTimeDto(), new StopTimeDto() }}
            };
            // when
            _viewModel.SelectedRoute = _viewModel.Timetable.Keys.First();
            // then
            _viewModel.StopTimes.Count.ShouldBeEquivalentTo(2);
        }
    }
}