namespace BreakoutSystem
{
    using Core;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Brick behaviour.
    /// </summary>
    public class BrickBehaviour : MonoBehaviour, iInteractable
    {

        [SerializeField]
        [Tooltip("How many hits can this break take?")]
        private int health = 1;

        private GameZone gameZone;

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