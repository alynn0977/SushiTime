namespace BreakoutSystem.UI
{
    using Core;
    using CustomUI;
    using Sirenix.OdinInspector;
    using System;
    using TMPro;
    using Unity.VisualScripting;
    using UnityEngine;
    using UnityEngine.UI;

    public class UI_Manager : MonoBehaviour
    {
        private const int MaxGoalTiles = 3;

        // Should begin by reading the Goal Keeping scriptable.
        // What level is this one?
        // What are the goals for this level?
        // What time limit is there?
        // What's the score?
        // How many lives?
        // What power ups are there?
        [TabGroup("General Setup")]
        public GameZone GameZone;

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
        private void Start()
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
            EventManager.Instance.AddListener<PowerUpEvent>(PowerUp);
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

            Debug.Log($"[UI Manager] Found Goal Object {gameGoal.CurrentLevel} object");
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

            if (newTile.TryGetComponent(out iConstructable<SpriteRenderer, string, string> construct))
            {
                construct.ConstructWithThree(spriteImage, qty.ToString(), name);
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
