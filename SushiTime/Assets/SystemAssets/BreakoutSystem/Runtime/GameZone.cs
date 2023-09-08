namespace BreakoutSystem
{
    using UnityEngine;

    // Tracks globals for this particular level instance.
    public class GameZone : MonoBehaviour
    {
        [SerializeField]
        private int playerPowr = 1;

        [SerializeField]
        private BallBehaviour mainBall;

        [SerializeField]
        private GoalKeeping goal;
        /// <summary>
        /// Read-only access of current player power stat.
        /// </summary>
        public int PlayerPower => playerPowr;

        /// <summary>
        /// Read-only access to current game goal data.
        /// </summary>
        public GoalKeeping GameGoal => goal;

        public BallBehaviour MainBall => mainBall;

        private void Start()
        {
            if (!goal)
            {
                Debug.LogWarning($"[Game Zone]:{gameObject.name} does not have goal data. Is this intentional?");
            }

            Invoke(nameof(BeginGame), 1.5f);
        }

        private void PlayerPowerIncrease(IncreasePowerEvent e)
        {
            Debug.Log($"Player power increased to {e.SetPower}");
            playerPowr = e.SetPower;
        }

        [ContextMenu("Begin Game")]
        private void BeginGame()
        {
            MainBall.LaunchBall();
        }

        // TO DO: What are the goals?
    }

}