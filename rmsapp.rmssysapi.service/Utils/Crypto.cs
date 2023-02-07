using System;
using System.Linq;
using System.Security.Cryptography;


namespace rmsapp.rmssysapi.service.Utils
{
    public static class Crypto
    {
        private const int HashIterations = 10000;
        private const int HashSaltSize = 32;
        private const int HashKeySize = 24;

        public static (string, byte[]) Hash(string secret)
        {
            return Hash(secret, null);
        }

        public static (string, byte[]) Hash(string secret, byte[] salt)
        {
            if (string.IsNullOrEmpty(secret))
            {
                throw new ArgumentException("The secret to hash must be specified");
            }
            else if (salt != null && salt.Length < HashSaltSize)
            {
                throw new ArgumentOutOfRangeException(
                    $"Custom salts must be at least {HashSaltSize} bytes long");
            }

            var derivedBytes = salt != null ?
                new Rfc2898DeriveBytes(secret, salt, HashIterations) :
                new Rfc2898DeriveBytes(secret, HashSaltSize, HashIterations);

            using (derivedBytes)
            {
                var hash = Convert.ToBase64String(derivedBytes.GetBytes(HashKeySize));
                salt ??= derivedBytes.Salt;

                return (hash, salt);
            }
        }

        public static bool Compare(string givenSecret, string hashedSecret, byte[] salt)
        {
            if (string.IsNullOrEmpty(givenSecret) || string.IsNullOrEmpty(hashedSecret))
            {
                throw new ArgumentException("Both secrets must be specified");
            }
            else if (salt == null || salt.Length < HashSaltSize)
            {
                throw new ArgumentOutOfRangeException(
                    $"The salt must be at least {HashSaltSize} bytes long");
            }

            using var givenSecretBytes = GetDeriveBytes(givenSecret, salt);
            var givenSecretHashed = Convert.ToBase64String(
                givenSecretBytes.GetBytes(HashKeySize));

            return givenSecretHashed.SequenceEqual(hashedSecret);
        }

        private static Rfc2898DeriveBytes GetDeriveBytes(string secret, byte[] salt = null)
        {
            return salt != null ?
                new Rfc2898DeriveBytes(secret, salt, HashIterations) :
                new Rfc2898DeriveBytes(secret, HashSaltSize, HashIterations);
        }
    }
}
