﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using YourRest.Application.Dto;
using YourRest.Application.Dto.ViewModels;
using YourRest.Domain.Entities;
using YourRest.WebApi.Tests.Fixtures;

namespace YourRest.WebApi.Tests.Controllers
{
    [Collection(nameof(SingletonApiTest))]
    public class AgeRangeControllerTest //: IClassFixture<SingletonApiTest>
    {
        private readonly SingletonApiTest fixture;
        public AgeRangeControllerTest(SingletonApiTest fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task AddAgeRange_ReturnsStatusCodeCreated()
        {
            var ageRange = new AgeRangeDto() { AgeFrom = 6, AgeTo = 14 };
            var content = new StringContent(JsonConvert.SerializeObject(ageRange), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PostAsync($"api/age-ranges", content);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var ageRangeResponse = JsonConvert.DeserializeObject<AgeRangeDto>(responseContent);

            Assert.NotNull(ageRangeResponse);
            Assert.Equal(ageRange.AgeFrom, ageRangeResponse.AgeFrom);
            Assert.Equal(ageRange.AgeTo, ageRangeResponse.AgeTo);
        }

        [Fact]
        public async Task AddAgeRange_ReturnBadRequest_WhenAgeFromGreaterThanAgeTo()
        {
            var ageRange = new AgeRangeDto() { AgeFrom = 14, AgeTo = 6 };
            var content = new StringContent(JsonConvert.SerializeObject(ageRange), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PostAsync($"api/age-ranges", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var errorData = response.Content.ReadFromJsonAsync<ErrorViewModel>();
            var errorDescription = errorData?.Result?.ValidationErrors["AgeTo"].FirstOrDefault();

            Assert.Equal($"'Age To' должно быть больше или равно '{ageRange.AgeFrom}'.", errorDescription);
        }

        [Fact]
        public async Task GetAgeRange_ReturnStatusOK_WhenDbHasAgeRange()
        {
            var ageRangeEntity = new AgeRange() { AgeFrom = 6, AgeTo = 14 };

            AgeRange ageRange = await fixture.InsertObjectIntoDatabase(ageRangeEntity);

            var response = await fixture.Client.GetAsync($"api/age-ranges/{ageRange.Id}");

            var responseContent = await response.Content.ReadAsStringAsync();
            var ageRangeResponse = JsonConvert.DeserializeObject<AgeRangeDto>(responseContent);


            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(ageRangeResponse);
            Assert.Equal(ageRange.AgeFrom, ageRangeResponse.AgeFrom);
            Assert.Equal(ageRange.AgeTo, ageRangeResponse.AgeTo);
        }

        [Fact]
        public async Task PutAgeRange_ReturnsBadRequest_WhenDbEmpty()
        {

            var ageRangePut = new AgeRangeWithIdDto() { Id = 1, AgeFrom = 4, AgeTo = 16 };
            var content = new StringContent(JsonConvert.SerializeObject(ageRangePut), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PutAsync($"api/age-ranges/", content);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PutAgeRange_ReturnsStatusCodeOK_WhenDbHasAgeRange()
        {
            var ageRangeEntity = new AgeRange() { AgeFrom = 6, AgeTo = 14 };
            AgeRange ageRange = await fixture.InsertObjectIntoDatabase(ageRangeEntity);

            var ageRangePut = new AgeRange() { Id = ageRange.Id, AgeFrom = 4, AgeTo = 16 };
            var content = new StringContent(JsonConvert.SerializeObject(ageRangePut), Encoding.UTF8, "application/json");
            var response = await fixture.Client.PutAsync($"api/age-ranges/", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.Equal("The AgeRange has been edited", responseContent);
        }
    }
}
