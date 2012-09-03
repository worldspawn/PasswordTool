using System.Diagnostics;
using System.Text;
using Xunit;

namespace PasswordTool.Services.Tests
{
    public class PasswordServiceTests
    {
        [Fact]
        public void Test()
        {
            var fallback = new PasswordRequirement(0, PasswordService.AlphaChars);
            var settings = new PasswordSettings();
            var passwordLength = 20;
            var reqs = new[]
                           {
                               new PasswordRequirement(settings.MinimumNonAlphaChars, settings.MaximumNonAlphaChars, settings.IsUsKeyboard ? PasswordService.UsKeyboardNonAlphas : PasswordService.NonAlphas),
                               new PasswordRequirement(settings.MinimumUpperCase, settings.MaximumUpperCase, PasswordService.UpperAlphaChars),
                               new PasswordRequirement(settings.MinimumDigits, settings.MaximumDigits, PasswordService.DigitChars)
                           };
            var passwordService = new PasswordService();
            byte[] password = passwordService.GeneratePassword(passwordLength, fallback, reqs);
            
            Assert.NotNull(password);
            Assert.Equal(passwordLength, password.Length);

            foreach (PasswordRequirement req in reqs)
            {
                Assert.Equal(req.Required, req.Used);
            }
        }
    }
}