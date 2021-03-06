﻿using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Domain.Entities;
using PublicTransport.Domain.Enums;
using PublicTransport.Services;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Repositories;

namespace PublicTransport.Tests.Services
{
    public class UserRepositoryTest : RepositoryTest
    {
        private UserRepository _userRepository;
        private Mock<IPasswordService> _passwordService;
        private UserFilter _userFilter;

        [SetUp]
        public void ServiceSetUp()
        {
            _passwordService = new Mock<IPasswordService>();
            _passwordService.Setup(ps => ps.GenerateHash(It.IsAny<string>())).Returns("hashed");
            _userRepository = new UserRepository(DbContext, _passwordService.Object);
            _userFilter = new UserFilter();
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
            _userRepository.Create(user);
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
            _userRepository.Update(user);
            // then
            _passwordService.Verify(ps => ps.GenerateHash("new"));
            var updatedUser = DbContext.Users.Find(2);
            updatedUser.Roles.Should().ContainSingle(s => s.Name == RoleType.Administrator);
        }

        [Test]
        public void FilterUsersTest_ByUserName()
        {
            // given
            _userFilter.UserNameFilter = "o";
            // when
            var users = _userRepository.FilterUsers(_userFilter);
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
            _userFilter.UserNameFilter = "";
            _userFilter.RoleTypeFilter = type;
            // when
            var users = _userRepository.FilterUsers(_userFilter);
            // then
            users.Should().ContainSingle(u => u.UserName == "root");
        }

        [Test]
        public void GetFavouriteRoutesByUserName()
        {
            // given
            // when
            var favourites = _userRepository.GetFavouriteRoutesByUserName("root");
            // then
            favourites.Count.ShouldBeEquivalentTo(2);
            favourites.Should().ContainSingle(r => r.ShortName == "E-1");
            favourites.Should().ContainSingle(r => r.ShortName == "101");
        }

        [Test]
        public void GetFavouriteStopsByUserName()
        {
            // given
            // when
            var favourites = _userRepository.GetFavouriteStopsByUserName("root");
            // then
            favourites.Count.ShouldBeEquivalentTo(2);
            favourites.Should().ContainSingle(s => s.Id == 1);
            favourites.Should().ContainSingle(s => s.Id == 2);
        }

        [Test]
        public void UpdateFavouriteRoutes()
        {
            // given
            const string userName = "root";
            var changes = new Dictionary<int, bool>
            {
                { 1, false },
                { 3, true }
            };
            // when
            _userRepository.UpdateFavouriteRoutes(changes, userName);
            // then
            var user = DbContext.Users.First(u => u.UserName == userName);
            user.FavouriteRoutes.Should().NotContain(s => s.Id == 1);
            user.FavouriteRoutes.Should().ContainSingle(s => s.Id == 3);
        }

        [Test]
        public void UpdateFavouriteStops()
        {
            // given
            const string userName = "root";
            var changes = new Dictionary<int, bool>
            {
                { 1, false },
                { 4, true }
            };
            // when
            _userRepository.UpdateFavouriteStops(changes, userName);
            // then
            var user = DbContext.Users.First(u => u.UserName == userName);
            user.FavouriteStops.Should().NotContain(s => s.Id == 1);
            user.FavouriteStops.Should().ContainSingle(s => s.Id == 4);
        }
    }
}