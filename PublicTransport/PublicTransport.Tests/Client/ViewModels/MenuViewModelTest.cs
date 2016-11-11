using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using PublicTransport.Client.Models;
using PublicTransport.Client.ViewModels;
using PublicTransport.Domain.Enums;
using PublicTransport.Services.DataTransfer;

namespace PublicTransport.Tests.Client.ViewModels
{
    [TestFixture]
    public class MenuViewModelTest
    {
        private MenuViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _viewModel = new MenuViewModel();
        }

        [Test]
        public void PopulateMenuOnLogin_NonAdmin()
        {
            // given
            var roles = new List<RoleType> {RoleType.Employee};
            var userInfo = new UserInfo("user", roles);
            // when
            _viewModel.UserInfo = userInfo;
            // then
            _viewModel.Menu.Should().NotBeEmpty();
            _viewModel.Menu.Should().NotContain(s => s.Item.Option == MenuOption.User);
        }

        [Test]
        public void PopulateMenuOnLogin_Admin()
        {
            // given
            var roles = new List<RoleType> { RoleType.Employee, RoleType.Administrator };
            var userInfo = new UserInfo("root", roles);
            // when
            _viewModel.UserInfo = userInfo;
            // then
            _viewModel.Menu.Should().NotBeEmpty();
            _viewModel.Menu.Should().Contain(s => s.Item.Option == MenuOption.User);
        }

        [Test]
        public void Logout()
        {
            // given
            _viewModel.Menu.Add(new MenuItemViewModel(new MenuItem("test", MenuOption.City)));
            // when
            _viewModel.LogOut.Execute(null);
            // then
            _viewModel.UserInfo.Should().BeNull();
            _viewModel.Menu.Should().BeEmpty();
        }
    }
}
