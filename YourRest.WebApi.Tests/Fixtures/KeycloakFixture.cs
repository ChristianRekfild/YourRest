using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using System.Diagnostics;
using Testcontainers.PostgreSql;

namespace YourRest.WebApi.Tests.Fixtures
{
    public class KeycloakFixture : IDisposable
    {
        private INetwork _network;
        private PostgreSqlContainer _keycloakDbContainer;
        private IContainer _keycloakContainer;

        private static KeycloakFixture? instance = null;
        private static readonly object syncObj = new object();

        //public void EnsureInitialized()
        //{
        //    if (!_initialized)
        //    {
        //        Initialize();
        //        _initialized = true;
        //    }
        //}

        //private bool _initialized = false;


        public static KeycloakFixture Instance()
        {
            if (instance == null)
            {
                lock (syncObj)
                {
                    if (instance == null)
                    {
                        instance = new KeycloakFixture();
                        instance.Start();
                    }
                }
            }
            return instance;
        }
        
        private KeycloakFixture()
        {        
            //RemoveContainer("keycloakdb-test");
            //CreateNetwork("yourrest_local-network");
            //BuildDockerImage("../../../Dockerfile", "keycloak_test:latest");

            _network = new NetworkBuilder()
                //.WithName(Guid.NewGuid().ToString("D"))
                .WithName("yourrest_local-network")
                .WithCleanUp(true)
                .Build();

            _keycloakDbContainer = new PostgreSqlBuilder()
                //.WithImage("postgres:latest")
                .WithImage("postgres:15.4-alpine")
                .WithName("keycloakdb-test")
                //.WithNetwork("yourrest_local-network")
                .WithNetwork(_network)
                .WithUsername("keycloak")
                .WithPassword("keycloakpassword")
                .WithDatabase("keycloak-test")
                .WithCleanUp(true)
                .WithAutoRemove(true)
                .Build();

            _keycloakContainer = new ContainerBuilder()
                .WithImage("jboss/keycloak:latest")
                .WithName("keycloak_test")
                //.WithNetwork("yourrest_local-network")
                .WithNetwork(_network)
                .WithAutoRemove(true)
                .WithEnvironment("DB_VENDOR", "postgres")
                .WithEnvironment("DB_ADDR", "keycloakdb-test")
                .WithEnvironment("DB_DATABASE", "keycloak-test")
                .WithEnvironment("DB_USER", "keycloak")
                .WithEnvironment("DB_PASSWORD", "keycloakpassword")
                .WithEnvironment("KEYCLOAK_USER", "admin")
                .WithEnvironment("KEYCLOAK_PASSWORD", "admin")
                .WithPortBinding(8083, 8080)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(8080/*WeatherForecastImage.HttpsPort*/))
                .WithResourceMapping("../../../realm-export.json", "/opt/jboss/keycloak/")
                .WithCommand("-b", "0.0.0.0", "-Dkeycloak.migration.action=import", "-Dkeycloak.migration.provider=singleFile", "-Dkeycloak.migration.file=/opt/jboss/keycloak/realm-export.json", "-Dkeycloak.migration.strategy=OVERWRITE_EXISTING")
                //.WithWaitStrategy(Wait.ForUnixContainer().UntilContainerIsHealthy())
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
            //await Task.Run(() => process.WaitForExit());
        }
        public void Start()
        {
            Task.Run(async () =>
            {
                await _network.CreateAsync();
                await _keycloakDbContainer.StartAsync();
                await _keycloakContainer.StartAsync();
                //await Task.Delay(TimeSpan.FromSeconds(30));
            }).Wait();
        }
        public void CreateNetwork(string networkName)
        {
            var checkNetwork = Process.Start("docker", $"network inspect {networkName}");
            checkNetwork.WaitForExit();

            if (checkNetwork.ExitCode != 0)
            {
                var createNetwork = Process.Start("docker", $"network create {networkName}");
                createNetwork?.WaitForExit();
            }
        }
        public void RemoveNetwork(string networkName)
        {
            var removeNetwork = Process.Start("docker", $"network rm {networkName}");
            removeNetwork?.WaitForExit();
        }
        public void RemoveContainer(string containerName)
        {
            var stopContainer = Process.Start("docker", $"stop {containerName}");
            stopContainer?.WaitForExit();

            //var removeContainer = Process.Start("docker", $"rm {containerName}");
            //removeContainer?.WaitForExit();
        }

        public void Dispose()
        {
            Task.Run(async () =>
            {
                await _keycloakContainer.StopAsync();
                await _keycloakContainer.DisposeAsync();
            }).Wait();
            Task.Run(async () =>
            {
                await _keycloakDbContainer.StopAsync();
                await _keycloakDbContainer.DisposeAsync();
            }).Wait();
            RemoveNetwork("yourrest_local-network");
        }
    }
}



