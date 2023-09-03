namespace BreakoutSystem
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ChangeLivesEvent : GameEvent
    {
        /// <summary>
        /// How much to add to the score
        /// </summary>
        public int LivesAmount;

        /// <summary>
        /// Event to change the score.
        /// </summary>
        /// <param name="ScoreAmount">Ask to confirm first.</param>
        public ChangeLivesEvent(int LivesAmount)
        {
            this.LivesAmount = LivesAmount;
        }
    }

}