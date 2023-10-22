using YourRest.Domain.Models;

namespace YourRest.Domain.Repositories;
public interface ITokenRepository
{
    Task<Token> GetTokenAsync(string username, string password);
    Task<Token> GetAdminTokenAsync();
    Task CreateRealm(string adminToken, string realmName);
    Task CreateClient(string adminToken, string realmName, string clientId, string clientName);

    Task<string> CreateUser(string adminToken, string realmName, string username, string firstName, string lastName,
        string email, string password);
    Task<string> CreateGroup(string adminToken, string realmName, string groupName);
    Task AssignUserToGroup(string adminToken, string realmName, string userId, string groupId);
    Task<string> GetClientSecret(string adminToken, string realmName, string clientId);
    Task<string> GetClientIdByName(string adminToken, string realmName, string clientName);
    void SetCredentials(string clientId, string clientSecret, string keycloakUrl);
    Task<string> RegenerateClientSecret(string adminToken, string realmName, string clientId);

    Task AddClientProtocolMapper(string adminToken, string realmName, string clientId, string mapperName,
        string userAttribute, string tokenClaimName);

    Task AddClientGroupMembershipMapper(string adminToken, string realmName, string clientId, string mapperName,
        string userAttribute);

    Task AddClientAudienceMapper(string adminToken, string realmName, string clientId, string mapperName);

}