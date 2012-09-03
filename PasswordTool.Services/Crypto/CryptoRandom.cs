using System;
using System.Security.Cryptography;

namespace PasswordTool.Services.Crypto
{
    public class CryptoRandom : Random
    {
        private readonly RNGCryptoServiceProvider _rng =
            new RNGCryptoServiceProvider();
        private byte[] _uint32Buffer = new byte[4];

        public override int Next()
        {
            _rng.GetBytes(_uint32Buffer);
            return BitConverter.ToInt32(_uint32Buffer, 0) & 0x7FFFFFFF;
        }

        public override int Next(Int32 maxValue)
        {
            if (maxValue < 0)
                throw new ArgumentOutOfRangeException("maxValue");
            return Next(0, maxValue);
        }

        public override int Next(Int32 minValue, Int32 maxValue)
        {
            if (minValue > maxValue)
                throw new ArgumentOutOfRangeException("minValue");
            if (minValue == maxValue) return minValue;
            Int64 diff = maxValue - minValue;
            while (true)
            {
                _rng.GetBytes(_uint32Buffer);
                uint rand = BitConverter.ToUInt32(_uint32Buffer, 0);

                long max = (1 + (long)UInt32.MaxValue);
                long remainder = max % diff;
                if (rand < max - remainder)
                {
                    return (int)(minValue + (rand % diff));
                }
            }
        }

        public override double NextDouble()
        {
            _rng.GetBytes(_uint32Buffer);
            uint rand = BitConverter.ToUInt32(_uint32Buffer, 0);
            return rand / (1.0 + UInt32.MaxValue);
        }

        public override void NextBytes(byte[] buffer)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");
            _rng.GetBytes(buffer);
        }

        public bool NextBoolean()
        {
            var buffer = new byte[1];
            _rng.GetBytes(buffer);

            return buffer[0] % 2 == 0;
        }
    }
}