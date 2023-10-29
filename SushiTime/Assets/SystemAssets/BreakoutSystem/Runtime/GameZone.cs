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
        [SerializeField]
        [BoxGroup("UI")]
        private GameObject gameWinScreen;

        [SerializeField]
        [BoxGroup("Initialized Items")]
        private GameObject[] initializedObjects;

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

        public void CallGameWin()
        {
            gameWinScreen.SetActive(true);
            Debug.Log($"[{GetType().Name}]: YOU WIN!!!!!!!!!!");
        }

        private void OnEnable()
        {
            if (!goal)
            {
                Debug.LogWarning($"[Game Zone]:{gameObject.name} does not have goal data. Is this intentional?");
            }

            InitializeModalCanvas();
            InitializeObjects();
            Invoke(nameof(BeginGame), 1.5f);
        }

        private void InitializeObjects()
        {
            if (initializedObjects == null || initializedObjects.Length <= 0)
            {
                Debug.LogWarning($"[{GetType().Name}] - {gameObject.name}: Nothing to initialize.");
                return;
            }

            foreach (var obj in initializedObjects)
            {
                obj.gameObject.SetActive(true);
            }
        }

        private void PlayerPowerIncrease(IncreasePowerEvent e)
        {
            Debug.Log($"Player power increased to {e.SetPower}");
            playerPowr = e.SetPower;
        }

        [ContextMenu("Begin Game")]
        private void BeginGame()
        {
            MainBall.gameObject.SetActive(true);
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