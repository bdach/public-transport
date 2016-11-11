using FluentAssertions;
using Microsoft.Reactive.Testing;
using NUnit.Framework;
using PublicTransport.Client.ViewModels;
using ReactiveUI.Testing;

namespace PublicTransport.Tests.Client.ViewModels
{
    [TestFixture]
    public class NotificationViewModelTest
    {
        [Test]
        public void ShowNotification()
        {
            new TestScheduler().With(s =>
            {
                // given
                var notificationViewModel = new NotificationViewModel();
                // when
                notificationViewModel.IsVisible = true;
                // then
                s.AdvanceByMs(5000);
                notificationViewModel.IsVisible.Should().BeTrue();
                s.AdvanceByMs(50000);
                notificationViewModel.IsVisible.Should().BeFalse();
            });
        }

        [Test]
        public void ManualNotificationClosing()
        {
            new TestScheduler().With(s =>
            {
                // given
                var notificationViewModel = new NotificationViewModel();
                // when
                notificationViewModel.IsVisible = true;
                // then
                s.AdvanceByMs(5000);
                notificationViewModel.Close.Execute(null);
                s.AdvanceByMs(1);
                notificationViewModel.IsVisible.Should().BeFalse();
            });
        }
    }
}