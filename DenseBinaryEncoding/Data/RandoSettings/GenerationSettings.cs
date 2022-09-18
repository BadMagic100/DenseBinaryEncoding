namespace DenseBinaryEncoding.Data.RandoSettings
{
    public class GenerationSettings
    {
        public int Seed;

        public TransitionSettings TransitionSettings = new();
        public SkipSettings SkipSettings = new();
        public PoolSettings PoolSettings = new();
        public NoveltySettings NoveltySettings = new();
        public CostSettings CostSettings = new();
        public CursedSettings CursedSettings = new();
        public LongLocationSettings LongLocationSettings = new();
        public StartLocationSettings StartLocationSettings = new();
        public StartItemSettings StartItemSettings = new();
        public MiscSettings MiscSettings = new();
        public ProgressionDepthSettings ProgressionDepthSettings = new();
        public DuplicateItemSettings DuplicateItemSettings = new();
        public SplitGroupSettings SplitGroupSettings = new();
    }
}
