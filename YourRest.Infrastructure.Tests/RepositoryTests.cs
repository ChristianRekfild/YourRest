using YourRest.Infrastructure.Tests.Fixtures;
using YourRest.Infrastructure.DbContexts;
using YourRest.Infrastructure.Repositories;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;

namespace YourRest.Infrastructure.Tests
{

    [Collection("Database")]
    public class RepositoryTest
    {
        private SharedDbContext _testDbContext;
        private readonly IRepository<Country, int> _countryRepository;

    public RepositoryTest(DatabaseFixture databaseFixture)
    {
            _testDbContext = databaseFixture.DbContext;
            _countryRepository = new CountryRepository(_testDbContext);
    }

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