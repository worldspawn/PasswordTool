using System;
using System.Collections;
using System.Security.Cryptography;

namespace PasswordTool.Services
{
    public class HashService : IHashService
    {
        private readonly RNGCryptoServiceProvider _rngCryptoServiceProvider = new RNGCryptoServiceProvider();

        public byte[] CreateSalt(int saltLength)
        {
            if (saltLength < 8)
                throw new ArgumentException("saltLength must be at least 8");
            var salt = new byte[saltLength];
            _rngCryptoServiceProvider.GetBytes(salt);
            return salt;
        }

        public byte[] Hash(byte[] data, byte[] salt, int iterations = 1000, int outputLength = 64)
        {
            if (salt.Length < 8)
                throw new ArgumentException("salt length must be at least 8");

            var crypto = new Rfc2898DeriveBytes(data, salt, iterations);
            var result = crypto.GetBytes(outputLength);

            return result;
        }

        public bool Verify(byte[] data, byte[] salt, int iterations, byte[] compareTo)
        {
            IStructuralEquatable output = Hash(data, salt, iterations, compareTo.Length);
            return output.Equals(compareTo, StructuralComparisons.StructuralEqualityComparer);
        }
    }
}