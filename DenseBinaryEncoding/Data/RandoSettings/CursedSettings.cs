using DenseBinaryEncoding.Encoding;

namespace DenseBinaryEncoding.Data.RandoSettings
{
    public class CursedSettings
    {
        public bool LongerProgressionChains;
        public bool ReplaceJunkWithOneGeo;
        public bool RemoveSpellUpgrades;
        public bool Deranged;
        [MinValue(0)]
        [MaxValue(4)]
        public int CursedMasks;
        [MinValue(0)]
        [MaxValue(4)]
        public int CursedNotches;
        public bool RandomizeMimics;
        [MinValue(0)]
        public int MaximumGrubsReplacedByMimics;
    }
}
