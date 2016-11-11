using NUnit.Framework;
using System;
using System.Reactive.Linq;
using FluentAssertions;
using Moq;
using PublicTransport.Client.DataTransfer;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Client.ViewModels.Filter;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.UnitsOfWork;
using ReactiveUI;

namespace PublicTransport.Tests.Client.ViewModels.Filter
{
    [TestFixture]
    public class FilterUserViewModelTest
    {
        private readonly Mock<IScreen> _screen = new Mock<IScreen>();
        private readonly RoutingState _router = new RoutingState();
        private readonly Mock<IUserUnitOfWork> _userUnitOfWork = new Mock<IUserUnitOfWork>();
        private FilterUserViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _screen.Setup(s => s.Router).Returns(_router);
            _viewModel = new FilterUserViewModel(_screen.Object, _userUnitOfWork.Object);
        }

        [Test]
        public void FilterUsers()
        {
            // given
            _viewModel.UserFilter = new UserFilter();
            // when
            _viewModel.FilterUsers.Execute(null);
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
            _viewModel.DeleteUser.Execute(null);
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
            _router.Navigate
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
            _router.Navigate
                .Where(vm => vm is EditUserViewModel)
                .Subscribe(_ => navigatedToEdit = true);
            // when
            _viewModel.AddUser.Execute(null);
            // then
            navigatedToEdit.Should().BeTrue();
            var editUserViewModel = _router.GetCurrentViewModel() as EditUserViewModel;
            editUserViewModel.Should().NotBeNull();
            editUserViewModel.User.ShouldBeEquivalentTo(_viewModel.SelectedUser);
        }
    }
}
