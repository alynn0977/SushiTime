namespace Core
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.InputSystem;

    /// <summary>
    /// Allows pausing of a game from a gameobject.
    /// </summary>
    public class PauseGame : MonoBehaviour, ISystemInitializer
    {
        [Header("Setup")]
        /// <summary>
        /// Dictates whether pause function kicks in on awake.
        /// </summary>
        public bool isPauseOnAwake = false;

        /// <summary>
        /// Specify a delay time before Pausing.
        /// </summary>
        public float delayPause = 0f;

        [Space]
        [Header("Optional Keyboard")]
        /// <summary>
        /// If true, will pause when Pause Key is pressed.
        /// </summary>
        public bool isPauseByKey = true;

        /// <summary>
        /// Use events to call a pause function.
        /// </summary>
        [SerializeField]
        private UnityEvent onPauseKey = new UnityEvent();

        [SerializeField]
        private InputAction pauseAction;
        public void Initialize()
        {
            PauseGameGlobal();
        }

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
            Debug.Log("Game is resuming.");
            AppManager.GlobalResume();
            EventManager.Instance.QueueEvent(new PauseGameEvent(AppManager.IsGlobalPaused));
        }

        /// <summary>
        /// Use this to toggle pause, but not call a screen.
        /// </summary>
        public void TogglePause()
        {
            if (!AppManager.IsGlobalPaused)
            {
                AppManager.GlobalPause();
            }
            else
            {
                AppManager.GlobalResume();
            }
        }

        private void OnEnable()
        {
            pauseAction.performed += context => OnPause();
        }

        private void OnPause()
        {
            if (isPauseByKey && pauseAction.triggered)
            {
                // Pause.
                onPauseKey?.Invoke();
            }
        }

        private void Start()
        {
            if (delayPause > 0 && isPauseOnAwake)
            {
                Debug.LogWarning($"Delay pause called from {gameObject.name}.");
                Invoke(nameof(Initialize), delayPause);
            }
            else if (isPauseOnAwake)
            {
                Initialize();
            }
        }
    }

}