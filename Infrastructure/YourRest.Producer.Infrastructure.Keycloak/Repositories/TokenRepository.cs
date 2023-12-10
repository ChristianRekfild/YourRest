using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Options;
using YourRest.Domain.Repositories;
using YourRest.Domain.Models;
using Newtonsoft.Json;
using YourRest.Producer.Infrastructure.Keycloak.Http;
using YourRest.Producer.Infrastructure.Keycloak.Settings;

namespace YourRest.Producer.Infrastructure.Keycloak.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly ICustomHttpClientFactory _httpClientFactory;
        private readonly string _url;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _realmName;

        public TokenRepository(ICustomHttpClientFactory httpClientFactory, IOptions<KeycloakSetting> settings)
        {
            _httpClientFactory = httpClientFactory;
            _url = settings.Value.KeycloakUrl;
            _clientId = settings.Value.ClientId;
            _clientSecret = settings.Value.ClientSecret;
            _realmName = settings.Value.RealmName;
        }
        public async Task<Token> GetTokenAsync(string username, string password)
        {
            using var httpClient = GetConfiguredHttpClient();

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("client_id", _clientId),
                new KeyValuePair<string, string>("client_secret", _clientSecret),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            });

            var response = await httpClient.PostAsync($"{_url}/auth/realms/{_realmName}/protocol/openid-connect/token", content);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Token>(result);
        }
        
        public async Task<Token> GetAdminTokenAsync()
        {
            using var httpClient = GetConfiguredHttpClient();

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("client_id", "admin-cli"),
                new KeyValuePair<string, string>("username", "admin"),
                new KeyValuePair<string, string>("password", "admin")
            });
            var response = await httpClient.PostAsync($"{_url}/auth/realms/master/protocol/openid-connect/token", content);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Token>(result);
        }
        
        public async Task CreateRealm(string adminToken, string realmName)
        {
            using var httpClient = GetConfiguredHttpClient(adminToken);
            var url = $"{_url}/auth/admin/realms";
            var content = new StringContent(JsonConvert.SerializeObject(new { realm = realmName, enabled = true }), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
        }
        public async Task CreateClient(string adminToken, string realmName, string clientId, string clientName)
        {
            using var httpClient = GetConfiguredHttpClient(adminToken);
            var url = $"{_url}/auth/admin/realms/{realmName}/clients";
            var payload = new
            {
                clientId = clientId,
                name = clientName,
                publicClient = false,
                bearerOnly = false,
                directAccessGrantsEnabled = true, // for "password" grant type
                clientAuthenticatorType = "client-secret",
                redirectUris = new[] { "http://test.com/" }
            };

            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, content);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error creating client: {errorContent}");
            }

        }

        public async Task<string> CreateUser(string adminToken, string username, string firstName, string lastName, string email, string password)
        {
            using var httpClient = GetConfiguredHttpClient(adminToken);
            var url = $"{_url}/auth/admin/realms/{_realmName}/users";

            try
            {
                // Step 1: Create the user
                var payload = new
                {
                    username = username,
                    firstName = firstName,
                    lastName = lastName,
                    email = email,
                    enabled = true
                };

                var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8,
                    "application/json");
                var response = await httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
            }

            // Step 2: Obtain the ID of the user just created
            var searchUrl = $"{url}?username={username}";
            var searchResponse = await httpClient.GetAsync(searchUrl);
            searchResponse.EnsureSuccessStatusCode();

            var users = JsonConvert.DeserializeObject<List<User>>(await searchResponse.Content.ReadAsStringAsync());
            
            if (users == null || users.Count == 0)
            {
                throw new Exception("Unable to find the newly created user.");
            }

            var userId = users[0].Id;

            // Step 3: Set the password for the user
            var passwordUrl = $"{url}/{userId}/reset-password";
            var passwordPayload = new
            {
                type = "password",
                value = password,
                temporary = false // set to true if you want the user to change the password at next login
            };

            var passwordContent = new StringContent(JsonConvert.SerializeObject(passwordPayload), Encoding.UTF8, "application/json");
            var passwordResponse = await httpClient.PutAsync(passwordUrl, passwordContent);
            passwordResponse.EnsureSuccessStatusCode();

            return userId;
        }
        
        public async Task<User> GetUser(string userId)
        {
            string adminToken = (await GetAdminTokenAsync()).access_token;
            using var httpClient = GetConfiguredHttpClient(adminToken);

            var url = $"{_url}/auth/admin/realms/{_realmName}/users/{userId}";

            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var user = JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            return user;
        }
        
        public async Task<string> CreateGroup(string adminToken, string groupName)
        {
            using var httpClient = GetConfiguredHttpClient(adminToken);
    
            var url = $"{_url}/auth/admin/realms/{_realmName}/groups";
            var payload = new
            {
                name = groupName
            };

            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var searchUrl = $"{url}?search={groupName}";
            var searchResponse = await httpClient.GetAsync(searchUrl);
            searchResponse.EnsureSuccessStatusCode();

            var groups = JsonConvert.DeserializeObject<List<Group>>(await searchResponse.Content.ReadAsStringAsync());
            
            if (groups == null || groups.Count == 0)
            {
                throw new Exception("Unable to find the newly created group.");
            }

            return groups[0].Id;
        }
        public async Task AssignUserToGroup(string adminToken, string userId, string groupId)
        {
            using var httpClient = GetConfiguredHttpClient(adminToken);
            var url = $"{_url}/auth/admin/realms/{_realmName}/users/{userId}/groups/{groupId}";
            var response = await httpClient.PutAsync(url, null); // No content needed, just the PUT request
            response.EnsureSuccessStatusCode();
        }
        
        public async Task<string> RegenerateClientSecret(string adminToken, string clientId)
        {
            using var httpClient = GetConfiguredHttpClient(adminToken);
    
            string internalId;
            try
            {
                internalId = await GetClientIdByName(adminToken, clientId);
            }
            catch
            {
                throw new Exception($"Client with name {clientId} not found in realm {_realmName}.");
            }

            var secretUrl = $"{_url}/auth/admin/realms/{_realmName}/clients/{internalId}/client-secret";
            var secretResponse = await httpClient.PostAsync(secretUrl, null);
            secretResponse.EnsureSuccessStatusCode();

            var secretData = JsonConvert.DeserializeObject<dynamic>(await secretResponse.Content.ReadAsStringAsync());

            return secretData?.value;
        }
        
        public async Task<string> GetClientSecret(string adminToken, string clientId)
        {
            string internalClientId;
            try
            {
                internalClientId = await GetClientIdByName(adminToken, clientId);
            }
            catch
            {
                throw new Exception($"Client with name {clientId} not found in realm {_realmName}.");
            }

            using var httpClient = GetConfiguredHttpClient(adminToken);
            var url = $"{_url}/auth/admin/realms/{_realmName}/clients/{internalClientId}/client-secret";

            var response = await httpClient.GetAsync(url);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new Exception($"Client with internal ID {internalClientId} not found or it doesn't have a secret.");
            }

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var secretObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
            
            return secretObject?.value;
        }
        
        public async Task<string> GetClientIdByName(string adminToken, string clientName)
        {
            using var httpClient = GetConfiguredHttpClient(adminToken);
            var url = $"{_url}/auth/admin/realms/{_realmName}/clients?clientId={clientName}";

            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var clients = JsonConvert.DeserializeObject<List<dynamic>>(await response.Content.ReadAsStringAsync());
            if(clients != null && clients.Count > 0)
            {
                return clients[0].id;
            }

            throw new Exception($"Client with name {clientName} not found in realm {_realmName}.");
        }
        
        public async Task AddClientProtocolMapper(string adminToken, string clientId, string mapperName, string userAttribute, string tokenClaimName)
        {
            using var httpClient = GetConfiguredHttpClient(adminToken);
            string internalClientId;
            try
            {
                internalClientId = await GetClientIdByName(adminToken, clientId);
            }
            catch
            {
                throw new Exception($"Client with name {clientId} not found in realm {_realmName}.");
            }
            
            var url = $"{_url}/auth/admin/realms/{_realmName}/clients/{internalClientId}/protocol-mappers/models";
            var payload = new
            {
                name = mapperName,
                protocol = "openid-connect",
                protocolMapper = "oidc-usermodel-attribute-mapper",
                config = new Dictionary<string, string>
                {
                    { "user.attribute", userAttribute },
                    { "claim.name", tokenClaimName },
                    { "jsonType.label", "String" },
                    { "id.token.claim", "true" }, 
                    { "access.token.claim", "true" }
                }
            };
            
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
        }

        public async Task AddClientGroupMembershipMapper(string adminToken, string clientId, string mapperName, string userAttribute)
        {
            using var httpClient = GetConfiguredHttpClient(adminToken);
            string internalClientId;
            try
            {
                internalClientId = await GetClientIdByName(adminToken,clientId);
            }
            catch
            {
                throw new Exception($"Client with name {clientId} not found in realm {_realmName}.");
            }
            
            var url = $"{_url}/auth/admin/realms/{_realmName}/clients/{internalClientId}/protocol-mappers/models";
            var payload = new
            {
                name = mapperName,
                protocol = "openid-connect",
                protocolMapper = "oidc-group-membership-mapper",
                config = new Dictionary<string, string>
                {
                    { "claim.name", userAttribute },
                    { "full.path", "false" },
                    { "id.token.claim", "true" },
                    { "access.token.claim", "true" }
                }
            };
            
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
        }

        public async Task AddClientAudienceMapper(string adminToken, string clientId, string mapperName)
        {
            using var httpClient = GetConfiguredHttpClient(adminToken);
            
            string internalClientId;
            try
            {
                internalClientId = await GetClientIdByName(adminToken,clientId);
            }
            catch
            {
                throw new Exception($"Client with name {clientId} not found in realm {_realmName}.");
            }
            
            var url = $"{_url}/auth/admin/realms/{_realmName}/clients/{internalClientId}/protocol-mappers/models";
            var payload = new
            {
                name = mapperName,
                protocol = "openid-connect",
                protocolMapper = "oidc-audience-mapper",
                config = new Dictionary<string, string>
                {
                    { "included.client.audience", clientId },
                    { "id.token.claim", "false" },
                    { "access.token.claim", "true" }
                }
            };
            
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
        }
        private HttpClient GetConfiguredHttpClient(string token = "")
        {
            var httpClient = _httpClientFactory.CreateClient();
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return httpClient;
        }
        
    }

}