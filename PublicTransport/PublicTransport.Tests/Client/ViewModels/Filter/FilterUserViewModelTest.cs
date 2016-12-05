using NUnit.Framework;
using System;
using System.Reactive.Linq;
using FluentAssertions;
using Moq;
using PublicTransport.Client.DataTransfer;
using PublicTransport.Client.Services.Users;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Client.ViewModels.Filter;
using PublicTransport.Domain.Enums;
using PublicTransport.Services.DataTransfer;
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
            _userService.Setup(u => u.FilterUsersAsync(It.IsAny<UserFilter>())).ReturnsAsync(new[] { new UserDto() });
            // when
            _viewModel.FilterUsers.ExecuteAsync().Wait();
            // then
            _userService.Verify(u => u.FilterUsersAsync(It.IsAny<UserFilter>()), Times.Once);
            _viewModel.Users.Count.ShouldBeEquivalentTo(1);
        }

        [Test]
        public void FilterUsers_InvalidFilter()
        {
            // given
            // when
            _viewModel.UserReactiveFilter.UserNameFilter = "";
            _viewModel.UserReactiveFilter.RoleTypeFilter = null;
            // then
            _userService.Verify(u => u.FilterUsersAsync(It.IsAny<UserFilter>()), Times.Never);
        }

        [Test]
        public void DeleteUser()
        {
            // given
            var user = new UserDto();
            // when
            _viewModel.SelectedUser = user;
            _viewModel.DeleteUser.ExecuteAsync().Wait();
            // then
            _userService.Verify(u => u.DeleteUserAsync(user), Times.Once);
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
            _viewModel.SelectedUser = new UserDto();
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
            _viewModel.SelectedUser = new UserDto();
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
            _viewModel.UserReactiveFilter = new UserReactiveFilter { RoleTypeFilter = RoleType.Administrator };
            // when
            _viewModel.ClearRoleTypeChoice.Execute(null);
            // then
            _viewModel.UserReactiveFilter.RoleTypeFilter.Should().BeNull();
        }
    }
}
