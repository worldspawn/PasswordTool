using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace PasswordTool.Services
{
    public class MD5HashService : SaltyHashService
    {
        public override byte[] Hash(byte[] data, byte[] salt, int version, int iterations = 1000, int outputLength = 64)
        {
            var crypto = new MD5CryptoServiceProvider();
            var output = new byte[outputLength];
            crypto.TransformBlock(data, 0, data.Length, output, 0);

            salt.CopyTo(output, output.Length - salt.Length);
            return output;
        }

        public override bool Verify(byte[] data, byte[] salt, IDictionary<int, HashParameters> hashParameters, byte[] compareTo)
        {
            IStructuralEquatable output = Hash(data, salt, 0, compareTo.Length);
            return output.Equals(compareTo, StructuralComparisons.StructuralEqualityComparer);
        }
    }
}