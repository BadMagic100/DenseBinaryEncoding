using System.Collections;

namespace DenseBinaryEncoding.Encoding
{
    public class NullableEncoder : IEncoder
    {
        IEncoder? innerEncoder;

        public NullableEncoder(IEncoder? innerEncoder)
        {
            this.innerEncoder = innerEncoder;
        }

        public int GetInputSize(object? value)
        {
            if (value == null)
            {
                return 1;
            }
            else
            {
                return 1 + innerEncoder?.GetInputSize(value) ?? 0;
            }
        }

        public int GetOutputSize(BitArray bits, int start)
        {
            if (!bits[start])
            {
                return 1;
            }
            else
            {
                return 1 + innerEncoder?.GetOutputSize(bits, start + 1) ?? 0;
            }
        }

        public BitArray GetBits(object? value)
        {
            if (value == null)
            {
                return new BitArray(new bool[] { false });
            }
            else
            {
                BitArray result = new(new bool[] { true });
                BitArray inner = innerEncoder?.GetBits(value) ?? new(0);
                result.Length += inner.Length;
                inner.CopyTo(result, 1);
                return result;
            }
        }

        public object? GetValue(BitArray bits, int start = 0)
        {
            if (bits[start] && innerEncoder != null)
            {
                return innerEncoder.GetValue(bits, start + 1);
            }
            else
            {
                return null;
            }
        }
    }
}
