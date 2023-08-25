namespace BreakoutSystem
{
    using UnityEngine;
    using Sirenix.OdinInspector;
    using System;

    /// <summary>
    /// Goal Keeping Scriptable Object. Create one for each level, and define 
    /// what the player must do, in order to win the level.
    /// </summary>
    [CreateAssetMenu(fileName = "Goal Keeper", menuName = "My Assets/Goal", order = 0)]
    public class GoalKeeping : ScriptableObject 
    {
        [SerializeField] 
        private string level;

        [EnumToggleButtons]
        [SerializeField]
        private GoalType levelGoal;

        #region Goal Varaiables
        [SerializeField]
        [ShowIf("levelGoal", GoalType.TileGoal)]
        private Goal[] goalTiles;

        [SerializeField]
        [Tooltip("In Minutes")]
        [ShowIf("levelGoal", GoalType.TimeGoal)]
        private float timeLimit;

        [SerializeField]
        [ShowIf("levelGoal", GoalType.ClearAll)]
        [InfoBox("Still under construction.")]
        #endregion
        public string CurrentLevel => level;
        public GoalType CurrentGoal => levelGoal;

        public float TimeLimit => timeLimit;

        public enum GoalType{
            TileGoal,
            TimeGoal,
            ClearAll,
        }
    }

    /// <summary>
    /// Specify a GameObject to hit, and how many times.
    /// </summary>
    [Serializable]
    public struct Goal
    {
        /// <summary>
        /// What Game Tile should be hit?
        /// </summary>
        public BrickBehaviour GameTile;

        /// <summary>
        /// How many should be hit?
        /// </summary>
        public int Quantity;
    }
}
