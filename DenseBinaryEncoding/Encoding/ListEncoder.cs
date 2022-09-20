using System.Collections;
using System.Reflection;

namespace DenseBinaryEncoding.Encoding
{
    // encodes a list with 2 leading bytes (a little endian ushort) to denote the number of elements in the list
    public class ListEncoder : IEncoder
    {
        readonly Type collectionType;
        readonly Type elementType;
        readonly IEncoder? elementEncoder;

        public ListEncoder(Type collectionType)
        {
            this.collectionType = collectionType;
            if (collectionType.IsArray)
            {
                elementType = collectionType.GetElementType()!;
            }
            else if (collectionType.IsGenericType && collectionType.GetGenericTypeDefinition() == typeof(List<>))
            {
                elementType = collectionType.GetGenericArguments()[0];
            }
            else
            {
                throw new ArgumentException("Collection was not a valid array or list", nameof(collectionType));
            }
            elementEncoder = EncoderFactory.CreateEncoder(elementType);
        }

        public int GetInputSize(object value)
        {
            IEnumerable values = (IEnumerable)value;
            return 16 + values.Cast<object>().Sum(e => elementEncoder?.GetInputSize(e) ?? 0);
        }

        public int GetOutputSize(BitArray bits, int start)
        {
            BitArray counter = bits.Range(start, 16);
            byte[] bytes = new byte[2];
            counter.CopyTo(bytes, 0);
            if (!BitConverter.IsLittleEndian)
            {
                bytes = bytes.Reverse().ToArray();
            }

            ushort numElements = BitConverter.ToUInt16(bytes, 0);

            int offset = 16;
            if (elementEncoder != null)
            {
                for (int i = 0; i < numElements; i++)
                {
                    offset += elementEncoder.GetOutputSize(bits, start + offset);
                }
            }
            return offset;
        }

        public BitArray GetBits(object value)
        {
            IEnumerable values = (IEnumerable)value;

            ushort len = (ushort)values.Cast<object>().Count();
            byte[] lenBytes = BitConverter.GetBytes(len);
            if (!BitConverter.IsLittleEndian)
            {
                lenBytes = lenBytes.Reverse().ToArray();
            }

            BitArray result = new(lenBytes);

            if (elementEncoder != null)
            {
                foreach (object val in values)
                {
                    int temp = result.Length;
                    BitArray arr = elementEncoder.GetBits(val);
                    result.Length += arr.Length;
                    arr.CopyTo(result, temp);
                }
            }

            return result;
        }

        public object GetValue(BitArray bits, int start)
        {
            BitArray counter = bits.Range(start, 16);
            byte[] bytes = new byte[2];
            counter.CopyTo(bytes, 0);
            if (!BitConverter.IsLittleEndian)
            {
                bytes = bytes.Reverse().ToArray();
            }

            ushort numElements = BitConverter.ToUInt16(bytes, 0);
            Array arr = Array.CreateInstance(elementType, numElements);
            int offset = 16;
            if (elementEncoder != null)
            {
                for (int i = 0; i < numElements; i++)
                {
                    arr.SetValue(elementEncoder.GetValue(bits, start + offset), i);
                    offset += elementEncoder.GetOutputSize(bits, start + offset);
                }
            }

            if (collectionType.IsArray)
            {
                return arr;
            }
            else
            {
                object list = collectionType.GetConstructor(Type.EmptyTypes)!.Invoke(null);
                // runtime complains if we AddRange because Array is not strongly typed
                MethodInfo add = collectionType.GetMethod("Add", new Type[] { elementType })!;
                foreach (object item in arr)
                {
                    add.Invoke(list, new object[] { item });
                }
                return list;
            }
        }
    }
}
