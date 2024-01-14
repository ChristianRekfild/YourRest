using Newtonsoft.Json;
using System.Net;
using System.Text;
using SystemJson = System.Text.Json;
using YourRest.Application.Dto.Models.AccommodationFacility;
using YourRest.Domain.Entities;
using YourRest.WebApi.Tests.Fixtures;

namespace YourRest.WebApi.Tests.Controllers
{
    [Collection(nameof(SingletonApiTest))]
    public class AccommodationFacilityControllerTest
    {
        private readonly SingletonApiTest fixture;
        public AccommodationFacilityControllerTest(SingletonApiTest fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task GivenAllAccommodationFacility_WhenGetCalled_ReturnsListOfAccommodationFacilityDto()
        {
            var expected = new AccommodationFacility { Name = "test", Description = "test" };
            await fixture.InsertObjectIntoDatabase(expected);
            await fixture.InsertObjectIntoDatabase(new AccommodationFacility { Name = "test2", Description = "test2" });

            var response = await fixture.Client.GetAsync($"api/accommodation-facilities");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var options = new SystemJson.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var result = SystemJson.JsonSerializer.Deserialize<List<AccommodationFacilityWithIdDto>>(content, options);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(expected.Name, result.FirstOrDefault(c => c.Id == expected.Id)?.Name);
        }
        
        [Fact]
        public async Task GivenEmptyDB_WhenApiInvoked_ThenReturnsEmptyList()
        {
            fixture.CleanDatabase();
            
            var response = await fixture.Client.GetAsync($"api/accommodation-facilities");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var options = new SystemJson.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var result = SystemJson.JsonSerializer.Deserialize<List<AccommodationFacilityWithIdDto>>(content, options);
            
            Assert.Empty(result);
        }
        
        [Fact]
        public async Task GivenValidData_WhenFacilityAddedToAccommodation_ThenReturnsSuccess()
        {
            var accommodationType = new AccommodationType { Name = "Test Type" };
            var accommodation = new Accommodation { Name = "TestHotel", AccommodationType = accommodationType };
            var facility = new AccommodationFacility { Name = "TestFacility" };
            await fixture.InsertObjectIntoDatabase(accommodation);
            await fixture.InsertObjectIntoDatabase(facility);
            
            var dto = new AccommodationFacilityIdDto { FacilityId = facility.Id };
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PostAsync($"api/accommodation/{accommodation.Id}/facility", content);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var retrievedAccommodation = await fixture.GetAccommodationById(accommodation.Id);
            Assert.NotNull(retrievedAccommodation);

            var facilityLinkedToAccommodation = retrievedAccommodation.AccommodationFacilities
                .Any(ua => ua.AccommodationId == accommodation.Id);

            Assert.True(facilityLinkedToAccommodation, "The facility is not linked to the accommodation.");
        }
        
        [Fact]
        public async Task GivenNonExistingAccommodation_WhenAddingFacility_ThenReturnsNotFound()
        {
            var nonExistingAccommodationId = 999;
            var facility = new AccommodationFacility { Name = "TestFacility" };
            await fixture.InsertObjectIntoDatabase(facility);

            var dto = new AccommodationFacilityIdDto { FacilityId = facility.Id };
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PostAsync($"api/accommodation/{nonExistingAccommodationId}/facility", content);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var errorMassage = await response.Content.ReadAsStringAsync();
            var expectedMessage = new { message = $"Accommodation with id {nonExistingAccommodationId} not found" };
            var expectedMessageJson = JsonConvert.SerializeObject(expectedMessage);
            Assert.Equal(errorMassage, expectedMessageJson);
        }
        
        [Fact]
        public async Task GivenFacilityAlreadyLinked_WhenAddingSameFacility_ThenReturnsConflict()
        {
            var accommodationType = new AccommodationType { Name = "Test Type" };
            var accommodation = new Accommodation { Name = "TestHotel", AccommodationType = accommodationType };
            var facility = new AccommodationFacility { Name = "TestFacility" };
            accommodation.AccommodationFacilities.Add(new AccommodationFacilityLink
            {
                AccommodationFacility = facility,
                Accommodation = accommodation,
            });
            await fixture.InsertObjectIntoDatabase(accommodation);
                
            var dto = new AccommodationFacilityIdDto { FacilityId = facility.Id };
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PostAsync($"api/accommodation/{accommodation.Id}/facility", content);

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            var errorMassage = await response.Content.ReadAsStringAsync();
            var expectedMessage = new { message = $"Facility with id {facility.Id} is already linked to this accommodation." };
            var expectedMessageJson = JsonConvert.SerializeObject(expectedMessage);
            Assert.Equal(errorMassage, expectedMessageJson);
        }
        
        [Fact]
        public async Task GivenNonExistingFacility_WhenAddingToAccommodation_ThenReturnsNotFound()
        {
            var accommodationType = new AccommodationType { Name = "Test Type" };
            var accommodation = new Accommodation { Name = "TestHotel", AccommodationType = accommodationType };
            await fixture.InsertObjectIntoDatabase(accommodation);
            var nonExistingFacilityId = 999;
            
            var dto = new AccommodationFacilityIdDto { FacilityId = nonExistingFacilityId };
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PostAsync($"api/accommodation/{accommodation.Id}/facility", content);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var errorMassage = await response.Content.ReadAsStringAsync();
            var expectedMessage = new { message = $"AccommodationFacility with id {nonExistingFacilityId} not found" };
            var expectedMessageJson = JsonConvert.SerializeObject(expectedMessage);
            Assert.Equal(errorMassage, expectedMessageJson);
        }
        
        [Fact]
        public async Task GivenValidAccommodationId_WhenRequested_ThenReturnsFacilities()
        {
            fixture.CleanDatabase();

            var accommodationType = new AccommodationType { Name = "Test Type" };
            var accommodation = new Accommodation { Name = "TestHotel", AccommodationType = accommodationType };
            var facility1 = new AccommodationFacility { Name = "TestFacility" };
            accommodation.AccommodationFacilities.Add(new AccommodationFacilityLink
            {
                AccommodationFacility = facility1,
                Accommodation = accommodation,
            });
            await fixture.InsertObjectIntoDatabase(accommodation);
            
            var facility2 = new AccommodationFacility { Name = "TestFacility2" };
            accommodation.AccommodationFacilities.Add(new AccommodationFacilityLink
            {
                AccommodationFacility = facility2,
                Accommodation = accommodation,
            });
            await fixture.SaveChangesAsync();
            
            var response = await fixture.Client.GetAsync($"api/accommodation/{accommodation.Id}/facilities");

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var facilities = SystemJson.JsonSerializer.Deserialize<List<AccommodationFacilityWithIdDto>>(content, new SystemJson.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(facilities);
            Assert.Contains(facilities, f => f.Id == facility1.Id);
            Assert.Contains(facilities, f => f.Id == facility2.Id);
        }
        
        [Fact]
        public async Task GivenNonExistingAccommodationId_WhenRequested_ThenReturnsNotFound()
        {
            var nonExistingAccommodationId = 999;

            var response = await fixture.Client.GetAsync($"api/accommodation/{nonExistingAccommodationId}/facilities");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var errorMassage = await response.Content.ReadAsStringAsync();
            var expectedMessage = new { message = $"Accommodation with id number {nonExistingAccommodationId} not found" };
            var expectedMessageJson = JsonConvert.SerializeObject(expectedMessage);
            Assert.Equal(errorMassage, expectedMessageJson);
        }
        
        [Fact]
        public async Task GivenAccommodationWithNoFacilities_WhenRequested_ThenReturnsNotFound()
        {
            var accommodationType = new AccommodationType { Name = "Test Type" };
            var accommodation = new Accommodation { Name = "TestHotel", AccommodationType = accommodationType };
            await fixture.InsertObjectIntoDatabase(accommodation);

            var response = await fixture.Client.GetAsync($"api/accommodation/{accommodation.Id}/facilities");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var errorMassage = await response.Content.ReadAsStringAsync();
            var expectedMessage = new { message = $"Not found AccommodationFacility in current accommodation (id : {accommodation.Id})" };
            var expectedMessageJson = JsonConvert.SerializeObject(expectedMessage);
            Assert.Equal(errorMassage, expectedMessageJson);
        }
    }
}


