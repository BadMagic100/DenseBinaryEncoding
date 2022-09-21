namespace DenseBinaryEncoding.Encoding
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SettingsIncludeAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SettingsExcludeAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class MinValueAttribute : Attribute
    {
        public readonly int Value;
        public MinValueAttribute(int value)
        {
            Value = value;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class MaxValueAttribute : Attribute
    {
        public readonly int Value;
        public MaxValueAttribute(int value)
        {
            Value = value;
        }
    }
}
