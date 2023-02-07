using System;
using System.Collections.Generic;
using System.Text;

namespace rmsapp.rmssysapi.service.Utils
{
    public class ClientSecret
    {
        public string Description { get; set; }

        public string Value { get; set; }

        public byte[] Salt { get; set; }

        /// <summary>
        /// Gets or sets when this secret will stop being valid by.
        /// </summary>
        public DateTime? Expiration { get; set; }

        /// <summary>
        /// Gets or sets a hint about what kind of secret this is.
        /// See IdentityServer4's IdentityServerConstants.SecretTypes for options.
        /// </summary>
        public string Type { get; set; } = "SharedSecret";
    }
}
