using System.Collections;

namespace DenseBinaryEncoding.Encoding
{
    public class FloatingPointEncoder : FixedSizeEncoder
    {
        private readonly int size;
        public override int Size => size;

        public FloatingPointEncoder(Type t)
        {
            if (t == typeof(float))
            {
                size = 32;
            }
            else if (t == typeof(double))
            {
                size = 64;
            }
            else
            {
                throw new ArgumentException("Expected float type", nameof(t));
            }
        }

        public override BitArray GetBits(object value)
        {
            byte[] bytes;
            if (size == 32)
            {
                bytes = BitConverter.GetBytes((float)value);
            }
            else
            {
                bytes = BitConverter.GetBytes((double)value);
            }

            if (!BitConverter.IsLittleEndian)
            {
                bytes = bytes.Reverse().ToArray();
            }
            return new BitArray(bytes);
        }

        public override object GetValue(BitArray bits, int start = 0)
        {
            BitArray bits2 = bits.Range(start, size);
            byte[] bytes = new byte[size / 8];
            bits2.CopyTo(bytes, 0);

            if (!BitConverter.IsLittleEndian)
            {
                bytes = bytes.Reverse().ToArray();
            }
            
            if (size == 32)
            {
                return BitConverter.ToSingle(bytes, 0);
            }
            else
            {
                return BitConverter.ToDouble(bytes, 0);
            }
        }
    }
}
