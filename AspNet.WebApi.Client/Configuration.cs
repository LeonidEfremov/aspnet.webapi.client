using System;

namespace AspNet.WebApi
{
    /// <summary>Crypterium API Client Configuration.</summary>
    public abstract class Configuration
    {
        /// <summary>Service Endpoint.</summary>
        public Uri Endpoint { get; set; }
    }
}
