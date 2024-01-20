using AutoMapper;
using YourRest.Infrastructure.Core.Contracts.Models;
using YourRest.Infrastructure.Core.Contracts.Repositories;
using YourRest.Producer.Infrastructure.Repositories;
using YourRest.Producer.Infrastructure.Tests.Fixtures;

namespace YourRest.Producer.Infrastructure.Tests.RepositoryTests
{

    [Collection(nameof(SingletonApiTest))]
    public class CountryRepositoryTests
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        private readonly SingletonApiTest fixture;
        public CountryRepositoryTests(SingletonApiTest fixture)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ProducerInfrastructureMapper>());
            _mapper = config.CreateMapper();

            this.fixture = fixture;
            _countryRepository = new CountryRepository(fixture.DbContext, _mapper);
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
            await _countryRepository.AddAsync(new CountryDto { Name = "First Country" });
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
            await _countryRepository.AddAsync(new CountryDto { Name = "First Country" });
            var fc = await _countryRepository.GetAllAsync();
            int id = 0;
            string name = "Third Country";
            foreach (var c in fc)
            {
                id = c.Id;                
                var thirdCountry = new CountryDto { Id = c.Id, Name = name };
                await _countryRepository.UpdateAsync(thirdCountry);
            }
            var updatedCountry = await _countryRepository.GetAsync(id);
            //var countries = testDbContext.Countries;
            
            Assert.Equal(name, updatedCountry?.Name);
        }
    }
}