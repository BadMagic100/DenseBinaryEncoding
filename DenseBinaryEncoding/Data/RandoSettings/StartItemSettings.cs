using DenseBinaryEncoding.Encoding;

namespace DenseBinaryEncoding.Data.RandoSettings
{
    public class StartItemSettings
    {
        [MinValue(0)]
        [MaxValue(50000)]
        public int MinimumStartGeo;

        [MinValue(0)]
        [MaxValue(50000)]
        public int MaximumStartGeo;

        public enum StartVerticalType
        {
            None,
            ZeroOrMore,
            OneRandomItem,
            MantisClaw,
            MonarchWings,
            All,
        }
        public StartVerticalType VerticalMovement;

        public enum StartHorizontalType
        {
            None,
            ZeroOrMore,
            OneRandomItem,
            MothwingCloak,
            CrystalHeart,
            All
        }
        public StartHorizontalType HorizontalMovement;

        public enum StartCharmType
        {
            None,
            ZeroOrMore,
            OneRandomItem,
        }
        public StartCharmType Charms;

        public enum StartStagType
        {
            None,
            DirtmouthStag,
            ZeroOrMoreRandomStags,
            OneRandomStag,
            ManyRandomStags,
            AllStags
        }
        public StartStagType Stags;

        public enum StartMiscItems
        {
            None,
            ZeroOrMore,
            Many,
            DreamNail,
            DreamNailAndMore,
        }
        public StartMiscItems MiscItems;
    }
}
