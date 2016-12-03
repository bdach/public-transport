using System;
using System.Security.Cryptography;

namespace PublicTransport.Services
{
    public interface IPasswordService
    {
        /// <summary>
        ///     Generates a hash for the supplied plaintext passphrase.
        /// </summary>
        /// <param name="plaintext">Plaintext password to generate hash for.</param>
        /// <returns>Hashed and salted password, returned as a base64 string.</returns>
        string GenerateHash(string plaintext);

        /// <summary>
        ///     Compares the supplied plaintext passphrase with the supplied hash.
        /// </summary>
        /// <param name="plaintext">Plaintext password to compare.</param>
        /// <param name="hash">Password hash with salt, encoded as a base64 string.</param>
        /// <returns>True, if password hashes match; false otherwise.</returns>
        bool CompareWithHash(string plaintext, string hash);
    }

    /// <summary>
    ///     Service used for handling passwords and password hashes.
    /// </summary>
    public class PasswordService : IPasswordService
    {
        /// <summary>
        ///     Number of iterations in the PBKDF2 algorithm.
        ///     <see href="https://www.ietf.org/rfc/rfc2898.txt" />
        /// </summary>
        private const int Iterations = 10000;

        /// <summary>
        ///     Length of the generated salt, in bytes.
        /// </summary>
        private const int SaltLength = 16;

        /// <summary>
        ///     Length of the actual password hash, in bytes.
        /// </summary>
        private const int HashLength = 20;

        /// <summary>
        ///     Generates a hash for the supplied plaintext passphrase.
        /// </summary>
        /// <param name="plaintext">Plaintext password to generate hash for.</param>
        /// <returns>Hashed and salted password, returned as a base64 string.</returns>
        public string GenerateHash(string plaintext)
        {
            // TODO: Validation error if password null
            if (plaintext == null) return null;

            var salt = new byte[SaltLength];
            new RNGCryptoServiceProvider().GetBytes(salt);
            var pbkdf2 = new Rfc2898DeriveBytes(plaintext, salt, Iterations);
            var hash = pbkdf2.GetBytes(HashLength);

            var combined = new byte[SaltLength + HashLength];
            Array.Copy(salt, 0, combined, 0, SaltLength);
            Array.Copy(hash, 0, combined, SaltLength, HashLength);
            return Convert.ToBase64String(combined);
        }

        /// <summary>
        ///     Compares the supplied plaintext passphrase with the supplied hash.
        /// </summary>
        /// <param name="plaintext">Plaintext password to compare.</param>
        /// <param name="hash">Password hash with salt, encoded as a base64 string.</param>
        /// <returns>True, if password hashes match; false otherwise.</returns>
        public bool CompareWithHash(string plaintext, string hash)
        {
            var hashBytes = Convert.FromBase64String(hash);
            var salt = new byte[SaltLength];
            Array.Copy(hashBytes, 0, salt, 0, SaltLength);
            var pbkdf2 = new Rfc2898DeriveBytes(plaintext, salt, Iterations);
            var hashedPlaintext = pbkdf2.GetBytes(HashLength);
            var result = true;
            for (var i = 0; i < HashLength; ++i)
            {
                result &= hashBytes[i + SaltLength] == hashedPlaintext[i];
            }
            return result;
        }
    }
}