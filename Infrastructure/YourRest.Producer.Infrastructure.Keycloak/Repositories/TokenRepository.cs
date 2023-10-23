using System.Net.Http.Headers;
using System.Text;
using YourRest.Domain.Repositories;
using YourRest.Domain.Models;
using Newtonsoft.Json;
using YourRest.Producer.Infrastructure.Keycloak.Http;

namespace YourRest.Producer.Infrastructure.Keycloak.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly ICustomHttpClientFactory _httpClientFactory;
        private string? _clientId;
        private string? _clientSecret;
        private string? _keycloakUrl;
        private string? _url;

        public TokenRepository(ICustomHttpClientFactory httpClientFactory, string? clientId, string? clientSecret, string? keycloakUrl, string? url = null)
        {
            _httpClientFactory = httpClientFactory;
            _clientId = clientId;
            _clientSecret = clientSecret;
            _keycloakUrl = keycloakUrl;
            _url = url;
        }

        public void SetCredentials(string clientId, string clientSecret, string keycloakUrl, string url)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _keycloakUrl = keycloakUrl;
            _url = url;
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

            var response = await httpClient.PostAsync(_keycloakUrl + "/protocol/openid-connect/token", content);
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

        public async Task<string> CreateUser(string adminToken, string realmName, string username, string firstName, string lastName, string email, string password)
        {
            using var httpClient = GetConfiguredHttpClient(adminToken);
    
            // Step 1: Create the user
            var url = $"{_url}/auth/admin/realms/{realmName}/users";
            var payload = new
            {
                username = username,
                firstName = firstName,
                lastName = lastName,
                email = email,
                enabled = true
            };

            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            // Step 2: Obtain the ID of the user just created
            var searchUrl = $"{url}?username={username}";
            var searchResponse = await httpClient.GetAsync(searchUrl);
            searchResponse.EnsureSuccessStatusCode();

            var users = JsonConvert.DeserializeObject<List<User>>(await searchResponse.Content.ReadAsStringAsync());
            if (users.Count == 0)
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
        
        public async Task<string> CreateGroup(string adminToken, string realmName, string groupName)
        {
            using var httpClient = GetConfiguredHttpClient(adminToken);
    
            var url = $"{_url}/auth/admin/realms/{realmName}/groups";
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
            if (groups.Count == 0)
            {
                throw new Exception("Unable to find the newly created group.");
            }

            return groups[0].Id;
        }
        public async Task AssignUserToGroup(string adminToken, string realmName, string userId, string groupId)
        {
            using var httpClient = GetConfiguredHttpClient(adminToken);
            var url = $"{_url}/auth/admin/realms/{realmName}/users/{userId}/groups/{groupId}";
            var response = await httpClient.PutAsync(url, null); // No content needed, just the PUT request
            response.EnsureSuccessStatusCode();
        }
        
        public async Task<string> RegenerateClientSecret(string adminToken, string realmName, string clientId)
        {
            using var httpClient = GetConfiguredHttpClient(adminToken);
    
            string internalId;
            try
            {
                internalId = await GetClientIdByName(adminToken, realmName, clientId);
            }
            catch
            {
                throw new Exception($"Client with name {clientId} not found in realm {realmName}.");
            }

            var secretUrl = $"{_url}/auth/admin/realms/{realmName}/clients/{internalId}/client-secret";
            var secretResponse = await httpClient.PostAsync(secretUrl, null);
            secretResponse.EnsureSuccessStatusCode();

            var secretData = JsonConvert.DeserializeObject<dynamic>(await secretResponse.Content.ReadAsStringAsync());

            return secretData.value;
        }
        
        public async Task<string> GetClientSecret(string adminToken, string realmName, string clientId)
        {
            string internalClientId;
            try
            {
                internalClientId = await GetClientIdByName(adminToken, realmName, clientId);
            }
            catch
            {
                throw new Exception($"Client with name {clientId} not found in realm {realmName}.");
            }

            using var httpClient = GetConfiguredHttpClient(adminToken);
            var url = $"{_url}/auth/admin/realms/{realmName}/clients/{internalClientId}/client-secret";

            var response = await httpClient.GetAsync(url);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new Exception($"Client with internal ID {internalClientId} not found or it doesn't have a secret.");
            }

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var secretObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
            return secretObject.value;
        }
        
        public async Task<string> GetClientIdByName(string adminToken, string realmName, string clientName)
        {
            using var httpClient = GetConfiguredHttpClient(adminToken);
            var url = $"{_url}/auth/admin/realms/{realmName}/clients?clientId={clientName}";

            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var clients = JsonConvert.DeserializeObject<List<dynamic>>(await response.Content.ReadAsStringAsync());
            if(clients != null && clients.Count > 0)
            {
                return clients[0].id;
            }

            throw new Exception($"Client with name {clientName} not found in realm {realmName}.");
        }
        
        public async Task AddClientProtocolMapper(string adminToken, string realmName, string clientId, string mapperName, string userAttribute, string tokenClaimName)
        {
            using var httpClient = GetConfiguredHttpClient(adminToken);
            string internalClientId;
            try
            {
                internalClientId = await GetClientIdByName(adminToken, realmName, clientId);
            }
            catch
            {
                throw new Exception($"Client with name {clientId} not found in realm {realmName}.");
            }
            
            var url = $"{_url}/auth/admin/realms/{realmName}/clients/{internalClientId}/protocol-mappers/models";
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

        public async Task AddClientGroupMembershipMapper(string adminToken, string realmName, string clientId, string mapperName, string userAttribute)
        {
            using var httpClient = GetConfiguredHttpClient(adminToken);
            string internalClientId;
            try
            {
                internalClientId = await GetClientIdByName(adminToken, realmName, clientId);
            }
            catch
            {
                throw new Exception($"Client with name {clientId} not found in realm {realmName}.");
            }
            
            var url = $"{_url}/auth/admin/realms/{realmName}/clients/{internalClientId}/protocol-mappers/models";
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

        public async Task AddClientAudienceMapper(string adminToken, string realmName, string clientId, string mapperName)
        {
            using var httpClient = GetConfiguredHttpClient(adminToken);
            
            string internalClientId;
            try
            {
                internalClientId = await GetClientIdByName(adminToken, realmName, clientId);
            }
            catch
            {
                throw new Exception($"Client with name {clientId} not found in realm {realmName}.");
            }
            
            var url = $"{_url}/auth/admin/realms/{realmName}/clients/{internalClientId}/protocol-mappers/models";
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
        private HttpClient GetConfiguredHttpClient(string token = null)
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