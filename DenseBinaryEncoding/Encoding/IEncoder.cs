using System.Collections;

namespace DenseBinaryEncoding.Encoding
{
    public interface IEncoder
    {
        /// <summary>
        /// Encodes a value as bits
        /// </summary>
        BitArray GetBits(object value);

        /// <summary>
        /// Reads a value from the data buffer starting at the given index
        /// </summary>
        object GetValue(BitArray bits, int start = 0);

        /// <summary>
        /// Calculates the size in bits needed to encode the value using the value that will be encoded
        /// </summary>
        int GetInputSize(object value);

        /// <summary>
        /// Calculates the size in bits to be consumed from the data buffer when decoding a value, using the data buffer and a starting index
        /// </summary>
        int GetOutputSize(BitArray bits, int start);
    }

    public abstract class FixedSizeEncoder : IEncoder
    {
        public abstract int Size { get; }
        public abstract BitArray GetBits(object value);
        public abstract object GetValue(BitArray bits, int start);
        public int GetInputSize(object value) => Size;
        public int GetOutputSize(BitArray bits, int start) => Size;
    }
}
