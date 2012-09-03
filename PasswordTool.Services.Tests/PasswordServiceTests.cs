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
            var reqs = new[]
                           {
                               new PasswordRequirement(settings.MinimumNonAlphaChars, settings.MaximumNonAlphaChars, settings.IsUsKeyboard ? PasswordService.UsKeyboardNonAlphas : PasswordService.NonAlphas),
                               new PasswordRequirement(settings.MinimumUpperCase, settings.MaximumUpperCase, PasswordService.UpperAlphaChars),
                               new PasswordRequirement(settings.MinimumDigits, settings.MaximumDigits, PasswordService.DigitChars)
                           };
            var passwordService = new PasswordService();
            byte[] password = passwordService.GeneratePassword(20, fallback, reqs);

            Debug.WriteLine(Encoding.UTF8.GetString(password));

            foreach (var @byte in password)
            {
                Debug.Write(@byte);
                Debug.Write(" ");
            }
            
            Debug.WriteLine("");

            foreach (PasswordRequirement req in reqs)
            {
                Debug.WriteLine("{0} - {1}", req.Required, req.Used);
                Assert.Equal(req.Required, req.Used);
            }
        }
    }
}