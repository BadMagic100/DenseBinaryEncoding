namespace DenseBinaryEncoding.Data
{
    public class DictionaryTest
    {
        public Dictionary<string, string> d1 = new();

        public Dictionary<IFoo, string> d2 = new();

        public Dictionary<string, IFoo> d3 = new();
    }
}
