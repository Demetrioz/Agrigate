namespace Agrigate.Api;

/// <summary>
/// Constant values used throughout Agrigate
/// </summary>
public class Constants
{
    /// <summary>
    /// Policy Names
    /// </summary>
    public class Policies
    {
        /// <summary>
        /// Policy name used for API key authentication
        /// </summary>
        public const string ApiKeyPolicy = "ApiKeyPolicy";
    }

    /// <summary>
    /// Authentication related constants
    /// </summary>
    public class Authentication
    {
        /// <summary>
        /// The name of the scheme allowing Swagger to authenticate requests
        /// </summary>
        public const string SchemeName = "Api Key Scheme";

        /// <summary>
        /// The id of the scheme definition allowing Swagger to authenticate
        /// requests
        /// </summary>
        public const string SchemeDefinition = "API Key";

        /// <summary>
        /// Description for the swagger authentication scheme
        /// </summary>
        public const string SchemeDescription = "Enter the API Key";

        /// <summary>
        /// The header name that should be sent with the API key
        /// </summary>
        public const string ApiKeyHeader = "AgrigateApiKey";
    }
}