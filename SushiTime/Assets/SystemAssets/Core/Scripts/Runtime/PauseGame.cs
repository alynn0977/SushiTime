namespace Core
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Allows pausing of a game from a gameobject.
    /// </summary>
    public class PauseGame : MonoBehaviour
    {
        /// <summary>
        /// Force Pause a game from a script.
        /// </summary>
        public void PauseGameGlobal()
        {
            Debug.LogWarning("Game is pausing.");
            AppManager.GlobalPause();
            EventManager.Instance.QueueEvent(new PauseGameEvent(AppManager.IsGlobalPaused));
        }

        /// <summary>
        /// Force Resume a game from a script.
        /// </summary>
        public void ResumeGameGlobal()
        {
            Debug.LogWarning("Game is resuming.");
            AppManager.GlobalResume();
            EventManager.Instance.QueueEvent(new PauseGameEvent(AppManager.IsGlobalPaused));
        }
    }

}