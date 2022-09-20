using System.Collections;

namespace DenseBinaryEncoding.Encoding
{
    public class BoolEncoder : FixedSizeEncoder
    {
        public override int Size => 1;

        public override BitArray GetBits(object value)
        {
            return new BitArray(new bool[] { (bool)value });
        }

        public override object GetValue(BitArray bits, int start = 0)
        {
            BitArray bits2 = bits.Range(start, 1);
            bool[] bools = new bool[1];
            bits2.CopyTo(bools, 0);
            return bools[0];
        }
    }
}
