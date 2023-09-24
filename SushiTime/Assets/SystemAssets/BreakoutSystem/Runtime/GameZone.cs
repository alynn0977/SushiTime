namespace BreakoutSystem
{
    using UnityEngine;
    using Sirenix.OdinInspector;

    // Tracks globals for this particular level instance.
    public class GameZone : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField]
        [BoxGroup("Player")]
        private int playerPowr = 1;

        [SerializeField]
        [BoxGroup("Player")]
        private BallBehaviour mainBall;

        [SerializeField]
        [BoxGroup("Goal")]
        private GoalKeeping goal;

        [SerializeField]
        [BoxGroup("UI")]
        private Canvas modalCanvas;
        
        [SerializeField]
        [BoxGroup("UI")]
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

        /// <summary>
        /// Call the gameover screen.
        /// </summary>
        public void CallGameOver()
        {
            gameOverScreen.SetActive(true);
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
        }
    }

}