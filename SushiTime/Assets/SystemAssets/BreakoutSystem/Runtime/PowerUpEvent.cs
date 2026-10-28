namespace BreakoutSystem
{
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
        public PowerUpEvent(IPowerUp powerUp, bool isReset = false)
        {
            PowerUp = powerUp;
            IsReset = isReset;
        }

        /// <summary>
        /// If true, calls for a reset of Event Type.
        /// </summary>
        public bool IsReset = false;
    }

}