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
        public void OnCallHomeScreen(bool modalMode)
        {
            EventManager.Instance.QueueEvent(new CallHomeScreenEvent(modalMode));
        }       
    } 
}
