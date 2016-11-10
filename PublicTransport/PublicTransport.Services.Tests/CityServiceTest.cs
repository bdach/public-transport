using FluentAssertions;
using NUnit.Framework;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Services.Tests
{
    [TestFixture]
    public class CityServiceTest : ServiceTest
    {
        private CityService _cityService;

        [SetUp]
        public void ServiceSetUp()
        {
            _cityService = new CityService(DbContext);
        }

        [Test]
        public void CreateTest()
        {
            // given
            var city = new City {Name = "Kraków"};
            // when
            _cityService.Create(city);
            // then
            DbContext.Cities.Should().Contain(c => c.Name == "Kraków");
        }

        [Test]
        public void ReadTest()
        {
            // given
            // when
            var city = _cityService.Read(1);
            // then
            city.Should().NotBeNull();
            city.Name.Should().BeEquivalentTo("Warszawa");
        }

        [Test]
        public void UpdateTest()
        {
            // given
            var city = DbContext.Cities.Find(2);
            city.Name = "Kielce";
            // when
            _cityService.Update(city);
            // then
            DbContext.Cities.Should().Contain(c => c.Name == "Kielce");
            DbContext.Cities.Should().NotContain(c => c.Name == "Zawiercie");
        }

        [Test]
        public void DeleteTest()
        {
            // given
            var city = DbContext.Cities.Find(4);
            // when
            _cityService.Delete(city);
            // then
            DbContext.Cities.Should().NotContain(c => c.Id == 4);
        }

        [Test]
        public void DeleteTest_NonexistentEntry()
        {
            // given
            var city = new City {Id = 1000};
            // expect exception when
            Assert.Throws<EntryNotFoundException>(() => _cityService.Delete(city));
        }

        [Test]
        public void GetCitiesContainingStringTest()
        {
            // given
            // when
            var cities = _cityService.GetCitiesContainingString("ice");
            // then
            cities.Count.ShouldBeEquivalentTo(3); // Gliwice, Katowice, Ząbkowice
        }
    }
}