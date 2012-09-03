using System;
using System.Security.Cryptography;

namespace PasswordTool.Services
{
    public abstract class SaltyHashService : IHashService
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

        public abstract byte[] Hash(byte[] data, byte[] salt, int iterations = 1000, int outputLength = 64);
        public abstract bool Verify(byte[] data, byte[] salt, int iterations, byte[] compareTo);
    }
}