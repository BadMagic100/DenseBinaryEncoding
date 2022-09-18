using System.Collections;

namespace DenseBinaryEncoding.Encoding
{
    public class StringEncoder : IEncoder
    {
        public int GetInputSize(object value)
        {
            return 8 * (((string)value).Length + 1);
        }

        public int GetOutputSize(BitArray bits, int start)
        {
            BitArray bits2 = bits.ReadToEnd(start);
            byte[] chars = new byte[bits2.Count / 8 + 1];
            bits2.CopyTo(chars, 0);

            int count;
            for (count = 0; count < chars.Length; count++)
            {
                if (chars[count] == 0)
                {
                    break;
                }
            }
            return 8 * (count + 1);
        }

        public BitArray GetBits(object value)
        {
            string finalValue = value + "\0";
            return new BitArray(System.Text.Encoding.UTF8.GetBytes(finalValue));
        }

        public object GetValue(BitArray bits, int start)
        {
            BitArray bits2 = bits.ReadToEnd(start);
            byte[] chars = new byte[bits2.Count / 8 + 1];
            bits2.CopyTo(chars, 0);

            List<byte> bytes = new();
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] == 0)
                {
                    break;
                }
                bytes.Add(chars[i]);
            }
            return System.Text.Encoding.UTF8.GetString(bytes.ToArray());
        }
    }
}
