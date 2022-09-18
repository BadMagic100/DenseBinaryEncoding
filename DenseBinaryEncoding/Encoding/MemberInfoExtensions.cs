using System.Reflection;

namespace DenseBinaryEncoding.Encoding
{
    public static class MemberInfoExtensions
    {
        public static Type GetMemberDataType(this MemberInfo mem)
        {
            if (mem.MemberType == MemberTypes.Field)
            {
                return ((FieldInfo)mem).FieldType;
            }
            else if (mem.MemberType == MemberTypes.Property)
            {
                return ((PropertyInfo)mem).PropertyType;
            }
            else
            {
                throw new ArgumentException("Invalid member type", nameof(mem));
            }
        }
    }
}
