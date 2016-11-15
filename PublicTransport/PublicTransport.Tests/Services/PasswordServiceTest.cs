using FluentAssertions;
using NUnit.Framework;
using PublicTransport.Services;

namespace PublicTransport.Tests.Services
{
    [TestFixture]
    public class PasswordServiceTest
    {
        [Test]
        public void HashIdempotenceTest()
        {
            // given
            var passwordService = new PasswordService();
            var password = "correct horse battery staple";
            // when
            var hash = passwordService.GenerateHash(password);
            // then
            passwordService.CompareWithHash(password, hash).Should().BeTrue();
        }
    }
}
