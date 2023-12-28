namespace BreakoutSystem
{
    using System.Drawing;
    using System.Net.Sockets;
    using UnityEngine;
    using UnityEngine.SocialPlatforms.Impl;

    /// <summary>
    /// Tallies the final score and provides bonuses.
    /// Passes these numbers to the <see cref="GameManager"/>.
    /// TO DO: Provide a shop for players to buy upgrades with.
    /// </summary>
    public class WinScreen : MonoBehaviour
    {
        private int score = 0;
        private float time = 0f;
        private GoalKeeping goal = default;
        private int timeBonus = 0;
        private int finalScore;

        public void InitializeWinScreen(int score, float time, GoalKeeping goal)
        {
            if (goal == null)
            {
                Debug.LogWarning($"[{GetType().Name}]: Null GoalKeeping data. Default scoring will be used.");

                // Proceed to backup rounding method.
                return;
            }

            Debug.Log($"[{GetType().Name}]: Initialized with score: {score}, time {time}, {goal}");
            this.score = score;
            this.time = time;
            this.goal = goal;

            if (goal.CurrentGoal == GoalKeeping.GoalType.TileGoal || goal.CurrentGoal == GoalKeeping.GoalType.ClearAll)
            {
                // Check the time bonuses, and try to assign it accordingly.
                if (goal.TimeBonuses.Length <= 0)
                {
                    // Proceed to normal rounding.
                    FinalRounding();
                }
                else
                {
                    float[] convertedTimeBonus = new float[goal.TimeBonuses.Length];

                    // Convert all entires in time bonus to a float.
                    ConvertAndSortTimes(convertedTimeBonus);
                    AssignTimeBonus(convertedTimeBonus);
                }
            }
        }

        private void FinalRounding()
        {
            finalScore = score + timeBonus;

            // TO DO: All the animations and flair.
            Debug.Log($"[{GetType().Name}]: Your final score is {finalScore}!!!");
        }

        /// <summary>
        /// Calculate the time bonus based on the goal tiles.
        /// </summary>
        /// <param name="score"></param>
        /// <param name="time"></param>
        /// <param name="timeBonus"></param>
        /// <param name="convertedTimeBonus"></param>
        /// <returns></returns>
        private void AssignTimeBonus(float[] convertedTimeBonus)
        {
            if (time >= convertedTimeBonus[0])
            {
                // 0 stands for "zero bonus".
                timeBonus = 0;
                Debug.Log($"[{GetType().Name}]: No bonus.");
                FinalRounding();
            }
            else if (time <= convertedTimeBonus[convertedTimeBonus.Length - 1])
            {
                // Biggest index in the list is double bonus!
                timeBonus = score;
                Debug.Log($"[{GetType().Name}]: Double Bonus!!!");
                // TODO: Add an extra flair for a double bonus.
                FinalRounding();
            }
            else
            {
                // Finally, check to see what it's closest too beyond that and assign THAT bonus.
                for (int i = 1; i < convertedTimeBonus.Length -1; i++)
                {
                    if (time < convertedTimeBonus[i])
                    {
                        continue;
                    }
                    else
                    {
                        // This is our bonus.
                        // Convert to a decimal, times by score, and add it.
                        var percentile = i * .1f;
                        Debug.Log($"[{GetType().Name}]: Applying a {i * 10}% bonus.");
                        timeBonus = (int)(score * percentile);
                        break;
                    }
                }
                FinalRounding();
            }
        }

        private void ConvertAndSortTimes(float[] convertedTimeBonus)
        {
            // Conver the entries.
            for (int i = 0; i < goal.TimeBonuses.Length; i++)
            {
                convertedTimeBonus[i] = CoreUtilities.MinSecToFloat(
                    goal.TimeBonuses[i].Minute,
                    goal.TimeBonuses[i].Seconds);
                Debug.Log($"Converted {goal.TimeBonuses[i]} to {convertedTimeBonus[i]}");
            }

            // Sort the entries.
            for (int i = 0; i < convertedTimeBonus.Length - 1; i++)
            {
                for (int j = 0; j < convertedTimeBonus.Length - i - 1; j++)
                {
                    if (convertedTimeBonus[j] < convertedTimeBonus[j + 1])
                    {
                        float temp = convertedTimeBonus[j];
                        convertedTimeBonus[j] = convertedTimeBonus[j + 1];
                        convertedTimeBonus[j + 1] = temp;
                    }
                }
            }
        }
    } 
}
