using YourRest.Infrastructure.Core.DbContexts;

namespace YourRest.Producer.Infrastructure.Seeds;

public class DatabaseSeeder
{
    private readonly CountrySeeder _countrySeeder;
    private readonly RegionSeeder _regionSeeder;
    private readonly CitySeeder _citySeeder;
    private readonly RoomTypeSeeder _roomTypeSeeder;
    private readonly AccommodationTypeSeeder _accommodationTypeSeeder;
    private readonly RoomFacilitySeeder _roomFacilitySeeder;
    private readonly AccommodationFacilitySeeder _accommodationFacilitySeeder;

    public DatabaseSeeder(SharedDbContext context)
    {
        _countrySeeder = new CountrySeeder(context);
        _regionSeeder = new RegionSeeder(context);
        _citySeeder = new CitySeeder(context);
        _roomTypeSeeder = new RoomTypeSeeder(context);
        _accommodationTypeSeeder = new AccommodationTypeSeeder(context);
        _roomFacilitySeeder = new RoomFacilitySeeder(context);
        _accommodationFacilitySeeder = new AccommodationFacilitySeeder(context);
    }

    public void Seed()
    {
        _countrySeeder.Seed();
        _regionSeeder.Seed();
        _citySeeder.Seed();
        _roomTypeSeeder.Seed();
        _accommodationTypeSeeder.Seed();
        _roomFacilitySeeder.Seed();
        _accommodationFacilitySeeder.Seed();
    }
}
