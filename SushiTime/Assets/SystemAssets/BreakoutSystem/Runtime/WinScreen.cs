namespace BreakoutSystem
{
    using Sirenix.OdinInspector;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Tallies the final score and provides bonuses.
    /// Passes these numbers to the <see cref="GameManager"/>.
    /// TO DO: Provide a shop for players to buy upgrades with.
    /// </summary>
    public class WinScreen : MonoBehaviour
    {
        [InfoBox("Please Note: Win Screen is designed for use when game is paused!")]
        private const float DelayForSeconds = .011f;

        [SerializeField]
        private Text scoreText;
        [SerializeField]
        private Text timeText;
        [SerializeField]
        private Text totalText;

        [SerializeField]
        private string[] methodNames;

        private int score = 0;
        private float time = 0f;
        private GoalKeeping goal = default;
        private int timeBonus = 0;
        private int finalScore;

        /// <summary>
        /// Execute the series of methods listed in <see cref="methodNames"/>.
        /// </summary>
        public void RunRoutines()
        {
            StartCoroutine(RunMethods());
        }

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
                    PrepareFinalScore();
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

        private void PrepareFinalScore()
        {
            finalScore = score + timeBonus;
            Debug.Log($"[{GetType().Name}]: Your final score is {finalScore}!!!");
        }

        /// <summary>
        /// Animate the score text.
        /// </summary>
        public void AnimateScore()
        {
            // TO DO: All the animations and flair.
            if (scoreText)
            {
                scoreText.gameObject.SetActive(true);
                StartCoroutine(UpdateScoreText(scoreText, score, "Score: "));
            }

        }
        /// <summary>
        /// Animate the time bonus text.
        /// </summary>
        private void AnimateTimeBonus()
        {
            // TO DO: All the animations and flair.
            if (timeText)
            {
                timeText.gameObject.SetActive(true);
                StartCoroutine(UpdateScoreText(timeText, timeBonus, "Time:  "));
            }
        }

        /// <summary>
        /// Animate the final total.
        /// </summary>
        private void AnimateTotals()
        {
            // TO DO: All the animations and flair.
            if (totalText)
            {
                totalText.gameObject.SetActive(true);
                StartCoroutine(UpdateScoreText(totalText, finalScore, "TOTAL: "));
            }
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
                PrepareFinalScore();
            }
            else if (time <= convertedTimeBonus[convertedTimeBonus.Length - 1])
            {
                // Biggest index in the list is double bonus!
                timeBonus = score;
                Debug.Log($"[{GetType().Name}]: Double Bonus!!!");
                // TODO: Add an extra flair for a double bonus.
                PrepareFinalScore();
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

                PrepareFinalScore();
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
        private IEnumerator RunMethod(string methodName)
        {
            Debug.Log($"Running {methodName}");
            yield return StartCoroutine(methodName);
        }
        private IEnumerator RunMethods()
        {
            Debug.Log("Running all methods");
            scoreText.gameObject.SetActive(true);
            yield return StartCoroutine(UpdateScoreText(scoreText, score, "Score: "));
            timeText.gameObject.SetActive(true);
            if (timeBonus == 0)
            {
                timeText.fontSize = 80;
                timeText.text = "No Time Bonus";
            }
            else
            {
                yield return StartCoroutine(UpdateScoreText(timeText, timeBonus, "Time:  "));
            }
            
            totalText.gameObject.SetActive(true);
            yield return StartCoroutine(UpdateScoreText(totalText, finalScore, "TOTAL: "));
        }
        private IEnumerator UpdateScoreText(Text text, int targetScore, string prefix)
        {
            int currentScore = 0;
            while (currentScore < targetScore)
            {
                currentScore++;
                string ending = currentScore < 10 ? $"0{currentScore}" : currentScore.ToString();
                text.text = $"{prefix}{ending}";
                yield return new WaitForSecondsRealtime(DelayForSeconds);
            }
            yield return true;
        }
    } 
}
