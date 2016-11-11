using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Domain.Enums;
using PublicTransport.Services;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Tests.Services
{
    [TestFixture]
    public class LoginServiceTest : ServiceTest
    {
        private LoginService _loginService;
        private Mock<IPasswordService> _passwordService;

        [SetUp]
        public void ServiceSetUp()
        {
            _passwordService = new Mock<IPasswordService>();
            _loginService = new LoginService(DbContext, _passwordService.Object);
        }

        [Test]
        public void RequestLogin_CorrectCredentials()
        {
            // given
            var loginData = new LoginData("root", "test");
            _passwordService.Setup(ps => ps.CompareWithHash(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            // when
            var requestLogin = _loginService.RequestLogin(loginData);
            // then
            requestLogin.UserName.Should().BeEquivalentTo("root");
            requestLogin.UserRoles.Should().Contain(RoleType.Administrator);
            requestLogin.UserRoles.Should().Contain(RoleType.Employee);
        }

        [Test]
        public void RequestLogin_NonexistentUser()
        {
            // given
            var loginData = new LoginData("nonexistent", "test");
            _passwordService.Setup(ps => ps.CompareWithHash(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            // then expect
            _loginService.Invoking(ls => ls.RequestLogin(loginData))
                .ShouldThrow<InvalidCredentialsException>();
        }

        [Test]
        public void RequestLogin_InvalidPassword()
        {
            // given
            var loginData = new LoginData("root", "hacking");
            _passwordService.Setup(ps => ps.CompareWithHash(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            // then expect
            _loginService.Invoking(ls => ls.RequestLogin(loginData))
                .ShouldThrow<InvalidCredentialsException>();
        }
    }
}