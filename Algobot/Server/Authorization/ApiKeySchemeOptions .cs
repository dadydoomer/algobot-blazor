using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace BlazorAlgoBot.Server.Authorization
{
    public class ApiKeySchemeOptions : AuthenticationSchemeOptions
    {
        public const string SchemaName = "ApiKey";
        public const string PolicyName = "ApiKeyPolicy";
        public const string Section = "ApiKeyOptions";
        public const string HeaderName = "X-Api-Key";

        public string? ApiKey { get; set; }
        public bool ReadOnly { get; set; } = true;

        public override void Validate()
        {
            if (string.IsNullOrEmpty(ApiKey))
            {
                throw new ArgumentException("API key must be provided.");
            }
        }
    }
}
