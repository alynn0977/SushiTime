namespace BreakoutSystem
{
    using UnityEngine;
    using Sirenix.OdinInspector;

    [CreateAssetMenu(fileName = "Goal Keeper", menuName = "My Assets/Goal", order = 0)]
    public class GoalKeeping : ScriptableObject 
    {
        [SerializeField] 
        private string level;

        [SerializeField]
        private GoalType levelGoal;

        [SerializeField]
        [Tooltip("In Minutes")]
        private float timeLimit;

        public string CurrentLevel => level;
        public GoalType CurrentGoal => levelGoal;

        public float TimeLimit => timeLimit;

        public enum GoalType{
            TileGoal,
            TimeGoal,
            ItemGoal,
        }
    }
}
