using YourRest.Infrastructure.Core.Contracts.AuthModels;

namespace YourRest.Infrastructure.Core.Contracts.Repositories
{
    public interface ITokenRepository
    {
        Task<TokenDto> GetTokenAsync(string username, string password);
        Task<TokenDto> GetAdminTokenAsync(CancellationToken cancellationToken = default);
        Task CreateRealm(string adminToken, string realmName);
        Task CreateClient(string adminToken, string realmName, string clientId, string clientName);

        Task<string> CreateUser(string adminToken, string username, string firstName, string lastName,
            string email, string password);

        Task<UserDto> GetUser(string userId, CancellationToken cancellationToken);
        Task<string> CreateGroup(string adminToken, string groupName);
        Task AssignUserToGroup(string adminToken, string userId, string groupId);
        Task<string> GetClientSecret(string adminToken, string clientId);
        Task<string> GetClientIdByName(string adminToken, string clientName);
        Task<string> RegenerateClientSecret(string adminToken, string clientId);
        Task AddClientProtocolMapper(string adminToken, string clientId, string mapperName,
            string userAttribute, string tokenClaimName);

        Task AddClientGroupMembershipMapper(string adminToken, string clientId, string mapperName,
            string userAttribute);

        Task AddClientAudienceMapper(string adminToken, string clientId, string mapperName);
    }
}