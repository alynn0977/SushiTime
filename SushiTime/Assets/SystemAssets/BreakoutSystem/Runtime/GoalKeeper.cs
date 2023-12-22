namespace BreakoutSystem
{
    using BreakoutSystem.UI;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    /// <summary>
    /// Tracks the goal of the level and sends alerts.
    /// </summary>
    public class GoalKeeper : MonoBehaviour
    {
        private GameZone gameZone;
        private UI_Manager UI;
        private const string tileTag = "Tile";

        private GoalKeeping gameGoal;
        private delegate void CountGoal(GameObject go);
        private CountGoal countGoal;

        private List<BrickBehaviour> goalTiles = new List<BrickBehaviour>();
        private int specialGoalCount = 0;

        /// <summary>
        /// Initialize the Goal Keeper.
        /// </summary>
        /// <param name="UI">Requires UI Manager.</param>
        public void InitializeGoalKeeper(UI_Manager UI)
        {
            // Initialize the Goal Keeper by feeding it the UI.
            if (UI != null)
            {
                this.UI = UI;
                this.gameZone = UI.GameZone;
                this.gameGoal = UI.GameZone.GameGoal;
                countGoal = null;

                // With the data, register what goal should occur.
                RegisterGoal(gameGoal.CurrentGoal);
            }
            else
            {
                // No UI means default clear all.
                RegisterGoal(GoalKeeping.GoalType.ClearAll);
            }

        }

        /// <summary>
        /// Gets invoked when a target is hit.
        /// </summary>
        /// <param name="gameObject">GameObject to check against list.</param>
        public void HitGoal(GameObject gameObject)
        {
            if (countGoal == null)
            {
                Debug.LogWarning($"[{GetType().Name}]: HitGoal() called but delegate is null.");
                return;
            }

            countGoal(gameObject);
        }

        /// <summary>
        /// Clear any tile from a list. Used with <see cref="GoalKeeping.GoalType.ClearAll"/>.
        /// </summary>
        /// <param name="obj">Object to clear.</param>
        private void DeregisterAnyTile(GameObject obj)
        {
            if (obj.TryGetComponent(out BrickBehaviour brick))
            {
                if (goalTiles.Contains(brick))
                {
                    goalTiles.Remove(brick);
                }
            }

            if (goalTiles.Count <= 0)
            {
                gameZone.CallGameWin();
            }

            Debug.Log($"There are now {goalTiles.Count} tiles left in the goal.");
        }

        /// <summary>
        /// Deregister only certain tiles from the list. Used with <see cref="GoalKeeping.GoalType.TileGoal"/>.
        /// </summary>
        /// <param name="obj">Object to clear.</param>
        private void DeregisterTile(GameObject obj)
        {
            // Determine if the brick is registered.
            if (obj.TryGetComponent(out BrickBehaviour brick) && goalTiles.Contains(brick))
            {
                // If so, match it against the Goals.
                for (int i = 0; i <= UI.UI_Goals.Count - 1; i++)
                {
                    if (UI.UI_Goals[i].GoalName == brick.TileName)
                    {
                        // If this is a winning tile, reduce it's qty.
                        int qty = UI.UI_Goals[i].GoalQty;
                        if (qty <= 0)
                        {
                            return;
                        }

                        UI.UI_Goals[i].UpdateQTY(qty - 1);

                        if (UI.UI_Goals[i].GoalQty <= 0)
                        {
                            // Reduce special goal count.
                            specialGoalCount--;
                        }
                    }
                }
            }

            if (specialGoalCount <= 0)
            {
                gameZone.CallGameWin();
            }
        }

        /// <summary>
        /// Determine which goal type to track.
        /// </summary>
        /// <param name="goalType">GoalKeeping type.</param>
        private void RegisterGoal(GoalKeeping.GoalType goalType)
        {
            switch (gameGoal.CurrentGoal)
            {
                // If Clear All, register all tiles, count any hit.
                case GoalKeeping.GoalType.ClearAll:
                    // Register all the tiles based on this.
                    CountAllTiles();
                    countGoal = DeregisterAnyTile;
                    break;
                case GoalKeeping.GoalType.TimeGoal:
                    CountAllTiles();
                    countGoal = DeregisterAnyTile;
                    break;
                case GoalKeeping.GoalType.TileGoal:
                    // Get the two goal tiles, because they have the data and amount already.
                    CountSpecialTiles();
                    // Then register a method to address when those tiles are hit.
                    countGoal = DeregisterTile;
                    // Then make methods that reduce the numbers 
                    break;
            }
        }

        /// <summary>
        /// Count specialty tiles only.
        /// </summary>
        private void CountSpecialTiles()
        {
            // First find all the tiles.
            BrickBehaviour[] allBricks = FindObjectsOfType<BrickBehaviour>();
            
            if (allBricks.Length == 0)
            {
                Debug.LogError($"[{GetType().Name}]: Unable to count the bricks. This happens when {GetType().Name} initializes before the bricks do. Check initialize procedure.");
                return;
            }

            // Figure out how many types of special tiles exist.
            for (int i = 0; i <= UI.UI_Goals.Count - 1; i++)
            {
                var goalName = UI.UI_Goals[i].GoalName;

                // Separate these into a select list and add them to goalTiles.
                var addThese = allBricks.Where(brick => brick.TileName == goalName);
                goalTiles.AddRange(addThese);
            }

            // Create a special goal count to track if all goals are met.
            specialGoalCount = UI.UI_Goals.Count;
            foreach (var brick in goalTiles)
            {
                brick.OnBrickDestroy.AddListener(HitGoal);
            }
        }

        // Counts all tiles.
        private void CountAllTiles()
        {
            GameObject[] allTiles = GameObject.FindGameObjectsWithTag(tileTag);
            if (allTiles.Length == 0)
            {
                Debug.LogError($"[{GetType().Name}]: Unable to count the bricks. This happens when {GetType().Name} initializes before the bricks do. Check initialize procedure.");
                return;
            }

            foreach (GameObject tile in allTiles)
            {
                // All in all...
                if (tile.TryGetComponent(out BrickBehaviour brick))
                {
                    // Just another brick in the wall.
                    goalTiles.Add(brick);
                    brick.OnBrickDestroy.AddListener(HitGoal);
                }
            }
        }
    }

}