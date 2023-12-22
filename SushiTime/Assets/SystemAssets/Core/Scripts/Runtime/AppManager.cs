namespace Core
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Application manager for the system. Tracks only the most universal
    /// elements of any application such as pausing and quitting.
    /// </summary>
    public static class AppManager
    {
        private static bool isGlobalPaused = false;

        /// <summary>
        /// True if the entire game is paused,
        /// which means time is set to zero.
        /// </summary>
        public static bool IsGlobalPaused => isGlobalPaused;

        /// <summary>
        /// Ceases Game Time Scale
        /// at a global scale.
        /// </summary>
        public static void GlobalPause()
        {
            Debug.Log("Time Scale at 0");
            Time.timeScale = 0;
            isGlobalPaused = true;
        }

        /// <summary>
        /// Resumes Game Time Scale
        /// at global level.
        /// </summary>
        public static void GlobalResume()
        {
            Time.timeScale = 1;
            isGlobalPaused = false;
        }

        public static void EndGame()
        {
            //EventManager.Instance.QueueEvent(new KillPlayerEvent());
        }
    }

}