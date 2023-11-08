namespace BreakoutSystem.UI
{
    using Core;
    using CustomUI;
    using Sirenix.OdinInspector;
    using System;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UI_Manager : MonoBehaviour
    {
        private const int MaxGoalTiles = 3;

        [TabGroup("General Setup")]
        public GameZone GameZone;
        [TabGroup("General Setup")]
        [SerializeField]
        private bool isAutoInitialize = false;
        [TabGroup("Level and Goal")]
        [SerializeField]
        private TMP_Text levelText;
        [TabGroup("Level and Goal")]
        [SerializeField]
        private HorizontalLayoutGroup goalGroup;
        [TabGroup("Level and Goal")]
        [SerializeField]
        private GameObject textPrefab;
        [TabGroup("Level and Goal")]
        [SerializeField]
        private GameObject tilePrefab;
        [TabGroup("Level and Goal")]
        [SerializeField]
        private GoalKeeper goalKeeper;
        [TabGroup("Timer Section")]
        [SerializeField]
        private TMP_Text counterText;
        [TabGroup("Timer Section")]
        [SerializeField]
        private CountDown counter;
        [TabGroup("Stat Section")]
        [SerializeField]
        private TMP_Text scoreText;
        [TabGroup("Stat Section")]
        [SerializeField]
        private TMP_Text livesText;
        [TabGroup("Power Ups Section")]
        [SerializeField]
        private IconArray powerUpPanel;
        [TabGroup("Misc")]
        [SerializeField]
        private PopUpGraphic pressStart;
        private List<GoalTile> UI_goals = new List<GoalTile>();
        

        public List<GoalTile> UI_Goals => UI_goals;

        /// <summary>
        /// Initialize UI connections for the Gamezone.
        /// </summary>
        public void InitializeUIManager()
        {
            if (isGoalValid())
            {
                if (gameGoal != null)
                {
                    SetGoal();
                }
            }

            InitializeScore();
            InitializeLives();
            if (pressStart != null)
            {
                EventManager.Instance.AddListener<ResetGameEvent>(ActivateStartPopup);
                EventManager.Instance.AddListener<LaunchBallEvent>(DeactivateStartPopup);
            }

            EventManager.Instance.AddListener<PowerUpEvent>(PowerUp);
        }
        private GoalKeeping gameGoal
        {
            get
            {
                if (GameZone != null)
                {
                    return GameZone.GameGoal;
                }
                else
                {
                    Debug.LogError("[UI Manager] is missing a Goal Keeping referemce.");
                    return null;
                }
            }
        }

        #region Base Methods
        private void OnEnable()
        {
            if (isAutoInitialize)
            {
                InitializeUIManager(); 
            }
        }

        private void OnDisable()
        {
            if (pressStart != null && EventManager.Instance != null)
            {
                EventManager.Instance.RemoveListener<ResetGameEvent>(ActivateStartPopup);
                EventManager.Instance.RemoveListener<LaunchBallEvent>(DeactivateStartPopup);
                pressStart = default;
            }

            if (EventManager.Instance != null)
            {
                EventManager.Instance.AddListener<PowerUpEvent>(PowerUp); 
            }
        }
        private void Update()
        {
            if (counterText)
            {
                BindTimer();
            }
        } 
        #endregion

        #region Level Panel

        private bool isGoalValid()
        {
            if (!GameZone)
            {
                Debug.LogError("[UI Manager] is missing game zone object and will not activate.");
                return false;
            }

            // Debug.Log($"[UI Manager] Found Goal Object {gameGoal.CurrentLevel} object");
            return true;
        }

        [ContextMenu("Reset Levels")]
        protected void ClearGoal()
        {
            levelText.text = $"Level ##";
            var children = goalGroup.GetComponentsInChildren<Transform>();
            if (children.Length == 0)
            {
                // Nothing here.
                return;
            }

            for (int i = children.Length - 1; i > 0; i--)
            {
                RemoveChildObject(children[i].gameObject);
            }
        }

        private void SetGoal()
        {
            if (levelText == null)
            {
                Debug.LogWarning("[UI_Manager] is missing level text reference.");
                return;
            }
            levelText.text = $"Level {gameGoal.CurrentLevel}";

            if (goalGroup)
            {
                switch (gameGoal.CurrentGoal)
                {
                    case GoalKeeping.GoalType.TileGoal:
                        SetTileGoals();
                        SetCountUpTimer();
                        break;
                    case GoalKeeping.GoalType.TimeGoal:
                        SetTextGoal($"{gameGoal.TimeLimit} min " + "\r\n" + "time limit.");
                        SetTimeGoal(gameGoal.TimeLimit);
                        break;
                    case GoalKeeping.GoalType.ClearAll:
                        SetTextGoal("Clear All!");
                        SetCountUpTimer();
                        break;
                }
            }

            if (goalKeeper)
            {
                goalKeeper.InitializeGoalKeeper(this);
            }
            else
            {
                Debug.LogWarning($"[{GetType().Name}]: Is missing Goal Keeper reference. Is this intentional?");
            }
        }
        
        private void SetTextGoal(string value)
        {
            // Instantiate next Prefab.
            GameObject newText = Instantiate(textPrefab);

            // Set parent.
            newText.transform.SetParent(goalGroup.transform);

            // Set Text.
            newText.GetComponentInChildren<TMP_Text>().text = value;
        }

        private void SetTileGoals()
        {
            if (gameGoal.GoalTiles.Length == 0)
            {
                Debug.LogWarning("[UI Manager] tried to creat tiles, but list is null.");
                return;
            }

            int maxTiles;

            if (gameGoal.GoalTiles.Length - 1 > 3)
            {
                maxTiles = MaxGoalTiles;
            }
            else
            {
                maxTiles = gameGoal.GoalTiles.Length - 1;
            }

            for (int i = 0; i <= maxTiles; i++)
            {
                var gameTile = gameGoal.GoalTiles[i];
                SetNewGoalTile(
                    gameTile.GameTile.TileName,
                    gameTile.Quantity,
                    gameTile.GameTile.TileSprite);
            }
        }

        private void SetNewGoalTile(string name, int qty, SpriteRenderer spriteImage)
        {
            if (string.IsNullOrEmpty(name) || qty == default || spriteImage == null)
            {
                Debug.LogWarning("[UI Manager] SetNewGoalTile given empty paramter.");
                return;
            }

            // Instantiate the tile prefab.
            var newTile = Instantiate(tilePrefab);

            // Set the parent.
            newTile.transform.SetParent(goalGroup.transform);

            if (newTile.TryGetComponent(out IConstructable<SpriteRenderer, int, string> construct))
            {
                construct.ConstructWithThree(spriteImage, qty, name);
                
                UI_goals.Add(newTile.GetComponent<GoalTile>());
            }
            else
            {
                Debug.LogError("[UI Manager] Encountered an error trying to construct new tile object. Deleting and skipping.");
                RemoveChildObject(newTile);
            }
        }
        #endregion

        #region Time Panel

        public void BindTimer()
        {
            if (counterText)
            {
                counterText.text = CoreUtilities.MinSecCountdown(counter.RemainingTime) + " Min";
            }
        }

        private void SetTimeGoal(float timeGoalValue)
        {
            if (timeGoalValue == 0 || counter == null)
            {
                Debug.LogError("[UI Manager] Timer cannot be set without counter or 0 time.");
                return;
            }

            counter.SetCountDown(timeGoalValue, true);
        }

        private void SetCountUpTimer()
        {
            if (counter == null)
            {
                Debug.LogError("[UI Manager] Timer cannot be set without counter.");
                return;
            }

            counter.SetCountDown(0, false);
            BindTimer();
        }
        #endregion

        #region Stats Panel
        private int CurrentScore
        {
            get;
            set;
        }
        private int CurrentLives
        {
            get;
            set;
        }
        private void InitializeScore()
        {
            CurrentScore = 0;
            if (scoreText)
            {
                scoreText.text = "0";
            }
            EventManager.Instance.AddListener<ChangeScoreEvent>(ScoreChange);
        }

        private void InitializeLives()
        {
            CurrentLives = 3;
            if (livesText)
            {
                livesText.text = "x" + CurrentLives.ToString();
            }
            EventManager.Instance.AddListener<ChangeLivesEvent>(LifeChange);
        }
        private void ScoreChange(ChangeScoreEvent e)
        {
            CurrentScore = CurrentScore + e.ScoreAmount;
            if (scoreText)
            {
                scoreText.text = CurrentScore.ToString();
            }
        }

        private void LifeChange(ChangeLivesEvent e)
        {
            // Null check in case of debugging.
            if (CurrentLives == 0)
            {
                return;
            }

            CurrentLives = CurrentLives + e.LivesAmount;
            if (livesText)
            {
                livesText.text = "x"+CurrentLives.ToString();

                if (CurrentLives == 0)
                {
                    // Tell Game manager to game over!
                    GameZone.CallGameOver();
                }
            }
        }

        #endregion

        #region PowerUp Panel
        private void PowerUp(PowerUpEvent e)
        {
            if (e.PowerUp.PowerPrefab == null)
            {
                Debug.LogError("[UI Manager] null power up reference. Check prefab.");
            }
            if (powerUpPanel)
            {
                powerUpPanel.SetIcon(e.PowerUp.PowerPrefab, e.PowerUp.Time);
            }
        }
        #endregion

        #region Misc
        private void ActivateStartPopup(ResetGameEvent e)
        {
            pressStart.CallPopUp();
        }
        private void DeactivateStartPopup(LaunchBallEvent e)
        {
            if (pressStart.gameObject.activeInHierarchy)
            {
                pressStart.StopAllCoroutines();
                pressStart.gameObject.SetActive(false);
            }
        }

        #endregion

        #region Utilities
        private void RemoveChildObject(GameObject obj)
        {
#if UNITY_EDITOR
            DestroyImmediate(obj);
#else
                Destroy(obj);
#endif
        } 
        #endregion
    }
  
}
