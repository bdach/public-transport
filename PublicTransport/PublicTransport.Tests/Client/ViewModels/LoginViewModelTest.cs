using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Client.ViewModels;
using PublicTransport.Domain.Enums;
using PublicTransport.Services;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.Exceptions;
using ReactiveUI;

namespace PublicTransport.Tests.Client.ViewModels
{
    [TestFixture]
    public class LoginViewModelTest
    {
        private readonly Mock<ILoginService> _loginService = new Mock<ILoginService>();
        private readonly Mock<IScreen> _screen = new Mock<IScreen>();

        [Test]
        public void CannotSendRequestWithNoUsername()
        {
            // given
            var loginViewModel = new LoginViewModel(_screen.Object, _loginService.Object);
            // when
            loginViewModel.Username = "test";
            loginViewModel.Username = "";
            // then
            loginViewModel.SendLoginRequest.CanExecute(null).Should().BeFalse();
        }

        [Test]
        public void SuccessfulLogin()
        {
            // given
            var userInfo = new UserInfo("success", new List<RoleType> {RoleType.Administrator});
            _loginService.Setup(ls => ls.RequestLogin(It.IsAny<LoginData>())).Returns(userInfo);
            var loginViewModel = new LoginViewModel(_screen.Object, _loginService.Object);
            var userInfoLoaded = false;
            // when
            loginViewModel.WhenAnyValue(vm => vm.UserInfo, ui => ui == userInfo)
                .Subscribe(_ => userInfoLoaded = true);
            loginViewModel.Username = "hello";
            loginViewModel.SendLoginRequest.Execute(null);
            // then
            userInfoLoaded.Should().BeTrue();
        }

        [Test]
        public void FailedLogin()
        {
            // given
            _loginService.Setup(ls => ls.RequestLogin(It.IsAny<LoginData>())).Throws<InvalidCredentialsException>();
            var loginViewModel = new LoginViewModel(_screen.Object, _loginService.Object);
            var userInfoLoaded = false;
            // when
            loginViewModel.WhenAnyValue(vm => vm.UserInfo)
                .Where(ui => ui != null)
                .Subscribe(_ => userInfoLoaded = true);
            loginViewModel.Username = "hello";
            loginViewModel.SendLoginRequest.Execute(null);
            // then
            userInfoLoaded.Should().BeFalse();
        }
    }
}
