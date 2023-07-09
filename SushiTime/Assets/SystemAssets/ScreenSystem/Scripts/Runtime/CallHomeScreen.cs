namespace ScreenSystem
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Call the home screen. May instant close
    /// all other screens, or may invokve a modal
    /// screen first.
    /// </summary>
    public class CallHomeScreen : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Optional. How long to delay before calling home screen.")]
        private float delay = 0f;
        private bool thisModalMode;
        public void OnCallHomeScreen(bool modalMode)
        {
            if (delay <= 0)
            {
                EventManager.Instance.QueueEvent(new CallHomeScreenEvent(modalMode));
            }
            else
            {
                thisModalMode = modalMode;
                Invoke(nameof(DelayCallHomeScreen), delay);
            }
        }
        
        private void DelayCallHomeScreen()
        {
            EventManager.Instance.QueueEvent(new CallHomeScreenEvent(thisModalMode));
        }
    } 
}
