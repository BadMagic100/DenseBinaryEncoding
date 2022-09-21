namespace DenseBinaryEncoding.Data
{
    public interface IFoo
    {
        public string Bar { get; set; }
    }

    public class ConcreteFoo : IFoo
    {
        public string Bar { get; set; }

        public ConcreteFoo(string bar)
        {
            this.Bar = bar;
        }
    }

    public class NoEncoderListTest
    {
        public List<IFoo> foos = new();

        public int? myNum1;
        public int? myNum2;
    }
}
