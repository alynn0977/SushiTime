namespace BreakoutSystem
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    // Tracks globals for this particular level instance.
    public class GameZone : MonoBehaviour
    {
        [SerializeField]
        private int playerPowr = 1;

        [SerializeField]
        private BallBehaviour mainBall;

        /// <summary>
        /// Read-only access of current player power stat.
        /// </summary>
        public int PlayerPower => playerPowr;

        private void Start()
        {

            
        }

        private void PlayerPowerIncrease(IncreasePowerEvent e)
        {
            Debug.Log($"Player power increased to {e.SetPower}");
            playerPowr = e.SetPower;
        }

        // TO DO: Update time.
        // TO DO: What are the goals?
    }

}