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

        [SerializeField]
        private Canvas modalCanvas;

        [SerializeField]
        private GameObject gameOverScreen;
        /// <summary>
        /// Read-only access of current player power stat.
        /// </summary>
        public int PlayerPower => playerPowr;

        /// <summary>
        /// Read-only access to current game goal data.
        /// </summary>
        public GoalKeeping GameGoal => goal;

        public BallBehaviour MainBall => mainBall;
        public void CallGameOver()
        {
            gameOverScreen.SetActive(true);
        }

        public void RemoveGameOver()
        {
            // Turn off modal.
            // Shut down game.
            // Send info to GameManager if available.
        }

        // TODO: What are the goals?
        private void Start()
        {
            if (!goal)
            {
                Debug.LogWarning($"[Game Zone]:{gameObject.name} does not have goal data. Is this intentional?");
            }

            InitializeModalCanvas();
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

        private void InitializeModalCanvas()
        {
            if (modalCanvas != null)
            {
                modalCanvas = GetComponentInChildren<Canvas>();
                if (modalCanvas == null)
                {
                    Debug.LogWarning($"[{GetType().Name}]: {gameObject.name} lacks a modal canvas.");
                    return;
                }
            }
            gameOverScreen.SetActive(false);
        }
    }

}