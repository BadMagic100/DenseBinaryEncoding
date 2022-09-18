using DenseBinaryEncoding.Encoding;

namespace DenseBinaryEncoding.Data.RandoSettings
{
    public class MiscSettings
    {
        public bool RandomizeNotchCosts;
        [MinValue(0)]
        [MaxValue(240)]
        public int MinRandomNotchTotal = 70;
        [MinValue(0)]
        [MaxValue(240)]
        public int MaxRandomNotchTotal = 110;
        public bool ExtraPlatforms;
        public SalubraNotchesSetting SalubraNotches;
        public MaskShardType MaskShards;
        public VesselFragmentType VesselFragments;
        public bool SteelSoul;

        public enum MaskShardType
        {
            FourShardsPerMask,
            TwoShardsPerMask,
            OneShardPerMask
        }

        public enum VesselFragmentType
        {
            ThreeFragmentsPerVessel,
            TwoFragmentsPerVessel,
            OneFragmentPerVessel
        }

        public enum SalubraNotchesSetting
        {
            GroupedWithCharmNotchesPool,
            Vanilla,
            Randomized,
            AutoGivenAtCharmThreshold
        }
    }
}
