namespace BreakoutSystem
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Increase the power to the ball.
    /// </summary>
    public class IncreasePowerEvent : GameEvent
    {
        /// <summary>
        /// What power level should the game be at?
        /// </summary>
        public int SetPower { get; set; }

        /// <summary>
        /// Set the player power at an exact number.
        /// </summary>
        /// <param name="setPower">The number to set the power too.</param>
        public IncreasePowerEvent(int setPower)
        {
            this.SetPower = setPower;
        }

    }

}