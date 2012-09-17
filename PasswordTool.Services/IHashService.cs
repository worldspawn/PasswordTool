using System.Collections.Generic;

namespace PasswordTool.Services
{
    public interface IHashService
    {
        byte[] CreateSalt(int saltLength);
        byte[] Hash(byte[] data, byte[] salt, int version, int iterations = 1000, int outputLength = 64);
        bool Verify(byte[] data, byte[] salt, IDictionary<int, HashParameters> hashParameters, byte[] compareTo);
    }
}