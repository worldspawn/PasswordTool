using System.Collections.Generic;
using System.Diagnostics;
using Xunit;

namespace PasswordTool.Services.Tests
{
    public class Rfc2988Benchmark
    {
        private readonly List<byte[]> passwords = new List<byte[]>();

        public Rfc2988Benchmark()
        {
            var passwordService = new PasswordService();
            var settings = new PasswordSettings() {Length = 12, MinimumDigits = 2, MaximumDigits = 4, MaximumUpperCase = 4, MinimumUpperCase = 2, MinimumNonAlphaChars = 2, MaximumNonAlphaChars = 4};
            
            while(passwords.Count < 100)
                passwords.Add(passwordService.GeneratePassword(settings));
        }

        [Fact(Skip = "Benchmarks are run manually")]
        public void Run()
        {
            var instance = new Rfc2898HashService();
            var stopWatch = new Stopwatch();
            var passwordPointer = 0;
            for (var i = 0; i < 1000; i++)
            {
                var data = passwords[passwordPointer];
                stopWatch.Start();
                var salt = instance.CreateSalt(8);
                var password = instance.Hash(data, salt, 1);
                stopWatch.Stop();
                if (passwordPointer++ >= passwords.Count - 1)
                    passwordPointer = 0;
            }

            Debug.WriteLine("Total MS: {0}", stopWatch.Elapsed.TotalMilliseconds);
        }
    }
}
