using System;
using FluentAssertions;
using NUnit.Framework;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Services.DataTransfer;

namespace PublicTransport.Tests.Client.ViewModels.Edit
{
    [TestFixture]
    public class EditCalendarViewModelTest : RoutableChildViewModelTest
    {
        private CalendarDto _calendar;
        private EditCalendarViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _calendar = new CalendarDto();
            _viewModel = new EditCalendarViewModel(Screen.Object, _calendar);
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
    }
}