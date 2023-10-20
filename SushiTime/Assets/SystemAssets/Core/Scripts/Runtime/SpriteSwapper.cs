namespace Core
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    /// <summary>
    /// Swaps between a state of sprites.
    /// </summary>
    public class SpriteSwapper : MonoBehaviour
    {
        [SerializeField]
        private Sprite[] spriteStates;

        [SerializeField]
        private SpriteRenderer spriteTarget;

        [SerializeField]
        private SpriteStartState startingState = SpriteStartState.last;

        [SerializeField]
        private int spriteIndex;

        /// <summary>
        /// Go to a specific sprite.
        /// </summary>
        /// <param name="newIndex"></param>
        public void SwapSprite(int newIndex)
        {
            if (HasSprite() && newIndex >= 0 && newIndex < spriteStates.Length)
            {
                spriteTarget.sprite = spriteStates[newIndex];
                spriteIndex = newIndex;
            }
            else
            {
                Debug.LogWarning($"[{GetType().Name}]: {gameObject.name} failed to swap sprite.");
            }
        }

        /// <summary>
        /// Go one forward in sprite series.
        /// </summary>
        [ContextMenu("Next Sprite")]
        public void NextSprite()
        {
            if (spriteIndex == spriteStates.Length - 1)
            {
                // At the end.
                return;
            }

            var newIndex = spriteIndex + 1;
            SwapSprite(newIndex);
        }

        /// <summary>
        /// Go one back in sprite series.
        /// </summary>
        [ContextMenu("Prior Sprite")]
        public void PriorSprite()
        {
            if (spriteIndex == 0)
            {
                // Can't go back.
                return;
            }

            var newIndex = spriteIndex-1;
            SwapSprite(newIndex);
        }

        private void OnEnable()
        {
            if (!HasSprite())
            {
                Debug.LogWarning($"[{GetType().Name}]: {gameObject.name} sprite target is null.");
            }

            InitializeStartingState();
        }

        private void InitializeStartingState()
        {
            switch (startingState)
            {
                case SpriteStartState.first:
                    spriteIndex = 0;
                    SwapSprite(spriteIndex);
                    break;
                case SpriteStartState.last:
                    spriteIndex = spriteStates.Length - 1;
                    SwapSprite(spriteIndex);
                    break;
            }
        }

        private bool HasSprite()
        {
            return spriteTarget != null;
        }

        private enum SpriteStartState
        {
            /// <summary>
            /// Start sprite on first in series.
            /// </summary>
            first = 0,

            /// <summary>
            /// Start sprite on last in series.
            /// </summary>
            last = 1,
        }
    }

}