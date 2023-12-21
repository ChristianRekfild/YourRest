using YourRest.Producer.Infrastructure.Tests.Fixtures;
using YourRest.Producer.Infrastructure.Repositories;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;

namespace YourRest.Producer.Infrastructure.Tests.RepositoryTests
{

    [Collection(nameof(SingletonApiTest))]
    public class CountryRepositoryTests
    {
        private readonly IRepository<Country, int> _countryRepository;

        private readonly SingletonApiTest fixture;
        public CountryRepositoryTests(SingletonApiTest fixture)
        {
            this.fixture = fixture;
            _countryRepository = new CountryRepository(fixture.DbContext);
        }

        // public CountryRepositoryTests(DatabaseFixture databaseFixture)
        // {
        //     if (databaseFixture.DbContext != null)
        //     {
        //         _testDbContext = databaseFixture.DbContext;
        //         _countryRepository = new CountryRepository(_testDbContext);
        //     }
        //     else
        //     {
        //         throw new NullReferenceException("DatabaseFixture.DbContext is null");
        //     }
        // }

        [Fact]
        public async Task TestWithChangeProperty()
        {
            await _countryRepository.AddAsync(new Country { Name = "First Country" });
            var fc = await _countryRepository.GetAllAsync();
            int id = 0;
            string name = "Second Country";
            foreach (var c in fc)
            {
                id = c.Id;
                c.Name = name;
                await _countryRepository.UpdateAsync(c);
            }
            var updatedCountry = await _countryRepository.GetAsync(id);
            //var countries = testDbContext.Countries;
            
            Assert.Equal(name, updatedCountry?.Name);
        }

        [Fact]
        public async Task TestWithReplaceEntity()
        {
            await _countryRepository.AddAsync(new Country { Name = "First Country" });
            var fc = await _countryRepository.GetAllAsync();
            int id = 0;
            string name = "Third Country";
            foreach (var c in fc)
            {
                id = c.Id;                
                var thirdCountry = new Country { Id = c.Id, Name = name };
                await _countryRepository.UpdateAsync(thirdCountry);
            }
            var updatedCountry = await _countryRepository.GetAsync(id);
            //var countries = testDbContext.Countries;
            
            Assert.Equal(name, updatedCountry?.Name);
        }
    }
}