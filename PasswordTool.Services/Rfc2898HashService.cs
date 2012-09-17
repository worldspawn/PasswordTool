using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace PasswordTool.Services
{
    public class Rfc2898HashService : SaltyHashService
    {
        public override byte[] Hash(byte[] data, byte[] salt, int version, int iterations = 1000, int outputLength = 64)
        {
            if (salt.Length < 8)
                throw new ArgumentException("salt length must be at least 8");

            var crypto = new Rfc2898DeriveBytes(data, salt, iterations);
            var hash = crypto.GetBytes(outputLength - 4);
            var result = new byte[4 + hash.Length];
            BitConverter.GetBytes(version).CopyTo(result, 0);
            hash.CopyTo(result, 4);

            return result;
        }

        public override bool Verify(byte[] data, byte[] salt, IDictionary<int, HashParameters> hashParameters, byte[] compareTo)
        {
            var version = ReadVersionFromData(compareTo);
            if (!hashParameters.ContainsKey(version))
                return false;
            IStructuralEquatable output = Hash(data, salt, version, hashParameters[version].Iterations, compareTo.Length);
            return output.Equals(compareTo, StructuralComparisons.StructuralEqualityComparer);
        }

        private int ReadVersionFromData(byte[] data)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (data.Length < 4)
                return 0;

            return BitConverter.ToInt32(data, 0);
        }
    }
}