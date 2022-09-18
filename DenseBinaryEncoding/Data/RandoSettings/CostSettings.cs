using DenseBinaryEncoding.Encoding;

namespace DenseBinaryEncoding.Data.RandoSettings
{
    public class CostSettings
    {
        [MinValue(0)]
        [MaxValue(46)]
        public int MinimumGrubCost { get; set; }

        [MinValue(0)]
        [MaxValue(46)]
        public int MaximumGrubCost;

        [MinValue(0)]
        [MaxValue(46)]
        public int GrubTolerance;
        private int GrubToleranceUB => 46 - MaximumGrubCost;


        [MinValue(0)]
        [MaxValue(2800)]
        public int MinimumEssenceCost;

        [MinValue(0)]
        [MaxValue(2800)]
        public int MaximumEssenceCost;

        [MinValue(0)]
        [MaxValue(250)]
        public int EssenceTolerance;


        [MinValue(0)]
        [MaxValue(21)]
        public int MinimumEggCost;

        [MinValue(0)]
        [MaxValue(21)]
        public int MaximumEggCost;

        [MinValue(0)]
        [MaxValue(21)]
        public int EggTolerance;
        private int EggToleranceUB => 21 - MaximumEggCost;


        [MinValue(0)]
        [MaxValue(40)]
        public int MinimumCharmCost;

        [MinValue(0)]
        [MaxValue(40)]
        public int MaximumCharmCost;

        [MinValue(0)]
        [MaxValue(40)]
        public int CharmTolerance;
        private int CharmToleranceUB => 40 - MaximumCharmCost;

    }
}
