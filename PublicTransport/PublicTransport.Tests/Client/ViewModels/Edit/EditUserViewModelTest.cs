using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.UnitsOfWork;

namespace PublicTransport.Tests.Client.ViewModels.Edit
{
    [TestFixture]
    public class EditUserViewModelTest : RoutableChildViewModelTest
    {
        private Mock<IUserUnitOfWork> _userUnitOfWork;
        private EditUserViewModel _viewModel;
        private User _user;

        [SetUp]
        public void SetUp()
        {
            _userUnitOfWork = new Mock<IUserUnitOfWork>();
            _user = new User();
        }

        [Test]
        public void SaveUser_Created()
        {
            // given
            _viewModel = new EditUserViewModel(Screen.Object, _userUnitOfWork.Object);
            // when
            _viewModel.SaveUser.ExecuteAsyncTask().Wait();
            // then
            _userUnitOfWork.Verify(u => u.CreateUser(It.IsAny<User>()), Times.Once);
        }

        [Test]
        public void SaveUser_Updated()
        {
            // given
            _viewModel = new EditUserViewModel(Screen.Object, _userUnitOfWork.Object, _user);
            // when
            _viewModel.SaveUser.ExecuteAsyncTask().Wait();
            // then
            _userUnitOfWork.Verify(u => u.UpdateUser(_user), Times.Once);
        }

        [Test]
        public void GetRoles()
        {
            // given
            _userUnitOfWork.Setup(a => a.GetAllRoles()).Returns(new List<Role> { new Role() });
            _viewModel = new EditUserViewModel(Screen.Object, _userUnitOfWork.Object);
            // when
            _viewModel.GetRoles.ExecuteAsyncTask().Wait();
            // then
            _userUnitOfWork.Verify(u => u.GetAllRoles(), Times.Once);
        }

        [Test]
        public void Close()
        {
            // given
            var navigatedBack = false;
            _viewModel = new EditUserViewModel(Screen.Object, _userUnitOfWork.Object, _user);
            Router.NavigateBack.Subscribe(_ => navigatedBack = true);
            // when
            _viewModel.Close.Execute(null);
            // then
            navigatedBack.Should().BeTrue();
        }
    }
}
