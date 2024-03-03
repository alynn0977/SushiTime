namespace BreakoutSystem
{
    using UnityEngine;
    using Sirenix.OdinInspector;
    using BreakoutSystem.UI;
    using Core;

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
        [BoxGroup("Player")]
        private PaddleBehaviour paddle;

        [SerializeField]
        [BoxGroup("Goal")]
        private GoalKeeping goal;

        [SerializeField]
        [BoxGroup("UI")]
        private Canvas modalCanvas;
        
        [SerializeField]
        [BoxGroup("UI")]
        private UI_Manager UI_Manager;
        
        [SerializeField]
        [BoxGroup("UI")]
        private GameObject gameOverScreen;
        [SerializeField]
        [BoxGroup("UI")]
        private GameObject gameWinScreen;

        [SerializeField]
        [BoxGroup("Initialized Items")]
        private InitializeProxy[] initializedSubsystems;

        /// <summary>
        /// Read-only access of current player power stat.
        /// </summary>
        public int PlayerPower => playerPowr;

        /// <summary>
        /// Read-only access to current game goal data.
        /// </summary>
        public GoalKeeping GameGoal => goal;

        /// <summary>
        /// Read-only access to the ball.
        /// </summary>
        public BallBehaviour MainBall => mainBall;

        /// <summary>
        /// Read-only access to the paddle.
        /// </summary>
        public PaddleBehaviour MainPaddle => paddle;

        /// <summary>
        /// Call the gameover screen.
        /// </summary>
        public void CallGameOver()
        {
            gameOverScreen.SetActive(true);
            Destroy(MainBall);
            Destroy(MainPaddle);
        }

        [ContextMenu("Call Game Win")]
        public void CallGameWin()
        {
            gameWinScreen.SetActive(true);
            if (gameWinScreen.TryGetComponent(out WinScreen winScreen))
            {
                winScreen.InitializeWinScreen(UI_Manager.CurrentScore, UI_Manager.Counter.RemainingTime, goal);
            }
        }

        private void OnEnable()
        {
            if (!goal)
            {
                Debug.LogWarning($"[Game Zone]:{gameObject.name} does not have goal data. Is this intentional?");
            }

            InitializeModalCanvas();
            InitializeSubSytems();

            Invoke(nameof(BeginGame), 1.5f);
        }

        private void InitializeSubSytems()
        {
            foreach (var proxy in initializedSubsystems) 
            {
                proxy.Initialize();
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
            MainBall.LaunchBall(null);
            EventManager.Instance.AddListener<IncreasePowerEvent>(PlayerPowerIncrease);
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