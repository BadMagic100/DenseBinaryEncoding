using System.Collections;
using System.Reflection;

namespace DenseBinaryEncoding.Encoding
{
    public class ObjectEncoder : IEncoder
    {
        private record EncodableMember(MemberInfo Member, IEncoder Encoder) : IEncoder
        {
            public object? GetMemberValue(object? parent)
            {
                object? result;
                if (Member.MemberType == MemberTypes.Field)
                {
                    result = ((FieldInfo)Member).GetValue(parent);
                }
                else 
                {
                    result = ((PropertyInfo)Member).GetValue(parent);
                }

                return result;
            }

            public void SetMemberValue(object? parent, object? value)
            {
                if (Member.MemberType == MemberTypes.Field)
                {
                    ((FieldInfo)Member).SetValue(parent, value);
                }
                else
                {
                    ((PropertyInfo)Member).SetValue(parent, value);
                }
            }

            public int GetInputSize(object? value)
            {
                return Encoder.GetInputSize(GetMemberValue(value));
            }

            public int GetOutputSize(BitArray bits, int start)
            {
                return Encoder.GetOutputSize(bits, start);
            }

            public BitArray GetBits(object? value)
            {
                return Encoder.GetBits(GetMemberValue(value));
            }

            public object? GetValue(BitArray bits, int start = 0)
            {
                return Encoder.GetValue(bits, start);
            }
        }

        private static bool IsValidMember(MemberInfo member)
        {
            if (member.MemberType == MemberTypes.Field)
            {
                FieldInfo fi = (FieldInfo)member;
                if (fi.IsInitOnly)
                {
                    return false;
                }

                return fi.IsPublic && !Attribute.IsDefined(fi, typeof(SettingsExcludeAttribute))
                    || Attribute.IsDefined(fi, typeof(SettingsIncludeAttribute));
            }
            else if (member.MemberType == MemberTypes.Property)
            {
                PropertyInfo pi = (PropertyInfo)member;
                if (!pi.CanWrite || !pi.CanRead)
                {
                    return false;
                }

                return pi.GetMethod!.IsPublic && !Attribute.IsDefined(pi, typeof(SettingsExcludeAttribute))
                    || Attribute.IsDefined(pi, typeof(SettingsIncludeAttribute));
            }
            else
            {
                return false;
            }
        }

        readonly Type objType;
        readonly List<EncodableMember> members = new();

        public ObjectEncoder(Type objType)
        {
            this.objType = objType;

            foreach (MemberInfo member in objType.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(IsValidMember)
                .OrderBy(mi => mi.Name))
            {
                IEncoder? enc = EncoderFactory.CreateEncoder(member);
                if (enc != null) 
                {
                    members.Add(new EncodableMember(member, enc));
                }
            }
        }

        public int GetInputSize(object? value)
        {
            return members.Sum(em => em.GetInputSize(value));
        }

        public int GetOutputSize(BitArray bits, int start)
        {
            int offset = 0;
            foreach (EncodableMember em in members)
            {
                offset += em.GetOutputSize(bits, start + offset);
            }
            return offset;
        }

        public BitArray GetBits(object? value)
        {
            BitArray result = new(0);
            foreach (EncodableMember em in members)
            {
                int temp = result.Length;
                BitArray m = em.GetBits(value);
                result.Length += m.Length;
                m.CopyTo(result, temp);
            }
            return result;
        }

        public object? GetValue(BitArray bits, int start = 0)
        {
            object result = Activator.CreateInstance(objType)!;

            int offset = 0;
            foreach (EncodableMember em in members)
            {
                object? m = em.GetValue(bits, start + offset);
                offset += em.GetOutputSize(bits, start + offset);
                em.SetMemberValue(result, m);
            }
            return result;
        }
    }
}
