namespace BreakoutSystem.UI
{
    using Sirenix.OdinInspector;
    using TMPro;
    using UnityEngine;

    public class UI_Manager : MonoBehaviour
    {
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

        
        [ContextMenu("Validate Goal Object")]
        protected void ValidateGoal()
        {
            if (!GameZone)
            {
                Debug.LogError("[UI Manager] is missing game zone object and will not activate.");
            }

            Debug.Log($"[UI Manager] Found Goal Object {GameZone.GameGoal.CurrentLevel} object");
            SetGoal();
        }

        private void SetGoal()
        {
            levelText.text = $"Level {GameZone.GameGoal.CurrentLevel}";
        }
    }
  
}
