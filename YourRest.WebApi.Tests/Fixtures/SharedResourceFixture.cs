using YourRest.Domain.Repositories;

namespace YourRest.WebApi.Tests.Fixtures
{
    public class SharedResourcesFixture : IDisposable
    {
        public string SharedUserId { get; private set; }
        public string SharedAccessToken { get; private set; }
        private readonly ITokenRepository _tokenRepository;

        public SharedResourcesFixture(ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
            InitializeAsync().Wait();
        }

        private async Task InitializeAsync()
        {
            string adminToken = (await _tokenRepository.GetAdminTokenAsync()).access_token;
            SharedUserId = await _tokenRepository.CreateUser(adminToken, "test", "test", "test", "test@test.ru", "test");
            SharedAccessToken = await GetAccessTokenAsync();
        }
        
        private async Task<string> GetAccessTokenAsync()
        {
            return (await _tokenRepository.GetTokenAsync("test", "test")).access_token;
        }

        public void Dispose()
        {
            // Cleanup if necessary
        }
    }
}