using System.Collections;

namespace DenseBinaryEncoding.Encoding
{
    public static class BitExtensions
    {
        public static void CopyTo(this BitArray b, BitArray arr, int start)
        {
            for (int i = 0; i < b.Length; i++)
            {
                arr[start + i] = b[i];
            }
        }

        public static BitArray Range(this BitArray b, int start, int count)
        {
            if (count < 0)
            {
                throw new ArgumentException("Expected non-negative value", nameof(count));
            }

            BitArray result = new(count);
            for (int i = 0; i < count; i++)
            {
                result[i] = b[start + i];
            }
            return result;
        }

        public static BitArray ReadToEnd(this BitArray b, int start)
        {
            return b.Range(start, b.Count - start);
        }

        public static string ToBase64String(this BitArray b)
        {
            byte[] data = new byte[(int)Math.Ceiling(b.Length / 8f)];
            b.CopyTo(data, 0);
            return Convert.ToBase64String(data);
        }

        public static BitArray Base64ToBitArray(this string s)
        {
            byte[] data = Convert.FromBase64String(s);
            return new BitArray(data);
        }
    }
}
