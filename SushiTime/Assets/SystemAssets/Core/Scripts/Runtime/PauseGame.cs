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
        /// Dictates whether pause function kicks in on awake.
        /// </summary>
        public bool isPauseOnAwake = false;

        /// <summary>
        /// Specify a delay time before Pausing.
        /// </summary>
        public float delayPause = 0f;

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

        private void Start()
        {
            if (delayPause > 0 && isPauseOnAwake)
            {
                Debug.Log("Delay pause");
                Invoke(nameof(PauseGame), delayPause);
            }
            else if (isPauseOnAwake)
            {
                PauseGameGlobal();
            }
        }
    }

}