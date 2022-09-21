using System.Collections;

namespace DenseBinaryEncoding.Encoding
{
    public class DictEncoder : IEncoder
    {
        readonly Type collectionType;
        readonly Type keyType;
        readonly Type valueType;
        readonly IEncoder? keyEncoder;
        readonly IEncoder? valueEncoder;

        public DictEncoder(Type collectionType)
        {
            this.collectionType = collectionType;
            Type[] typeParams = collectionType.GetGenericArguments();
            keyType = typeParams[0];
            valueType = typeParams[1];
            keyEncoder = EncoderFactory.CreateEncoder(keyType);
            valueEncoder = EncoderFactory.CreateEncoder(valueType);
        }

        public int GetInputSize(object? value)
        {
            IDictionary dict = (IDictionary)value!;
            return 16 + dict.Keys.Cast<object>().Sum(k =>
            {
                if (keyEncoder != null)
                {
                    int keySize = keyEncoder.GetInputSize(k);
                    int valueSize = 0;
                    if (valueEncoder != null)
                    {
                        valueSize = valueEncoder.GetInputSize(dict[k]);
                    }
                    return keySize + valueSize;
                }
                return 0;
            });
        }

        public int GetOutputSize(BitArray bits, int start)
        {
            BitArray lenBits = bits.Range(start, 16);
            byte[] lenBytes = new byte[2];
            lenBits.CopyTo(lenBytes, 0);
            if (!BitConverter.IsLittleEndian)
            {
                lenBytes = lenBytes.Reverse().ToArray();
            }
            ushort len = BitConverter.ToUInt16(lenBytes, 0);

            int offset = 16;
            if (keyEncoder != null)
            {
                for (int i = 0; i < len; i++)
                {
                    offset += keyEncoder.GetOutputSize(bits, start + offset);
                    if (valueEncoder != null)
                    {
                        offset += valueEncoder.GetOutputSize(bits, start + offset);
                    }
                }
            }
            return offset;
        }

        public BitArray GetBits(object? value)
        {
            IDictionary dict = (IDictionary)value!;

            ushort len = (ushort)dict.Count;
            byte[] lenBytes = BitConverter.GetBytes(len);
            if (!BitConverter.IsLittleEndian)
            {
                lenBytes = lenBytes.Reverse().ToArray();
            }

            BitArray result = new BitArray(lenBytes);
            if (keyEncoder != null)
            {
                foreach (object key in dict.Keys)
                {
                    int temp = result.Length;
                    BitArray arrK = keyEncoder.GetBits(key);
                    result.Length += arrK.Length;
                    arrK.CopyTo(result, temp);
                    if (valueEncoder != null)
                    {
                        temp = result.Length;
                        BitArray arrV = valueEncoder.GetBits(dict[key]);
                        result.Length += arrV.Length;
                        arrV.CopyTo(result, temp);
                    }
                }
            }
            return result;
        }

        public object? GetValue(BitArray bits, int start = 0)
        {
            BitArray lenBits = bits.Range(start, 16);
            byte[] lenBytes = new byte[2];
            lenBits.CopyTo(lenBytes, 0);
            if (!BitConverter.IsLittleEndian)
            {
                lenBytes = lenBytes.Reverse().ToArray();
            }
            ushort len = BitConverter.ToUInt16(lenBytes, 0);

            IDictionary dict = (IDictionary)Activator.CreateInstance(collectionType)!;
            int offset = 16;
            if (keyEncoder != null)
            {
                for (int i = 0; i < len; i++)
                {
                    object? key = keyEncoder.GetValue(bits, start + offset);
                    offset += keyEncoder.GetOutputSize(bits, start + offset);
                    object? value = null;
                    if (valueEncoder != null)
                    {
                        value = valueEncoder.GetValue(bits, start + offset);
                        offset += valueEncoder.GetOutputSize(bits, start + offset);
                    }
                    else
                    {
                        if (valueType.IsValueType)
                        {
                            value = Activator.CreateInstance(valueType);
                        }
                    }
                    // null is not a valid dict key (unlike java)
                    if (key != null)
                    {
                        dict[key] = value;
                    }
                }
            }
            return dict;
        }
    }
}
