using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Testcontainers.PostgreSql;
using System.Diagnostics;

namespace YourRest.WebApi.Tests.Fixtures
{
    public class KeycloakFixture : IDisposable
    {
        private PostgreSqlContainer _keycloakDbContainer;
        private IContainer _keycloakContainer;
        
        public async Task EnsureInitializedAsync()
        {
            if (!_initialized)
            {
                await InitializeAsync();
                _initialized = true;
            }
        }

        private bool _initialized = false;
        public static KeycloakFixture Instance => _instance.Value;
        private KeycloakFixture()
        {
        }
        
        private static readonly Lazy<KeycloakFixture> _instance = 
            new Lazy<KeycloakFixture>(() => new KeycloakFixture());
        private async Task InitializeAsync()
        {
            await RemoveContainerAsync("keycloakdb-test");
            await CreateNetworkAsync("yourrest_local-network");
            await BuildDockerImageAsync("../../../Dockerfile", "keycloak_test:latest");

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

        public static async Task BuildDockerImageAsync(string dockerfilePath, string tagName)
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

            await Task.Run(() => process.WaitForExit());
        }
        public async Task StartAsync()
        {
            await _keycloakDbContainer.StartAsync();
            await _keycloakContainer.StartAsync();
            await Task.Delay(TimeSpan.FromSeconds(30));
        }
        public async Task CreateNetworkAsync(string networkName)
        {
            var checkNetwork = Process.Start("docker", $"network inspect {networkName}");
            await Task.Run(() => checkNetwork.WaitForExit());

            if (checkNetwork.ExitCode != 0)
            {
                var createNetwork = Process.Start("docker", $"network create {networkName}");
                await Task.Run(() => createNetwork?.WaitForExit());
            }
        }
        public async Task RemoveNetworkAsync(string networkName)
        {
            var removeNetwork = Process.Start("docker", $"network rm {networkName}");
            await Task.Run(() => removeNetwork?.WaitForExit());
        }
        public async Task RemoveContainerAsync(string containerName)
        {
            var stopContainer = Process.Start("docker", $"stop {containerName}");
            await Task.Run(() => stopContainer?.WaitForExit());

            var removeContainer = Process.Start("docker", $"rm {containerName}");
            await Task.Run(() => removeContainer?.WaitForExit());
        }

        public async ValueTask DisposeAsync()
        {
            await _keycloakContainer.StopAsync();
            await _keycloakContainer.DisposeAsync();
            await _keycloakDbContainer.StopAsync();
            await _keycloakDbContainer.DisposeAsync();
            await RemoveNetworkAsync("yourrest_local-network");
        }

        public void Dispose()
        {
            DisposeAsync().AsTask().Wait();
        }
    }
}



