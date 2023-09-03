namespace BreakoutSystem
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ChangeScoreEvent : GameEvent
    {
        /// <summary>
        /// How much to add to the score
        /// </summary>
        public int ScoreAmount;

        /// <summary>
        /// Event to change the score.
        /// </summary>
        /// <param name="ScoreAmount">Ask to confirm first.</param>
        public ChangeScoreEvent(int ScoreAmount)
        {
            this.ScoreAmount = ScoreAmount;
        }
    }

}