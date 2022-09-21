using System.Reflection;

namespace DenseBinaryEncoding.Encoding
{
    public static class EncoderFactory
    {
        public static IEncoder? CreateEncoder(MemberInfo member)
        {
            Type t = member.GetMemberDataType();
            if (t.IsEnum || ConstrainedIntEncoder.intTypes.ContainsKey(t))
            {
                return new ConstrainedIntEncoder(member);
            }
            else
            {
                return CreateEncoder(t);
            }
        }

        public static IEncoder? CreateEncoder(Type type)
        {
            if (type.IsEnum || ConstrainedIntEncoder.intTypes.ContainsKey(type))
            {
                return new ConstrainedIntEncoder(type);
            }
            else if (type == typeof(bool))
            {
                return new BoolEncoder();
            }
            else if (type == typeof(float) || type == typeof(double))
            {
                return new FloatingPointEncoder(type);
            }
            else if (type == typeof(string))
            {
                return new NullableEncoder(new StringEncoder());
            }
            else if (type.IsArray || type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                return new NullableEncoder(new ListEncoder(type));
            }
            else if (type.IsClass)
            {
                return new NullableEncoder(new ObjectEncoder(type));
            }
            else if (type.IsValueType)
            {
                return new ObjectEncoder(type);
            }
            else
            {
                return null;
            }
        }
    }
}
