namespace DenseBinaryEncoding.Encoding
{
    public class SettingsIncludeAttribute : Attribute { }

    public class SettingsExcludeAttribute : Attribute { }

    public class MinValueAttribute : Attribute
    {
        public readonly int Value;
        public MinValueAttribute(int value)
        {
            Value = value;
        }
    }

    public class MaxValueAttribute : Attribute
    {
        public readonly int Value;
        public MaxValueAttribute(int value)
        {
            Value = value;
        }
    }
}
