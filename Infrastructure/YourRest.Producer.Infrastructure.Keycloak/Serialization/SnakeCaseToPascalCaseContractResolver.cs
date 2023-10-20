using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace YourRest.Producer.Infrastructure.Keycloak.Serialization
{
    public class SnakeCaseToPascalCaseContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            // Convert the snake_case or kebab-case to PascalCase
            var parts = propertyName.Split(new char[] { '_', '-' }, StringSplitOptions.RemoveEmptyEntries);
            return string.Join("", parts.Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1).ToLowerInvariant()));
        }
    }

}