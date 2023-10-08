namespace DialogueSystem
{
    using Core;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public interface IPortrait : IFlippable
    {
        /// <summary>
        /// Get the Rect Transform of the character.
        /// </summary>
        public RectTransform CharacterRect { get;}
        /// <summary>
        /// Get the rect transform of the portrait object.
        /// </summary>
        public RectTransform PortraitRect { get; }

        /// <summary>
        /// Set the character art, size, and offset.
        /// </summary>
        /// <param name="sprite">Sprite to set character too.</param>
        /// <param name="size">Size to set sprite too.</param>
        /// <param name="offset">Offset to set sprite too.</param>
        public void SetCharactertArt(Sprite sprite, Vector2 size, Vector3 offset);

        public void FlipCharacterArt();
    }
}