using NUnit.Framework;
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
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.UnitsOfWork;
using ReactiveUI;

namespace PublicTransport.Tests.Client.ViewModels.Filter
{
    [TestFixture]
    public class FilterUserViewModelTest : RoutableViewModelTest
    {
        private readonly Mock<IUserUnitOfWork> _userUnitOfWork = new Mock<IUserUnitOfWork>();
        private FilterUserViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _viewModel = new FilterUserViewModel(Screen.Object, _userUnitOfWork.Object);
            _userUnitOfWork.Setup(u => u.FilterUsers(It.IsAny<IUserFilter>())).Returns(new List<User>());
            _userUnitOfWork.Setup(u => u.GetAllRoles()).Returns(new List<Role>());
        }

        [Test]
        public void FilterUsers()
        {
            // given
            _viewModel.UserFilter = new UserFilter();
            // when
            _viewModel.FilterUsers.ExecuteAsync().Wait();
            // then
            _userUnitOfWork.Verify(u => u.FilterUsers(_viewModel.UserFilter), Times.Once);
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
            _userUnitOfWork.Verify(u => u.DeleteUser(user), Times.Once);
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
            Router.Navigate
                .Where(vm => vm is EditUserViewModel)
                .Subscribe(_ => navigatedToEdit = true);
            // when
            _viewModel.AddUser.Execute(null);
            // then
            navigatedToEdit.Should().BeTrue();
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
            _viewModel.UserFilter = new UserFilter { RoleNameFilter = RoleType.Administrator };
            // when
            _viewModel.ClearRoleTypeChoice.Execute(null);
            // then
            _viewModel.UserFilter.RoleNameFilter.Should().BeNull();
        }
    }
}
