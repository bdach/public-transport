﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using FluentAssertions;
using Moq;
using PublicTransport.Client.DataTransfer;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Client.ViewModels.Filter;
using PublicTransport.Domain.Entities;
using PublicTransport.Domain.Enums;
using PublicTransport.Services;
using PublicTransport.Services.DataTransfer.Filters;
using ReactiveUI;

namespace PublicTransport.Tests.Client.ViewModels.Filter
{
    [TestFixture]
    public class FilterUserViewModelTest : RoutableViewModelTest
    {
        private Mock<IUserService> _userService;
        private FilterUserViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _userService = new Mock<IUserService>();
            _viewModel = new FilterUserViewModel(Screen.Object, _userService.Object);
        }

        [Test]
        public void FilterUsers()
        {
            // given
            _userService.Setup(u => u.FilterUsers(It.IsAny<IUserFilter>())).Returns(new List<User> { new User() });
            // when
            _viewModel.FilterUsers.ExecuteAsync().Wait();
            // then
            _userService.Verify(u => u.FilterUsers(_viewModel.UserFilter), Times.Once);
            _viewModel.Users.Count.ShouldBeEquivalentTo(1);
        }

        [Test]
        public void FilterUsers_InvalidFilter()
        {
            // given
            // when
            _viewModel.UserFilter.UserNameFilter = "";
            _viewModel.UserFilter.RoleTypeFilter = null;
            // then
            _userService.Verify(u => u.FilterUsers(It.IsAny<IUserFilter>()), Times.Never);
        }

        [Test]
        public void DeleteUser()
        {
            // given
            var user = new User();
            // when
            _viewModel.SelectedUser = user;
            _viewModel.DeleteUser.ExecuteAsync().Wait();
            // then
            _userService.Verify(u => u.DeleteUser(user), Times.Once);
        }

        [Test]
        public void EditDeleteUser_CannotExecuteIfNoUserSelected()
        {
            // given
            // when
            _viewModel.SelectedUser = null;
            // then
            _viewModel.EditUser.CanExecute(null).Should().BeFalse();
            _viewModel.DeleteUser.CanExecute(null).Should().BeFalse();
        }

        [Test]
        public void AddUser()
        {
            // given
            var navigatedToEdit = false;
            _viewModel.SelectedUser = new User();
            Router.Navigate
                .Where(vm => vm is EditUserViewModel)
                .Subscribe(_ => navigatedToEdit = true);
            // when
            _viewModel.AddUser.Execute(null);
            // then
            navigatedToEdit.Should().BeTrue();
            var editUserViewModel = Router.GetCurrentViewModel() as EditUserViewModel;
            editUserViewModel.Should().NotBeNull();
            editUserViewModel.User.Should().NotBe(_viewModel.SelectedUser);
        }

        [Test]
        public void EditUser()
        {
            // given
            var navigatedToEdit = false;
            _viewModel.SelectedUser = new User();
            Router.Navigate
                .Where(vm => vm is EditUserViewModel)
                .Subscribe(_ => navigatedToEdit = true);
            // when
            _viewModel.AddUser.Execute(null);
            // then
            navigatedToEdit.Should().BeTrue();
            var editUserViewModel = Router.GetCurrentViewModel() as EditUserViewModel;
            editUserViewModel.Should().NotBeNull();
            editUserViewModel.User.ShouldBeEquivalentTo(_viewModel.SelectedUser);
        }

        [Test]
        public void ClearRoleType()
        {
            // given
            _viewModel.UserFilter = new UserFilter { RoleTypeFilter = RoleType.Administrator };
            // when
            _viewModel.ClearRoleTypeChoice.Execute(null);
            // then
            _viewModel.UserFilter.RoleTypeFilter.Should().BeNull();
        }
    }
}
