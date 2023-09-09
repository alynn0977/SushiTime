namespace Core
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Sends signal of a PauseGame Event.
    /// </summary>
    public class PauseGameEvent : GameEvent
    {
        /// <summary>
        /// Is the game paused or not.
        /// </summary>
        public bool IsPause;

        /// <summary>
        /// Game Event to send signal of a pause event.
        /// </summary>
        /// <param name="isPause">If true, is paused.</param>
        public PauseGameEvent(bool isPause)
        {
            this.IsPause = isPause;
        }
    }

}