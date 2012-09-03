using System.Linq;
using System.Text;
using PasswordTool.Services.Crypto;

namespace PasswordTool.Services
{
    public class PasswordService : IPasswordService
    {
        public static readonly char[] UsKeyboardNonAlphas = "~!@#$%^&*()_+/".ToCharArray();
        public static readonly char[] NonAlphas = "~!@#£%^&*()_+/".ToCharArray();
        public static readonly char[] AlphaChars = "abcdefghijklmnopqrstuvqwxyz".ToCharArray();
        public static readonly char[] UpperAlphaChars = "abcdefghijklmnopqrstuvqwxyz".ToUpper().ToCharArray();
        public static readonly char[] DigitChars = "0123456789".ToCharArray();
        private readonly CryptoRandom _cryptoRandom = new CryptoRandom();

        public byte[] GeneratePassword()
        {
            return GeneratePassword(new PasswordSettings());
        }

        public byte[] GeneratePassword(PasswordSettings settings)
        {
            var fallback = new PasswordRequirement(0, AlphaChars);
            var password = GeneratePassword(settings.Length, fallback,
                                            new PasswordRequirement(settings.MinimumNonAlphaChars, settings.MaximumNonAlphaChars, settings.IsUsKeyboard ? UsKeyboardNonAlphas : NonAlphas),
                                            new PasswordRequirement(settings.MinimumUpperCase, settings.MaximumUpperCase, UpperAlphaChars),
                                            new PasswordRequirement(settings.MinimumDigits, settings.MaximumDigits, DigitChars)
                );

            return password;
        }

        public byte[] GeneratePassword(int passwordLength, PasswordRequirement fallbackChars, params PasswordRequirement[] reqs)
        {
            var indexProvider = _cryptoRandom;
            var password = new byte[passwordLength];
            var indexes = Enumerable.Range(0, passwordLength).ToList();

            for (var i = 0; i < passwordLength; i++)
            {
                var indexPointer = indexProvider.Next(0, indexes.Count);
                var index = indexes[indexPointer];
                indexes.RemoveAt(indexPointer);
                var actual = reqs.Sum(z => z.Required - z.Used);
                var fallback = true;
                foreach (var req in reqs)
                {
                    var result = Rand(req.Used, req.Required, (passwordLength - i) - actual - req.Required - req.Used);
                    if (result)
                    {
                        var value = req.RandomValue();
                        
                        password[index] = Encoding.UTF8.GetBytes(new[] { value }, 0, 1)[0];
                        req.Used++;
                        fallback = false;
                        break;
                    }
                }

                if (fallback)
                    password[index] = (byte)fallbackChars.RandomValue();
            }

            return password;
        }

        private bool Rand(int used, int required, int chancesLeft)
        {
            const decimal threshold = (300 * 0.5m);
            decimal stillNeed = required - used;
            if (stillNeed <= 0)
                return false;
            if (stillNeed >= chancesLeft)
                return true;

            var result = _cryptoRandom.Next(0, 300) > threshold;
            return result;
        }
    }
}