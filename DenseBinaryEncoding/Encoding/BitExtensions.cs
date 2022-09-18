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
    }
}
