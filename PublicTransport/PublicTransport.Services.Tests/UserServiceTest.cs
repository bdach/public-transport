using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Domain.Entities;
using PublicTransport.Domain.Enums;
using PublicTransport.Services.DataTransfer.Filters;

namespace PublicTransport.Services.Tests
{
    public class UserServiceTest : ServiceTest
    {
        private UserService _userService;
        private Mock<IPasswordService> _passwordService;
        private Mock<IUserFilter> _userFilter;

        [SetUp]
        public void ServiceSetUp()
        {
            _passwordService = new Mock<IPasswordService>();
            _passwordService.Setup(ps => ps.GenerateHash(It.IsAny<string>())).Returns("hashed");
            _userService = new UserService(DbContext, _passwordService.Object);
            _userFilter = new Mock<IUserFilter>();
        }

        [Test]
        public void CreateTest()
        {
            // given
            var user = new User
            {
                UserName = "tester",
                Password = "hello",
                Roles = DbContext.Roles.ToList()
            };
            // when
            _userService.Create(user);
            // then
            _passwordService.Verify(ps => ps.GenerateHash("hello"));
            var insertedUser = DbContext.Users.FirstOrDefault(u => u.UserName == "tester");
            insertedUser.Should().NotBeNull();
            insertedUser?.Password.ShouldBeEquivalentTo("hashed");
            insertedUser?.Roles.Count.ShouldBeEquivalentTo(2);
        }

        [Test]
        public void UpdateTest()
        {
            // given
            var user = DbContext.Users.Find(2); // employee
            var adminRole = DbContext.Roles.Find(2);
            user.Password = "new";
            user.Roles.Clear();
            user.Roles.Add(adminRole);
            // when
            _userService.Update(user);
            // then
            _passwordService.Verify(ps => ps.GenerateHash("new"));
            var updatedUser = DbContext.Users.Find(2);
            updatedUser.Roles.Should().ContainSingle(s => s.Name == RoleType.Administrator);
        }

        [Test]
        public void FilterUsersTest_ByUserName()
        {
            // given
            _userFilter.Setup(uf => uf.UserNameFilter).Returns("o");
            // when
            var users = _userService.FilterUsers(_userFilter.Object);
            // then
            users.Count.ShouldBeEquivalentTo(2);
            users.Should().Contain(u => u.UserName == "root");
            users.Should().Contain(u => u.UserName == "employee");
        }

        [Test]
        public void FilterUsersTest_ByRole()
        {
            // given
            RoleType? type = RoleType.Administrator;
            _userFilter.Setup(uf => uf.UserNameFilter).Returns("");
            _userFilter.Setup(uf => uf.RoleNameFilter).Returns(type);
            // when
            var users = _userService.FilterUsers(_userFilter.Object);
            // then
            users.Should().ContainSingle(u => u.UserName == "root");
        }
    }
}