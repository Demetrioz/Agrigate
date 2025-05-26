namespace Agrigate.Core;

/// <summary>
/// Constants used across Agrigate
/// </summary>
public class Constants
{
    /// <summary>
    /// Configuration related to Agrigate
    /// </summary>
    public class Agrigate
    {
        /// <summary>
        /// The configuration section containing agrigate related setup
        /// </summary>
        public const string Configuration = "Agrigate";
    }
    
    /// <summary>
    /// Configuration related to Logging
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
}