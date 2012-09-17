using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PasswordTool.Services.Tests
{
    public class HashServiceTests
    {
        private IDictionary<int, HashParameters> _hashParameters = new Dictionary<int, HashParameters>()
                                                                   {
                                                                       {1, new HashParameters {Version = 1, Iterations = 1000}},
                                                                       {2, new HashParameters {Version = 2, Iterations = 10000}}
                                                                   };

        [Fact]
        public void HashServiceProducesValidSalt()
        {
            var hashService = new Rfc2898HashService();
            var salt = hashService.CreateSalt(16);

            Assert.NotNull(salt);
            Assert.Equal(16, salt.Length);
        }

        [Fact]
        public void CanHashAndVerify()
        {
            const string testphrase = "Dr. DoLittle"; 
            var hashService = new Rfc2898HashService();
            var salt = hashService.CreateSalt(16);
            var testphrasebytes = Encoding.UTF8.GetBytes(testphrase);
            var hash = hashService.Hash(testphrasebytes, salt, 1, _hashParameters[1].Iterations, 64);

            Assert.NotNull(hash);
            Assert.Equal(64, hash.Length);

            var confirmation = hashService.Verify(testphrasebytes, salt, _hashParameters, hash);
            
            Assert.True(confirmation);
        }
    }
}
