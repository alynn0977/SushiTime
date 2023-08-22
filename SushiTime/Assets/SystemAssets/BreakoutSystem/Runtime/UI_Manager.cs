namespace BreakoutSystem.UI
{
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

        public GameZone gameZone;

        [SerializeField]
        private TMP_Text levelText;
    }
}
