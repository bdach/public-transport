using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Models;
using PublicTransport.Client.Providers;
using PublicTransport.Client.ViewModels;
using PublicTransport.Domain.Enums;
using PublicTransport.Services.DataTransfer;
using ReactiveUI;

namespace PublicTransport.Tests.Client.ViewModels
{
    public class ShellViewModelTest : RoutableChildViewModelTest
    {
        private Mock<IDetailViewModelFactory> _viewModelFactory;
        private ShellViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _viewModelFactory = new Mock<IDetailViewModelFactory>();
            _viewModel = new ShellViewModel(Screen.Object, _viewModelFactory.Object);
        }

        [Test]
        public void StartUpViewShouldBeLoginView()
        {
            // given
            // when
            // then
            Router.GetCurrentViewModel().Should().BeOfType<LoginViewModel>();
        }

        [Test]
        public void DisplayPlaceholderUponLogin()
        {
            // given
            // when
            _viewModel.LoginViewModel.UserInfo = new UserInfo("tst", new List<RoleType>());
            // then
            Router.GetCurrentViewModel().Should().BeOfType<PlaceholderViewModel>();
        }

        [Test]
        public void DisplayLoginScreenUponLogout()
        {
            // given
            // when
            _viewModel.MenuViewModel.LogOut.ExecuteAsyncTask().Wait();
            // then
            Router.GetCurrentViewModel().Should().BeOfType<LoginViewModel>();
        }

        [Test]
        public void SwitchView()
        {
            // given
            var detailViewModel = new Mock<IDetailViewModel>();
            _viewModelFactory.Setup(vmf => vmf.GetViewModel(It.IsAny<IScreen>(), MenuOption.Stop))
                .Returns(detailViewModel.Object);
            // when
            _viewModel.MenuViewModel.SelectedOption = new MenuItemViewModel(new MenuItem("test", MenuOption.Stop));
            // then
            _viewModelFactory.Verify(vmf => vmf.GetViewModel(Screen.Object, MenuOption.Stop), Times.Once);
            Router.GetCurrentViewModel().Should().Be(detailViewModel.Object);
        }

        [Test]
        public void DoNotSwitchViewIfUrlsMatch()
        {
            // given
            var currentViewModel = new Mock<IDetailViewModel>();
            currentViewModel.Setup(s => s.UrlPathSegment).Returns("Stop");
            Router.NavigationStack.Add(currentViewModel.Object);
            var detailViewModel = new Mock<IDetailViewModel>();
            _viewModelFactory.Setup(vmf => vmf.GetViewModel(It.IsAny<IScreen>(), MenuOption.Stop))
                .Returns(detailViewModel.Object);
            // when
            _viewModel.MenuViewModel.SelectedOption = new MenuItemViewModel(new MenuItem("test", MenuOption.Stop));
            // then
            _viewModelFactory.Verify(vmf => vmf.GetViewModel(Screen.Object, MenuOption.Stop), Times.Never);
            Router.GetCurrentViewModel().Should().Be(currentViewModel.Object);
        }

        [Test]
        public void UpdateMenuUponExternalSwitch()
        {
            // given
            var item = new MenuItemViewModel(new MenuItem("example", MenuOption.Fare));
            _viewModel.MenuViewModel.Menu.Add(item);
            var newViewModel = new Mock<IDetailViewModel>();
            newViewModel.Setup(nvm => nvm.AssociatedMenuOption).Returns(MenuOption.Fare);
            // when
            Router.Navigate.ExecuteAsyncTask(newViewModel.Object).Wait();
            // then
            _viewModel.MenuViewModel.SelectedOption.ShouldBeEquivalentTo(item);
        }
    }
}