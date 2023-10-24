using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Testcontainers.PostgreSql;
using YourRest.Domain.Repositories;
using YourRest.Domain.Models;
using YourRest.Producer.Infrastructure.Keycloak.Repositories;
using System.Diagnostics;

namespace YourRest.WebApi.Tests.Fixtures;

public class KeycloakFixture : IDisposable
{
    private readonly PostgreSqlContainer _keycloakDbContainer;
    private readonly ITokenRepository _tokenRepository;
    private IContainer _keycloakContainer;
    private static KeycloakFixture? instance = null;
    private static readonly object syncObj = new object();

    private string _secret;
    private string _userId;
    private string _url = "http://localhost:8081";
    private string _realm = "YourRestTest";
    private string _firstName = "test_name";
    private string _userName = "test_username";
    private string _lastName = "test_lastname";
    private string _email = "test@test.ru";
    private string _password = "test";
    private string _clientId = "your_rest_app_test";
    private string _clientName = "YourClientName";
    private bool isInitialized = false;
    public string GetTestRealmUrl()
    {
        return $"{_url}/auth/realms/{_realm}";
    }
    public string GetTestAudience() => _clientId;
    public string GetTestSymmetricKey() => _secret;

    private KeycloakFixture()
    {
        RemoveContainer("keycloakdb-test");
        CreateNetwork("yourrest_local-network");

        _keycloakDbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithName("keycloakdb-test")
            .WithNetwork("yourrest_local-network")
            .WithUsername("keycloak")
            .WithPassword("keycloakpassword")
            .WithDatabase("keycloak-test")
            .WithCleanUp(true)
            .Build();
        
        _keycloakContainer = new ContainerBuilder()
            .WithImage("jboss/keycloak:latest")
            .WithName("keycloak-test")
            .WithNetwork("yourrest_local-network")
            .WithEnvironment("DB_VENDOR", "postgres")
            .WithEnvironment("DB_ADDR", "keycloakdb-test") 
            .WithEnvironment("DB_DATABASE", "keycloak-test")
            .WithEnvironment("DB_USER", "keycloak")
            .WithEnvironment("DB_PASSWORD", "keycloakpassword")
            .WithEnvironment("KEYCLOAK_USER", "admin-test")
            .WithEnvironment("KEYCLOAK_PASSWORD", "admin-test")
            .WithPortBinding(8081, 8080)
            .Build();

        _tokenRepository = new TokenRepository(new TestHttpClientFactory(), null, null, null, _url);
    }
    public void RemoveContainer(string containerName)
    {
        Process.Start("docker", $"stop {containerName}")?.WaitForExit();
        Process.Start("docker", $"rm {containerName}")?.WaitForExit();
    }
    
    public static async Task<KeycloakFixture> GetInstanceAsync()
    {
        if (instance == null)
        {
            lock (syncObj)
            {
                if (instance == null)
                {
                    instance = new KeycloakFixture();
                }
            }
        }

        await instance.InternalInitializeAsync();
        return instance;
    }

    private async Task InternalInitializeAsync()
    {
        if (!isInitialized)
        {
            await _keycloakDbContainer.StartAsync();
            await _keycloakContainer.StartAsync();
            await Task.Delay(TimeSpan.FromSeconds(10));
            
            Token token = await _tokenRepository.GetAdminTokenAsync();
            string adminToken = token.access_token;
            try
            {
                await _tokenRepository.CreateRealm(adminToken, _realm);
            }
            catch { }
            
            try
            {
                await _tokenRepository.CreateClient(adminToken, _realm, _clientId, _clientName);
            }
            catch { }
            
            _secret = await _tokenRepository.RegenerateClientSecret(adminToken, _realm, _clientId);
            
            _tokenRepository.SetCredentials(_clientId, _secret, GetTestRealmUrl(), _url);
             
             try
             {
                 await _tokenRepository.AddClientAudienceMapper(adminToken, _realm, _clientId, "audience-mapper");
             }
             catch { }
             
             try
             {
                 await _tokenRepository.AddClientGroupMembershipMapper(adminToken, _realm, _clientId, "groupMapper", "groups");
             }
             catch { }
             
             try
             {
                 await _tokenRepository.AddClientProtocolMapper(adminToken, _realm, _clientId, "emailMapper", "email", "email");
             }
             catch { }
             
             try
             {
                 await _tokenRepository.AddClientProtocolMapper(adminToken, _realm, _clientId, "lastNameMapper", "lastName", "family_name");
             }
             catch { }
             
             try
             {
                 await _tokenRepository.AddClientProtocolMapper(adminToken, _realm, _clientId, "firstNameMapper", "firstName", "given_name");
             }
             catch { }
             
             try
             {
                 await _tokenRepository.AddClientProtocolMapper(adminToken, _realm, _clientId, "subMapper", "keyCloakId", "sub");
             }
             catch { }

            isInitialized = true;
        }
    }
    public async Task CreateGroup(int accommodationId)
    {
        string? groupId = null;
        string adminToken = (await _tokenRepository.GetAdminTokenAsync()).access_token;

        try
        {
            _userId = await _tokenRepository.CreateUser(adminToken, _realm, _userName, _firstName, _lastName, _email, _password);
        }
        catch { }
        try
        {
            groupId = await _tokenRepository.CreateGroup(adminToken, _realm, "/accommodations/" + accommodationId);
        }
        catch { }
        try
        {
            if (groupId != null)
            {
                await _tokenRepository.AssignUserToGroup(adminToken, _realm, _userId, groupId);
            }
        }
        catch { }
    }

    public async Task<string> GetAccessTokenAsync()
    {
        return (await _tokenRepository.GetTokenAsync(_userName, _password)).access_token;
    }
    
    public void CreateNetwork(string networkName)
    {
        var checkNetwork = Process.Start("docker", $"network inspect {networkName}");
        checkNetwork.WaitForExit();

        if (checkNetwork.ExitCode != 0)
        {
            Process.Start("docker", $"network create {networkName}")?.WaitForExit();
        }
    }
    
    public void RemoveNetwork(string networkName)
    {
        Process.Start("docker", $"network rm {networkName}")?.WaitForExit();
    }
    public void Dispose()
    {
        Task.Run(async () => await _keycloakContainer.StopAsync()).Wait();
        Task.Run(async () => await _keycloakContainer.DisposeAsync()).Wait();
        Task.Run(async () => await _keycloakDbContainer.StopAsync()).Wait();
        Task.Run(async () => await _keycloakDbContainer.DisposeAsync()).Wait();
        
        RemoveNetwork("yourrest_local-network");
    }
}


