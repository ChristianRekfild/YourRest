namespace YourRest.Producer.Infrastructure.Keycloak.Settings
{
    public class KeycloakSetting
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Authority { get; set; }
        public string KeycloakUrl { get; set; }
        public string RealmName { get; set; }
        
    }
}