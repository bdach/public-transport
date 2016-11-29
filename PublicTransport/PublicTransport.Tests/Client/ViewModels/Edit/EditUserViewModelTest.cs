using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Domain.Entities;
using PublicTransport.Services;

namespace PublicTransport.Tests.Client.ViewModels.Edit
{
    [TestFixture]
    public class EditUserViewModelTest : RoutableChildViewModelTest
    {
        private Mock<IUserService> _userService;
        private EditUserViewModel _viewModel;
        private User _user;

        [SetUp]
        public void SetUp()
        {
            _userService = new Mock<IUserService>();
            _user = new User();
        }

        [Test]
        public void SaveUser_Created()
        {
            // given
            _viewModel = new EditUserViewModel(Screen.Object, _userService.Object);
            // when
            _viewModel.SaveUser.ExecuteAsyncTask().Wait();
            // then
            _userService.Verify(u => u.CreateUser(It.IsAny<User>()), Times.Once);
        }

        [Test]
        public void SaveUser_Updated()
        {
            // given
            _viewModel = new EditUserViewModel(Screen.Object, _userService.Object, _user);
            // when
            _viewModel.SaveUser.ExecuteAsyncTask().Wait();
            // then
            _userService.Verify(u => u.UpdateUser(_user), Times.Once);
        }

        [Test]
        public void GetRoles()
        {
            // given
            _userService.Setup(a => a.GetAllRoles()).Returns(new List<Role> { new Role() });
            _viewModel = new EditUserViewModel(Screen.Object, _userService.Object);
            // when
            _viewModel.GetRoles.ExecuteAsyncTask().Wait();
            // then
            _userService.Verify(u => u.GetAllRoles(), Times.Once);
        }

        [Test]
        public void Close()
        {
            // given
            var navigatedBack = false;
            _viewModel = new EditUserViewModel(Screen.Object, _userService.Object, _user);
            Router.NavigateBack.Subscribe(_ => navigatedBack = true);
            // when
            _viewModel.Close.Execute(null);
            // then
            navigatedBack.Should().BeTrue();
        }
    }
}
