namespace DenseBinaryEncoding.Data.RandoSettings
{
    public class StartLocationSettings
    {
        public enum RandomizeStartLocationType
        {
            Fixed,
            RandomExcludingKP,
            Random,
        }

        public RandomizeStartLocationType StartLocationType;

        public string? StartLocation;

        public void SetStartLocation(string start) => StartLocation = start;
    }
}
