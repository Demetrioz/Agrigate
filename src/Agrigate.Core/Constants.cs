namespace Agrigate.Core;

/// <summary>
/// Constants used across Agrigate
/// </summary>
public class Constants
{
    /// <summary>
    /// Configuration related constants
    /// </summary>
    public class Logging
    {
        /// <summary>
        /// The configuration section containing logging related setup
        /// </summary>
        public const string Configuration = "Logging";
        
        /// <summary>
        /// A default source used for identifying where logs are coming from
        /// </summary>
        public const string DefaultSource = "Agrigate";

        /// <summary>
        /// Labels used for identifying logs within Loki
        /// </summary>
        public class Labels
        {
            /// <summary>
            /// The service name where the log originated from
            /// </summary>
            public const string ServiceName = "service_name";
        }
    }

    /// <summary>
    /// Authentication related constants
    /// </summary>
    public class Authentication
    {
        /// <summary>
        /// The configuration section containing authentication related setup
        /// </summary>
        public const string Configuration = "Authentication";
        
        /// <summary>
        /// Authentication policy names
        /// </summary>
        public class Policies
        {
            public const string Oidc = "oidc";
        }

        /// <summary>
        /// Claim types from auth tokens
        /// </summary>
        public class ClaimTypes
        {
            public const string Roles = "roles";
        }

        /// <summary>
        /// Routes used for handling authentication
        /// </summary>
        public class Routes
        {
            public const string Auth = "/authentication";
            public const string Login = "/login";
            public const string Logout = "/logout";
        }
    }
}