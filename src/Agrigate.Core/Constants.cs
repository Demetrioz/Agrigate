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
        public const string Configuration = "Logging";
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