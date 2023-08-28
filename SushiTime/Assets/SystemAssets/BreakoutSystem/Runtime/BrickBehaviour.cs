namespace BreakoutSystem
{
    using Core;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Brick behaviour.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class BrickBehaviour : MonoBehaviour, iInteractable
    {

        [SerializeField]
        [Tooltip("How many hits can this break take?")]
        private int health = 1;

        private GameZone gameZone;

        /// <summary>
        /// Read-only access to brick behaviour sprite.
        /// </summary>
        public SpriteRenderer TileSprite => GetComponent<SpriteRenderer>();

        /// <summary>
        /// Read-only access of tile name.
        /// </summary>
        public string TileName => gameObject.name;
        private void Awake()
        {
            if (GetComponentInParent<GameZone>())
            {
                gameZone = GetComponentInParent<GameZone>();

            }
        }

        /// <summary>
        /// Brick interactions 
        /// </summary>
        public void Interact()
        {
            HealthCheck();
        }

        private void HealthCheck()
        {
            if (health >= 100)
            {
                // This brick is unbreakable.
                return;
            }

            if (gameZone)
            {
                health = health - gameZone.PlayerPower;
            }
            else
            {
                health--;
            }

            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

}