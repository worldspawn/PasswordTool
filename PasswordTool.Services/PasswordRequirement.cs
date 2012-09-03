using PasswordTool.Services.Crypto;

namespace PasswordTool.Services
{
    public class PasswordRequirement
    {
        public PasswordRequirement(int required, char[] values)
        {
            _required = required;
            _values = values;
        }

        public PasswordRequirement(int min, int max, char[] values)
            : this(CryptoRandom.Next(min, max + 1), values)
        {
        }

        private static readonly CryptoRandom CryptoRandom = new CryptoRandom();
        private readonly int _required;
        private readonly char[] _values;

        public char[] Values
        {
            get { return _values; }
        }

        public int Used { get; set; }

        public int Required
        {
            get { return _required; }
        }

        public char RandomValue()
        {
            var index = CryptoRandom.Next(0, _values.Length);
            return _values[index];
        }

        public static PasswordRequirement operator |(PasswordRequirement x, PasswordRequirement y)
        {
            if (CryptoRandom.NextBoolean())
                return x;

            return y;
        }
    }
}