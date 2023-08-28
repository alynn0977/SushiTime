namespace BreakoutSystem.UI
{
    using Core;
    using Sirenix.OdinInspector;
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

        private GoalKeeping gameGoal => GameZone.GameGoal;

        
        [ContextMenu("Validate Goal Object")]
        protected void ValidateGoal()
        {
            if (!GameZone)
            {
                Debug.LogError("[UI Manager] is missing game zone object and will not activate.");
            }

            Debug.Log($"[UI Manager] Found Goal Object {gameGoal.CurrentLevel} object");
            SetGoal();
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

            for (int i = children.Length -1 ; i > 0; i--)
            {
                RemoveChildObject(children[i].gameObject);
            }
        }

        private void SetGoal()
        {
            levelText.text = $"Level {gameGoal.CurrentLevel}";

            if (goalGroup)
            {
                switch (gameGoal.CurrentGoal)
                {
                    case GoalKeeping.GoalType.TileGoal:
                        
                        if (gameGoal.GoalTiles.Length == 0)
                        {
                            Debug.LogWarning("[UI Manager] tried to creat tiles, but list is null.");
                            return;
                        }

                        int maxTiles;

                        if (gameGoal.GoalTiles.Length -1 > 3)
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

                        break;
                    case GoalKeeping.GoalType.TimeGoal:

                        SetTextGoal($"{gameGoal.TimeLimit} min "+ "\r\n" + "time limit.");
                        break;
                    case GoalKeeping.GoalType.ClearAll:
                        SetTextGoal("Clear All!");
                        break;
                }
            }
            // Depending on what goal type it is, it shoudl generate different things in the goal area.
            // If Tile Goal, display the tiles.
            // If time goal display the time. 
            // If break them all, them display break all.
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

        private void SetNewGoalTile(string name, int qty, SpriteRenderer spriteImage)
        {
            if (string.IsNullOrEmpty(name) || qty == null || spriteImage == null)
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

        private void RemoveChildObject(GameObject obj)
        {
#if UNITY_EDITOR
            DestroyImmediate(obj);
#else
                Destroy(obj);
#endif
        }
    }
  
}
