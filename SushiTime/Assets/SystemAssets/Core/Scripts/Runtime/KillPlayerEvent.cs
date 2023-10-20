namespace Core
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Remove the player's main object, such as in the event
    /// of a gameover.
    /// </summary>
    public class KillPlayerEvent : GameEvent
    {
        /// <summary>
        /// Generic constructor for kill player.
        /// </summary>
        public KillPlayerEvent()
        {

        }
    }
}