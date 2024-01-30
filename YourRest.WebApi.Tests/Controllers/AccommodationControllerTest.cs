using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using YourRest.Application.Dto;
using YourRest.Application.Dto.Models;
using YourRest.Application.Dto.ViewModels;
using YourRest.Domain.Entities;
using YourRest.WebApi.Responses;
using YourRest.WebApi.Tests.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using YourRest.Domain.Repositories;
using System.Net.Http.Headers;
using YourRest.Application.Dto.Models.Room;

namespace YourRest.WebApi.Tests.Controllers
{
    [Collection(nameof(SingletonApiTest))]
    public class AccommodationControllerTest
    {
        private readonly SingletonApiTest fixture;
        private readonly SharedResourcesFixture _sharedFixture;

        public AccommodationControllerTest(SingletonApiTest fixture)
        {
            this.fixture = fixture;
            var scope = fixture.Server.Host.Services.CreateScope();

            var tokenRepository = scope.ServiceProvider.GetRequiredService<ITokenRepository>();
            _sharedFixture = new SharedResourcesFixture(tokenRepository);
        }

        [Fact]
        public async Task GivenAccommodation_WhenApiMethodInvokedWithValidAddress_ThenReturns200Ok()
        {
            var accommodationType = new AccommodationType
            {
                Name = "Test Type"
            };
            var country = await fixture.InsertObjectIntoDatabase(new Country() { Name = "Russia" });
            var region = await fixture.InsertObjectIntoDatabase(new Region() { Name = "Московская область", CountryId = country.Id });
            var city = await fixture.InsertObjectIntoDatabase(new City { Name = "Moscow", RegionId = region.Id });
            var accommodation = await fixture.InsertObjectIntoDatabase(new Accommodation { Name = "FirstHotel", AccommodationType = accommodationType });
            var addressDto = CreateValidAddressDto(city.Id);
            
            var content = new StringContent(JsonConvert.SerializeObject(addressDto), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PostAsync($"api/operators/accommodations/{accommodation.Id}/address", content);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var responseString = await response.Content.ReadAsStringAsync();
            var createdAddress = JsonConvert.DeserializeObject<ResultDto>(responseString);
            
            Assert.True(createdAddress?.Id > 0);
            fixture.CleanDatabase();
        }

        [Fact]
        public async Task GivenAccommodationAndExistAddressInDB_WhenApiMethodInvokedWithTheSameAddress_ThenReturns200Ok()
        {

            var country = await fixture.InsertObjectIntoDatabase(new Country { Name = "Russia" });
            var region = await fixture.InsertObjectIntoDatabase(new Region { Name = "Московская область", CountryId = country.Id });
            var city = await fixture.InsertObjectIntoDatabase(new City { Name = "Moscow", RegionId = region.Id });
            var address = await fixture.InsertObjectIntoDatabase(new Address
            {
                Street = "First street",
                CityId = city.Id,
                ZipCode = "188644",
                Longitude = 100,
                Latitude = 80,
            });
            var accommodationType = new AccommodationType
            {
                Name = "Test Type"
            };
            var accommodation = await fixture.InsertObjectIntoDatabase(new Accommodation { Name = "SecondHotel", AccommodationType = accommodationType });
            var addressDto = CreateValidAddressDto(city.Id);
            addressDto.Street = "Second street";
            var content = new StringContent(JsonConvert.SerializeObject(addressDto), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PostAsync($"api/operators/accommodations/{accommodation.Id}/address", content);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var createdAddress = await response.Content.ReadFromJsonAsync<ResultDto>();

            Assert.True(createdAddress?.Id > 0);
            fixture.CleanDatabase();
        }

        [Fact]
        public async Task GivenNonexistentAccommodation_WhenAddAddressToAccommodationAsyncInvoked_ThenReturns404()
        {
            var country = await fixture.InsertObjectIntoDatabase(new Country { Name = "Russia" });
            var region = await fixture.InsertObjectIntoDatabase(new Region { Name = "Московская область", CountryId = country.Id });
            var city = await fixture.InsertObjectIntoDatabase(new City { Name = "Moscow", RegionId = region.Id });
            var accommodationId = -1;
            var addressDto = CreateValidAddressDto(city.Id);
            addressDto.Street = "Thrid street";

            var content = new StringContent(JsonConvert.SerializeObject(addressDto), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PostAsync($"api/operators/accommodations/{accommodationId}/address", content);
           
            var errorMassage = await response.Content.ReadAsStringAsync();
            var expectedMessage = new { message = $"Accommodation with id {accommodationId} not found" };
            var expectedMessageJson = JsonConvert.SerializeObject(expectedMessage);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(errorMassage, expectedMessageJson);
            fixture.CleanDatabase();
        }

        [Fact]
        public async Task GivenNonexistentCity_WhenAddAddressToAccommodationAsyncInvoked_ThenReturns400()
        {
            var accommodationType = new AccommodationType
            {
                Name = "Test Type"
            };
            var accommodation = await fixture.InsertObjectIntoDatabase(new Accommodation  { Name = "ThirdHotel", AccommodationType = accommodationType });
            var addressDto = new AddressDto
            {
                Street = "Fourth Street",
                ZipCode = "123456",
                Longitude = 0,
                Latitude = 0,
                CityId = 100
            };

            var content = new StringContent(JsonConvert.SerializeObject(addressDto), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PostAsync($"api/operators/accommodations/{accommodation.Id}/address", content);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var errorResponseString = await response.Content.ReadAsStringAsync();
            var expectedMessage = new { message = "City with id 100 not found" };
            var expectedMessageJson = JsonConvert.SerializeObject(expectedMessage);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(errorResponseString, expectedMessageJson);
            fixture.CleanDatabase();
        }

        [Fact]
        public async Task GivenInvalidAddress_WhenAddAddressToAccommodationAsyncInvoked_ThenReturns404()
        {
            var accommodationType = new AccommodationType
            {
                Name = "Test Type"
            };
            var accommodation = await fixture.InsertObjectIntoDatabase(new Accommodation { Name = "FourthHotel", AccommodationType = accommodationType });
            var addressDto = new AddressDto
            {
                Street = "",
                ZipCode = "",
                Longitude = 190,
                Latitude = 190,
                CityId = -1
            };
            var content = new StringContent(JsonConvert.SerializeObject(addressDto), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PostAsync($"api/operators/accommodations/{accommodation.Id}/address", content);
            var errorData = await response.Content.ReadFromJsonAsync<ErrorViewModel>();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("'Street' должно быть заполнено.", errorData?.ValidationErrors[nameof(addressDto.Street)][0]);
            Assert.Equal("'Zip Code' должно быть заполнено.", errorData?.ValidationErrors[nameof(addressDto.ZipCode)][0]);
            Assert.Equal("'Zip Code' имеет неверный формат.", errorData?.ValidationErrors[nameof(addressDto.ZipCode)][1]);
            Assert.Equal("'Latitude' должно быть в диапазоне от -90 до 90. Введенное значение: 190.", errorData?.ValidationErrors[nameof(addressDto.Latitude)][0]);
            Assert.Equal("'Longitude' должно быть в диапазоне от -180 до 180. Введенное значение: 190.", errorData?.ValidationErrors[nameof(addressDto.Longitude)][0]);
            Assert.Equal("'City Id' должно быть больше '0'.", errorData?.ValidationErrors[nameof(addressDto.CityId)][0]);
            fixture.CleanDatabase();
        }

        [Fact]
        public async Task GivenAccommodationWithAddress_WhenAddAddressToAccommodationAsyncInvoked_ThenThrows422()
        {
            var country = await fixture.InsertObjectIntoDatabase(new Country { Name = "Russia" });
            var region = await fixture.InsertObjectIntoDatabase(new Region { Name = "Московская область", CountryId = country.Id });
            var city = await fixture.InsertObjectIntoDatabase(new City { Name = "Moscow", RegionId = region.Id });
            var addressEntity = await fixture.InsertObjectIntoDatabase(new Address
            {
                Street = "Тестовая улица",
                CityId = city.Id,
                ZipCode = "188644",
                Longitude = 100,
                Latitude = 90,
            });
            var accommodationType = new AccommodationType
            {
                Name = "Test Type"
            };
            var accommodation = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "FifthHotel",
                AddressId = addressEntity.Id,
                AccommodationType = accommodationType
            });
            var addressDto = CreateValidAddressDto(city.Id);
            addressDto.Street = "Fifth street";

            var content = new StringContent(JsonConvert.SerializeObject(addressDto), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PostAsync($"api/operators/accommodations/{accommodation.Id}/address", content);

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);

            var errorResponseString = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorResponseString);

            Assert.Equal($"Address for accommodation with id {accommodation.Id} already exists", errorResponse?.Message);
            fixture.CleanDatabase();
        }
        
        [Fact]
        public async Task GivenAccommodations_WhenApiInvoked_ThenShouldReturnListOfAccommodations()
        {
            var country = await fixture.InsertObjectIntoDatabase(new Country { Name = "Russia" });
            var region = await fixture.InsertObjectIntoDatabase(new Region { Name = "Moscow region", CountryId = country.Id });
            var city = await fixture.InsertObjectIntoDatabase(new City { Name = "Moscow", RegionId = region.Id });
            var addressEntity = await fixture.InsertObjectIntoDatabase(new Address
            {
                Street = "Test Street",
                CityId = city.Id,
                ZipCode = "94105",
                Longitude = 120,
                Latitude = 75,
            });
            var accommodationType = new AccommodationType
            {
                Name = "Luxury"
            };
            
            var user = new User
            {
                FirstName = "Test",
                LastName = "Test",
                Email = "test@test.ru",
                KeyCloakId = _sharedFixture.SharedUserId
            };
            await fixture.InsertObjectIntoDatabase(user);

            var accommodation = new Accommodation
            {
                Name = "GoldenHotel",
                AddressId = addressEntity.Id,
                AccommodationType = accommodationType
            };
            var accommodationStarRating = new AccommodationStarRating
            {
                Stars = 5, 
                Accommodation = accommodation
            };
            accommodation.StarRating = accommodationStarRating;
            await fixture.InsertObjectIntoDatabase(accommodation);
            var userAcc = new UserAccommodation
            {
                User = user,
                Accommodation = accommodation
            };
            accommodation.UserAccommodations.Add(userAcc);
            await fixture.SaveChangesAsync();
            var fetchHotelsViewModel = new FetchAccommodationsViewModel
            {
                CityIds = new List<int> { city.Id },
                AccommodationTypesIds = new List<int> { accommodationType.Id }
            };
            
            var token = _sharedFixture.SharedAccessToken;

            fixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(fetchHotelsViewModel), Encoding.UTF8, "application/json");

            var response = await fixture.Client.PostAsync("api/accommodations", content);
           
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseString = await response.Content.ReadAsStringAsync();
            var accommodations = JsonConvert.DeserializeObject<List<AccommodationExtendedDto>>(responseString);
            
            Assert.True(accommodations.Count > 0);
            Assert.Contains(accommodations, dto => dto.Name == "GoldenHotel");
            Assert.Contains(accommodations, dto => dto.Stars.HasValue && dto.Stars.Value == 5);
           
            fixture.CleanDatabase();
        }
        
        [Fact]
        public async Task GivenValidAccommodationData_WhenCreateApiMethodInvoked_ThenShouldReturnCreatedResult()
        {
            var accommodationType = new AccommodationType { Name = "Luxury" };
            var accType = await fixture.InsertObjectIntoDatabase(accommodationType);
            
            var validCreateAccommodationDto = new CreateAccommodationDto
            {
                Name = "Test Accommodation",
                AccommodationTypeId = accType.Id,
                Stars = 4,
                Description = "A test description"
            };
            
            var user = new User
            {
                FirstName = "Test",
                LastName = "Test",
                Email = "test@test.ru",
                KeyCloakId = _sharedFixture.SharedUserId
            };
            await fixture.InsertObjectIntoDatabase(user);

            var token = _sharedFixture.SharedAccessToken;

            fixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var content = new StringContent(JsonConvert.SerializeObject(validCreateAccommodationDto), Encoding.UTF8, "application/json");

            var response = await fixture.Client.PostAsync("api/accommodation", content);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var createdAccommodation = JsonConvert.DeserializeObject<AccommodationDto>(await response.Content.ReadAsStringAsync());
            Assert.NotNull(createdAccommodation);
            Assert.Equal(validCreateAccommodationDto.Name, createdAccommodation.Name);
            Assert.Equal(validCreateAccommodationDto.AccommodationTypeId, createdAccommodation.AccommodationType.Id);
            Assert.Equal(validCreateAccommodationDto.Stars, createdAccommodation.Stars);
            Assert.Equal(validCreateAccommodationDto.Description, createdAccommodation.Description);
            
            var retrievedAccommodation = await fixture.GetAccommodationById(createdAccommodation.Id);
            Assert.NotNull(retrievedAccommodation);

            var userLinkedToAccommodation = retrievedAccommodation.UserAccommodations
                .Any(ua => ua.AccommodationId == createdAccommodation.Id);

            Assert.True(userLinkedToAccommodation, "The user is not linked to the accommodation.");
            
            fixture.CleanDatabase();
        }
        
        [Fact]
        public async Task GivenAccommodationDataWithoutOptionalFields_WhenCreateApiMethodInvoked_ThenShouldReturnCreatedResult()
        {
            var accommodationType = new AccommodationType { Name = "Luxury" };
            var accType = await fixture.InsertObjectIntoDatabase(accommodationType);
            var validCreateAccommodationDto = new CreateAccommodationDto
            {
                Name = "Test Accommodation",
                AccommodationTypeId = accType.Id
            };
            
            var token = _sharedFixture.SharedAccessToken;

            fixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var content = new StringContent(JsonConvert.SerializeObject(validCreateAccommodationDto), Encoding.UTF8, "application/json");

            var response = await fixture.Client.PostAsync("api/accommodation", content);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var createdAccommodation = JsonConvert.DeserializeObject<AccommodationDto>(await response.Content.ReadAsStringAsync());
            Assert.NotNull(createdAccommodation);
            Assert.Equal(createdAccommodation.Name, validCreateAccommodationDto.Name);
            Assert.Equal(createdAccommodation.AccommodationType.Id, validCreateAccommodationDto.AccommodationTypeId);
            Assert.Null(validCreateAccommodationDto.Stars);
            Assert.Null(validCreateAccommodationDto.Description);
            
            var retrievedAccommodation = await fixture.GetAccommodationById(createdAccommodation.Id);
            Assert.NotNull(retrievedAccommodation);

            var userLinkedToAccommodation = retrievedAccommodation.UserAccommodations
                .Any(ua => ua.AccommodationId == createdAccommodation.Id);

            Assert.True(userLinkedToAccommodation, "The user is not linked to the accommodation.");
        }
        [Fact]
        public async Task GetAll_ReturnsAccommodations_WhenAccommodationsInDB()
        {
            var accommodationType = new AccommodationType
            {
                Name = "Test Type"
            };
            var accType = await fixture.InsertObjectIntoDatabase(accommodationType);

            var country = await fixture.InsertObjectIntoDatabase(new Country() { Name = "Russia" });
            var region = await fixture.InsertObjectIntoDatabase(new Region() { Name = "Московская область", CountryId = country.Id });
            var city = await fixture.InsertObjectIntoDatabase(new City { Name = "Moscow", RegionId = region.Id });

            var address1 = await fixture.InsertObjectIntoDatabase(new Domain.Entities.Address
            {
                Street = "Test Street",
                ZipCode = "123456",
                Longitude = 0,
                Latitude = 0,
                CityId = city.Id
            });
            var address2 = await fixture.InsertObjectIntoDatabase(new Domain.Entities.Address
            {
                Street = "Test Street1",
                ZipCode = "123458",
                Longitude = 0,
                Latitude = 0,
                CityId = city.Id
            });

            var accommodation1 = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "FirstHotel",
                //AccommodationTypeId = accType.Id,
                AccommodationType = accType,
                AddressId = address1.Id,
                Description = "testAccommodationDescription1",
                StarRating = new AccommodationStarRating { Stars = 4 },
                Rooms = new List<Room>
                {
                    new Room { Name = "330", Capacity = 21, SquareInMeter = 230, RoomType = new RoomType { Name = "Test 1" } },
                    new Room { Name = "340", Capacity = 21, SquareInMeter = 10, RoomType = new RoomType { Name = "Test 2" } }
                }
            });
            var accommodation2 = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "SecondHotel",
                //AccommodationTypeId = accType.Id,
                AccommodationType = accType,
                AddressId = address2.Id,
                Description = "testAccommodationDescription2",
                StarRating = new AccommodationStarRating { Stars = 4 },
                Rooms = new List<Room>
                {
                    new Room { Name = "310", Capacity = 1, SquareInMeter = 20, RoomType = new RoomType { Name = "Test 3" } },
                    new Room { Name = "320", Capacity = 1, SquareInMeter = 20, RoomType = new RoomType { Name = "Test 4" } }
                }
            });

            var response = await fixture.Client.GetAsync($"api/accommodations");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseString = await response.Content.ReadAsStringAsync();
            var accommodationsReturn = JsonConvert.DeserializeObject<List<AccommodationExtendedDto>>(responseString);

            Assert.Equal(accommodation1.Id, accommodationsReturn.First().Id);
            Assert.Equal(accommodation2.Id, accommodationsReturn.Last().Id);
            Assert.Equal(accommodation1.Address.Id, accommodationsReturn.First().Address.Id);
            Assert.Equal(accommodation2.Address.Id, accommodationsReturn.Last().Address.Id);
            Assert.Equal(accommodation1.Name, accommodationsReturn.First().Name);
            Assert.Equal(accommodation2.Name, accommodationsReturn.Last().Name);
            Assert.Equal(accommodation1.AccommodationTypeId, accommodationsReturn.First().AccommodationType.Id);
            Assert.Equal(accommodation2.AccommodationTypeId, accommodationsReturn.Last().AccommodationType.Id);

            fixture.CleanDatabase();
        }

        [Fact]
        public async Task GetById_ReturnsAccommodation_WhenAccommodationsInDB()
        {
            var accommodationType = new AccommodationType
            {
                Name = "Test Type"
            };
            var accType = await fixture.InsertObjectIntoDatabase(accommodationType);

            var country = await fixture.InsertObjectIntoDatabase(new Country() { Name = "Russia" });
            var region = await fixture.InsertObjectIntoDatabase(new Region() { Name = "Московская область", CountryId = country.Id });
            var city = await fixture.InsertObjectIntoDatabase(new City { Name = "Moscow", RegionId = region.Id });

            var address1 = await fixture.InsertObjectIntoDatabase(new Domain.Entities.Address
            {
                Street = "Test Street",
                ZipCode = "123456",
                Longitude = 0,
                Latitude = 0,
                CityId = city.Id
            });
            var address2 = await fixture.InsertObjectIntoDatabase(new Domain.Entities.Address
            {
                Street = "Test Street1",
                ZipCode = "123458",
                Longitude = 0,
                Latitude = 0,
                CityId = city.Id
            });

            var accommodation1 = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "FirstHotel",
                //AccommodationTypeId = accType.Id,
                AccommodationType = accType,
                AddressId = address1.Id,
                Description = "testAccommodationDescription1",
                StarRating = new AccommodationStarRating { Stars = 4 },
                Rooms = new List<Room>
                {
                    new Room { Name = "330", Capacity = 21, SquareInMeter = 230, RoomType = new RoomType { Name = "Test 1" } },
                    new Room { Name = "340", Capacity = 21, SquareInMeter = 10, RoomType = new RoomType { Name = "Test 2" } }
                }
            });
            var accommodation2 = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "SecondHotel",
                //AccommodationTypeId = accType.Id,
                AccommodationType = accType,
                AddressId = address2.Id,
                Description = "testAccommodationDescription2",
                StarRating = new AccommodationStarRating { Stars = 4 }
            });

            var response = await fixture.Client.GetAsync($"api/accommodations/{accommodation2.Id}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseString = await response.Content.ReadAsStringAsync();
            var accommodationsReturn = JsonConvert.DeserializeObject<AccommodationExtendedDto>(responseString);

            Assert.Equal(accommodation2.Id, accommodationsReturn.Id);
            Assert.Equal(accommodation2.Address.Id, accommodationsReturn.Address.Id);
            Assert.Equal(accommodation2.Name, accommodationsReturn.Name);
            Assert.Equal(accommodation2.AccommodationTypeId, accommodationsReturn.AccommodationType.Id);
            Assert.Equal(accommodation2.StarRating.Stars, accommodationsReturn.Stars);

        }

        [Fact]
        public async Task Delete_ReturnsOk_WhenAccommodationsInDB()
        {

            var accommodationType = new AccommodationType
            {
                Name = "Test Type"
            };
            var accType = await fixture.InsertObjectIntoDatabase(accommodationType);

            var country = await fixture.InsertObjectIntoDatabase(new Country() { Name = "Russia" });
            var region = await fixture.InsertObjectIntoDatabase(new Region() { Name = "Московская область", CountryId = country.Id });
            var city = await fixture.InsertObjectIntoDatabase(new City { Name = "Moscow", RegionId = region.Id });

            var address1 = await fixture.InsertObjectIntoDatabase(new Domain.Entities.Address
            {
                Street = "Test Street",
                ZipCode = "123456",
                Longitude = 0,
                Latitude = 0,
                CityId = city.Id
            });
            var address2 = await fixture.InsertObjectIntoDatabase(new Domain.Entities.Address
            {
                Street = "Test Street1",
                ZipCode = "123458",
                Longitude = 0,
                Latitude = 0,
                CityId = city.Id
            });

            var accommodation1 = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "FirstHotel",
                //AccommodationTypeId = accType.Id,
                AccommodationType = accType,
                AddressId = address1.Id,
                Description = "testAccommodationDescription1",
                StarRating = new AccommodationStarRating { Stars = 4 },
                Rooms = new List<Room>
                {
                    new Room { Name = "330", Capacity = 21, SquareInMeter = 230, RoomType = new RoomType { Name = "Test 1" } },
                    new Room { Name = "340", Capacity = 21, SquareInMeter = 10, RoomType = new RoomType { Name = "Test 2" } }
                }
            });
            var accommodation2 = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "SecondHotel",
                //AccommodationTypeId = accType.Id,
                AccommodationType = accType,
                AddressId = address2.Id,
                Description = "testAccommodationDescription2",
                StarRating = new AccommodationStarRating { Stars = 4 },
                Rooms = new List<Room>
                {
                    new Room { Name = "310", Capacity = 1, SquareInMeter = 20, RoomType = new RoomType { Name = "Test 3" } },
                    new Room { Name = "320", Capacity = 1, SquareInMeter = 20, RoomType = new RoomType { Name = "Test 4" } }
                }
            });
            var accId = accommodation2.Id;

            var responseGetRoomsBefor = await fixture.Client.GetAsync($"api/accommodations/{accId}/rooms");
            var responseRoomsBefor = await responseGetRoomsBefor.Content.ReadAsStringAsync();
            var roomsEmptyListBefor = JsonConvert.DeserializeObject<List<RoomExtendedDto>>(responseRoomsBefor);

            Assert.NotNull(roomsEmptyListBefor);

            int[] accRoomsId = new int[roomsEmptyListBefor.Count];
            int i = 0;
            foreach (var room in roomsEmptyListBefor)
            {
                accRoomsId[i] = room.Id;
                i++;
            }

            var response = await fixture.Client.DeleteAsync($"api/accommodations/{accId}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseGetAcc = await fixture.Client.GetAsync($"api/accommodations/{accId}");
            Assert.Equal(HttpStatusCode.NotFound, responseGetAcc.StatusCode);

            foreach (var roomId in accRoomsId)
            {
                var responseGetRooms = await fixture.Client.GetAsync($"api/rooms/{roomId}");
                Assert.Equal(HttpStatusCode.NotFound, responseGetRooms.StatusCode);
            }
        }

        [Fact]
        public async Task Edit_ReturnsOk_WhenAccommodationsInDB()
        {
            var accommodationType = new AccommodationType
            {
                Name = "Test Type"
            };
            var accommodationTypeEdit = new AccommodationType
            {
                Name = "Test Type2"
            };
            var accType = await fixture.InsertObjectIntoDatabase(accommodationType);

            var country = await fixture.InsertObjectIntoDatabase(new Country() { Name = "Russia" });
            var region = await fixture.InsertObjectIntoDatabase(new Region() { Name = "Московская область", CountryId = country.Id });
            var city = await fixture.InsertObjectIntoDatabase(new City { Name = "Moscow", RegionId = region.Id });

            var address1 = await fixture.InsertObjectIntoDatabase(new Domain.Entities.Address
            {
                Street = "Test Street",
                ZipCode = "123456",
                Longitude = 0,
                Latitude = 0,
                CityId = city.Id
            });
            var address2 = await fixture.InsertObjectIntoDatabase(new Domain.Entities.Address
            {
                Street = "Test Street1",
                ZipCode = "123458",
                Longitude = 0,
                Latitude = 0,
                CityId = city.Id
            });

            Accommodation accommodation1 = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "FirstHotel",
                AccommodationTypeId = accType.Id,
                AccommodationType = accType,
                AddressId = address1.Id,
                Description = "testAccommodationDescription1",
                StarRating = new AccommodationStarRating { Stars = 4 },
                Rooms = new List<Room>
                {
                    new Room { Name = "330", Capacity = 21, SquareInMeter = 230, RoomType = new RoomType { Name = "Test 1" } },
                    new Room { Name = "340", Capacity = 21, SquareInMeter = 10, RoomType = new RoomType { Name = "Test 2" } }
                }
            });
            Accommodation accommodation2 = await fixture.InsertObjectIntoDatabase(new Accommodation
            {
                Name = "SecondHotel",
                AccommodationTypeId = accType.Id,
                AccommodationType = accType,
                AddressId = address2.Id,
                Description = "testAccommodationDescription2",
                //StarRating = new AccommodationStarRating { Stars = 4 },
                Rooms = new List<Room>
                {
                    new Room { Name = "310", Capacity = 1, SquareInMeter = 20, RoomType = new RoomType { Name = "Test 3" } },
                    new Room { Name = "320", Capacity = 1, SquareInMeter = 20, RoomType = new RoomType { Name = "Test 4" } }
                }
            });
            var accommodationChanges = new EditAccommodationDto
            {
                Id = accommodation2.Id,
                Name = "EditAcc",
                AccommodationTypeId = accommodationType.Id,
                Stars = 3,
                Description = "EditDisc"
            };



            StringContent content = new StringContent(JsonConvert.SerializeObject(accommodationChanges), Encoding.UTF8, "application/json");

            var sd = content.ReadAsStringAsync().Result;

            var responsePut = await fixture.Client.PutAsync($"api/accommodations", content);

            Assert.Equal(HttpStatusCode.OK, responsePut.StatusCode);

            var responseGet1 = await fixture.Client.GetAsync($"api/accommodations/{accommodation1.Id}");
            Assert.Equal(HttpStatusCode.OK, responseGet1.StatusCode);

            var responseString1 = await responseGet1.Content.ReadAsStringAsync();
            var accommodationsReturn1 = JsonConvert.DeserializeObject<AccommodationExtendedDto>(responseString1);

            Assert.Equal(accommodation1.Id, accommodationsReturn1.Id);
            Assert.Equal(accommodation1.Address.Id, accommodationsReturn1.Address.Id);
            Assert.Equal(accommodation1.Name, accommodationsReturn1.Name);
            Assert.Equal(accommodation1.AccommodationTypeId, accommodationsReturn1.AccommodationType.Id);
            Assert.Equal(accommodation1.Name, accommodationsReturn1.Name);
            Assert.Equal(accommodation1.StarRating.Stars, accommodationsReturn1.Stars);

            var responseGet2 = await fixture.Client.GetAsync($"api/accommodations/{accommodation2.Id}");
            Assert.Equal(HttpStatusCode.OK, responseGet2.StatusCode);

            var responseString2 = await responseGet2.Content.ReadAsStringAsync();
            var accommodationsReturn2 = JsonConvert.DeserializeObject<AccommodationExtendedDto>(responseString2);

            Assert.Equal(accommodation2.Id, accommodationsReturn2.Id);
            Assert.Equal(accommodation2.Address.Id, accommodationsReturn2.Address.Id);
            Assert.Equal(accommodationChanges.Name, accommodationsReturn2.Name);
            Assert.Equal(accommodationChanges.AccommodationTypeId, accommodationsReturn2.AccommodationType.Id);
            Assert.Equal(accommodationChanges.Name, accommodationsReturn2.Name);
            Assert.Equal(accommodationChanges.Stars, accommodationsReturn2.Stars);
        }

        private AddressDto CreateValidAddressDto(int cityId)
        {
            return new AddressDto
            {
                Street = "Test Street",
                ZipCode = "123456",
                Longitude = 0,
                Latitude = 0,
                CityId = cityId
            };
        }
    }
}
