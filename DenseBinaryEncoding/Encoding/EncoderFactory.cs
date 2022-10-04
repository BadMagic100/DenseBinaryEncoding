using System.Reflection;

namespace DenseBinaryEncoding.Encoding
{
    public static class EncoderFactory
    {
        // todo if used - we could use the NullableInfoContext API to determine based on nullability annotations whether a type may be null,
        // and thus shave off nesting certain types in NullableEncoder (we'd likely also provide a way to indicate this to those who are not
        // cultured enough to use nullable reference types, e.g. a marker annotation). This approach requires some design changes, however,
        // as with the current approach null annotations would be lost after 1 level of factory construction.
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
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                return new NullableEncoder(new DictEncoder(type));
            }
            else if (type.IsClass)
            {
                return new NullableEncoder(new ObjectEncoder(type));
            }
            else if (type.IsValueType)
            {
                Type? underlyingNullableType = Nullable.GetUnderlyingType(type);
                if (underlyingNullableType != null)
                {
                    // because we deal exclusively in objects, the CLR automatically boxes non-null values as T and null values as null
                    // so we don't actually need NullableEncoder to know or care about the existance of Nullable<T>
                    return new NullableEncoder(CreateEncoder(underlyingNullableType));
                }
                else
                {
                    return new ObjectEncoder(type);
                }
            }
            else
            {
                return null;
            }
        }
    }
}
