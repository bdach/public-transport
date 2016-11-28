using FluentAssertions;
using NUnit.Framework;
using PublicTransport.Domain.Entities;
using PublicTransport.Services;
using PublicTransport.Services.Exceptions;

namespace PublicTransport.Tests.Services
{
    [TestFixture]
    public class CityRepositoryTest : RepositoryTest
    {
        private CityRepository _cityRepository;

        [SetUp]
        public void ServiceSetUp()
        {
            _cityRepository = new CityRepository(DbContext);
        }

        [Test]
        public void CreateTest()
        {
            // given
            var city = new City {Name = "Kraków"};
            // when
            _cityRepository.Create(city);
            // then
            DbContext.Cities.Should().Contain(c => c.Name == "Kraków");
        }

        [Test]
        public void ReadTest()
        {
            // given
            // when
            var city = _cityRepository.Read(1);
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
            _cityRepository.Update(city);
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
            _cityRepository.Delete(city);
            // then
            DbContext.Cities.Should().NotContain(c => c.Id == 4);
        }

        [Test]
        public void DeleteTest_NonexistentEntry()
        {
            // given
            var city = new City {Id = 1000};
            // then expect
            _cityRepository.Invoking(cs => cs.Delete(city))
                .ShouldThrow<EntryNotFoundException>();
        }

        [Test]
        public void GetCitiesContainingStringTest()
        {
            // given
            // when
            var cities = _cityRepository.GetCitiesContainingString("ice");
            // then
            cities.Count.ShouldBeEquivalentTo(3); // Gliwice, Katowice, Ząbkowice
        }
    }
}