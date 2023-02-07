using System;
using System.Security.Cryptography;


namespace rmsapp.rmssysapi.service.Utils
{
    public static class Secrets
    {
        private static readonly RandomNumberGenerator RandomNumberGenerator =
            RandomNumberGenerator.Create();

        public static int ClientSecretExpiration { get; set; }

        public static (string, DateTime, ClientSecret) Create(string description = null)
        {
            return Create(DateTime.UtcNow.AddDays(ClientSecretExpiration), description);
        }

        public static (string, DateTime, ClientSecret) Create(
            DateTime expiration, string description = null)
        {
            var secret = GenerateClientSecret();
            var (hash, salt) = Crypto.Hash(secret);
            var newClientSecret =
                new ClientSecret
                {
                    Description = description,
                    Expiration = expiration,
                    Salt = salt,
                    Value = hash,
                };

            return (secret, expiration, newClientSecret);
        }

        private static string GenerateClientSecret()
        {
            var bytes = new byte[64];
            RandomNumberGenerator.GetNonZeroBytes(bytes);

            return Convert.ToBase64String(bytes);
        }
    }
}
