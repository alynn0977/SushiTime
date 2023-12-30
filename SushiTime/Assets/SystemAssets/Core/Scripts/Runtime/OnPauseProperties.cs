namespace Core
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Provides behaviour properties for behaviour when an <see cref="PauseGameEvent"/> is engaged.
    /// </summary>
    public class OnPauseProperties : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent onPauseDetection = new UnityEvent();
        [SerializeField]
        private UnityEvent onPauseRelease = new UnityEvent();
        private void OnEnable()
        {
            EventManager.Instance.AddListener<PauseGameEvent>(OnPauseDetected);
        }

        private void OnPauseDetected(PauseGameEvent e)
        {
            if (e.IsPause)
            {
                onPauseDetection?.Invoke();
            }
            else
            {
                onPauseRelease?.Invoke();
            }
        }
    }

}