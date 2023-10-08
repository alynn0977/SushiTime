namespace Core
{
    using UnityEngine;

    /// <summary>
    /// Provides a method to flip an asset.
    /// Used for objects like character portraits, etc.
    /// </summary>
    public interface IFlippable
    {
        public void FlipAsset();
    }
}