using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Testcontainers.PostgreSql;
using System.Diagnostics;

namespace YourRest.WebApi.Tests.Fixtures;

public class KeycloakFixture : IDisposable
{
    private readonly PostgreSqlContainer _keycloakDbContainer;
    private IContainer _keycloakContainer;
    private static KeycloakFixture? instance = null;
    private static readonly object syncObj = new object();
    private string _containerName = "keycloak-test";


    private KeycloakFixture()
    {
        RemoveContainer("keycloakdb-test");
        CreateNetwork("yourrest_local-network");
        BuildDockerImage("../../../Dockerfile", "keycloak_test:latest");

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
            .WithImage("keycloak_test:latest")
            .WithName("keycloak_test")
            .WithNetwork("yourrest_local-network")
            .WithEnvironment("DB_VENDOR", "postgres")
            .WithEnvironment("DB_ADDR", "keycloakdb-test") 
            .WithEnvironment("DB_DATABASE", "keycloak-test")
            .WithEnvironment("DB_USER", "keycloak")
            .WithEnvironment("DB_PASSWORD", "keycloakpassword")
            .WithEnvironment("KEYCLOAK_USER", "admin")
            .WithEnvironment("KEYCLOAK_PASSWORD", "admin")
            .WithPortBinding(8081, 8080)
            .Build();
    }
    
    public static void BuildDockerImage(string dockerfilePath, string tagName)
    {
        string dockerfileDirectory = Path.GetDirectoryName(dockerfilePath);

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "docker",
                Arguments = $"build -t {tagName} -f {dockerfilePath} {dockerfileDirectory}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.OutputDataReceived += (sender, data) => Console.WriteLine(data.Data);
        process.ErrorDataReceived += (sender, data) => Console.WriteLine("Error: " + data.Data);

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        process.WaitForExit();
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

        return instance;
    }

    public async Task StartAsync()
    {
        await _keycloakDbContainer.StartAsync();
        await _keycloakContainer.StartAsync();
        
        await Task.Delay(TimeSpan.FromSeconds(5));
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
    
    public void RemoveContainer(string containerName)
    {
        Process.Start("docker", $"stop {containerName}")?.WaitForExit();
        Process.Start("docker", $"rm {containerName}")?.WaitForExit();
    }
}


