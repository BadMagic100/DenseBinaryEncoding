using System.Collections;
using System.Reflection;

namespace DenseBinaryEncoding.Encoding
{
    public class ConstrainedIntEncoder : FixedSizeEncoder
    {
        // not supported: uint, long, ulong
        public static readonly IReadOnlyDictionary<Type, (int min, int max)> intTypes = new Dictionary<Type, (int, int)>()
        {
            [typeof(byte)] = (byte.MinValue, byte.MaxValue),
            [typeof(sbyte)] = (sbyte.MinValue, sbyte.MaxValue),
            [typeof(short)] = (short.MinValue, short.MaxValue),
            [typeof(ushort)] = (ushort.MinValue, ushort.MaxValue),
            [typeof(int)] = (int.MinValue, int.MaxValue)
        };

        

        public override int Size => (int)Math.Ceiling(Math.Log2(range));

        readonly Type fieldType;
        readonly int min, max;
        readonly ulong range;

        public ConstrainedIntEncoder(Type type)
        {
            fieldType = type;
            if (!intTypes.ContainsKey(type) && !type.IsEnum)
            {
                throw new ArgumentException("Expected an integer type smaller in magnitude than uint", nameof(type));
            }

            if (type.IsEnum)
            {
                min = Enum.GetValues(type).Cast<int>().Min();
            }
            else
            {
                min = intTypes[type].min;
            }

            if (type.IsEnum)
            {
                max = Enum.GetValues(type).Cast<int>().Max();
            }
            else
            {
                max = intTypes[type].max;
            }

            // do a progressive step-up to allow for the possibility of overflow - first wrap around to uint.max if needed, then get more space to add 1
            range = (ulong)(uint)(max - min) + 1;
        }

        public ConstrainedIntEncoder(MemberInfo mem) : this(mem.GetMemberDataType())
        {
            // check for further constraint on the fields as defined by attributes
            if (mem.GetCustomAttribute<MinValueAttribute>() is MinValueAttribute miva)
            {
                min = miva.Value;
            }
            if (mem.GetCustomAttribute<MaxValueAttribute>() is MaxValueAttribute mava)
            {
                max = mava.Value;
            }
            // recompute range
            range = (ulong)(uint)(max - min) + 1;
        }

        public override BitArray GetBits(object value)
        {
            uint constrained = (uint)((int)value - min);
            byte[] data = BitConverter.GetBytes(constrained);
            // force the LSB first
            if (!BitConverter.IsLittleEndian)
            {
                data = data.Reverse().ToArray();
            }
            BitArray bits = new(data)
            {
                // truncate trailing garbage
                Length = Size
            };
            return bits;
        }

        public override object GetValue(BitArray bits, int start)
        {
            BitArray bits2 = bits.Range(start, Size);
            byte[] data = new byte[4];
            bits2.CopyTo(data, 0);
            // get back from little endian to native byte order
            if (!BitConverter.IsLittleEndian)
            {
                data = data.Reverse().ToArray();
            }
            int val = BitConverter.ToInt32(data, 0);
            val += min;
            // cast back to the original field type
            if (fieldType.IsEnum)
            {
                return Enum.ToObject(fieldType, val);
            }
            else
            {
                return Convert.ChangeType(val, fieldType);
            }
        }
    }
}
