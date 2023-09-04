namespace BreakoutSystem
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PowerUpEvent : GameEvent
    {
        /// <summary>
        /// Which power up is associated with this.
        /// </summary>
        public IPowerUp PowerUp;

        /// <summary>
        /// Event to send a power up notification.
        /// </summary>
        /// <param name="powerUp">Powerup Interface.</param>
        public PowerUpEvent(IPowerUp powerUp)
        {
            PowerUp = powerUp;
        }
    }

}