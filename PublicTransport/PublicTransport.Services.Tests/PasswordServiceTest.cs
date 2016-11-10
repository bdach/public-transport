using FluentAssertions;
using NUnit.Framework;

namespace PublicTransport.Services.Tests
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
