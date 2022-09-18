namespace DenseBinaryEncoding.Data.RandoSettings
{
    /// <summary>
    /// Parameter which determines the function applied to the location depth.
    /// </summary>
    public enum TransformType
    {
        Linear,
        Quadratic,
        SquareRoot,
        Logarithmic
    }

    /// <summary>
    /// Parameter which determines how location depth should be adjusted according to item priority depth.
    /// </summary>
    public enum ItemPriorityDepthEffect
    {
        /// <summary>
        /// Cancel priority transform if item priority depth exceeds location depth.
        /// </summary>
        Cliff,
        /// <summary>
        /// Adjust location depth to fade linearly to 0 when greater than item priority depth.
        /// </summary>
        Fade,
        /// <summary>
        /// Clamp location depth to item priority depth as an upper bound.
        /// </summary>
        Cap,
        /// <summary>
        /// Item priority depth has no effect.
        /// </summary>
        Ignore,
    }

    public class ProgressionDepthSettings
    {
        public bool MultiLocationPenalty = true;
        public bool DuplicateItemPenalty = true;

        public TransformType LocationPriorityTransformType = TransformType.Linear;
        public ItemPriorityDepthEffect ItemLocationPriorityInteraction = ItemPriorityDepthEffect.Cliff;
        public float LocationPriorityTransformCoefficient = 3f;

        public TransformType TransitionPriorityTransformType = TransformType.SquareRoot;
        public ItemPriorityDepthEffect TransitionTransitionPriorityInteraction = ItemPriorityDepthEffect.Cliff;
        public float TransitionPriorityTransformCoefficient = 1f;
    }
}
