namespace ScreenSystem
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Call a new screen through monobehaviour.
    /// Ex: Place on a button, and call the public function.
    /// </summary>
    public class CallNewScreen : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Provide name of the screen to close. Should be availble via Screen Object.")]
        private string ScreenName;

        [SerializeField]
        [Tooltip("Optional: Delay the new screen event by X seconds.")]
        private float delayTime = 0;
        /// <summary>
        /// Closes the screen that matches the name specified in ScreenName field.
        /// </summary>
        public void OnCallScreen()
        {
            if (String.IsNullOrEmpty(ScreenName))
            {
                Debug.LogWarning($"OpenScreen on {gameObject.name} has null fields.");
                return;
            }
            if (delayTime <= 0)
            {
                EventManager.Instance.QueueEvent(new CallNewScreenGameEvent(ScreenName));
            }
            else
            {
                Invoke(nameof(DelayOnCallScreen), delayTime);
            }
        }

        private void DelayOnCallScreen()
        {
            EventManager.Instance.QueueEvent(new CallNewScreenGameEvent(ScreenName));
        }
    } 
}
