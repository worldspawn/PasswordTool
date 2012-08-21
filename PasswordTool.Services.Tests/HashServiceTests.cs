using System.Text;
using Xunit;

namespace PasswordTool.Services.Tests
{
    public class HashServiceTests
    {
        [Fact]
        public void HashServiceProducesValidSalt()
        {
            var hashService = new HashService();
            var salt = hashService.CreateSalt(16);

            Assert.NotNull(salt);
            Assert.Equal(16, salt.Length);
        }

        [Fact]
        public void CanHashAndVerify()
        {
            const string testphrase = "Dr. DoLittle"; 
            var hashService = new HashService();
            var salt = hashService.CreateSalt(16);
            var testphrasebytes = Encoding.UTF8.GetBytes(testphrase);
            var hash = hashService.Hash(testphrasebytes, salt, 1000, 64);

            Assert.NotNull(hash);
            Assert.Equal(64, hash.Length);
            
            var confirmation = hashService.Verify(testphrasebytes, salt, 1000, hash);
            
            Assert.True(confirmation);
        }
    }
}
