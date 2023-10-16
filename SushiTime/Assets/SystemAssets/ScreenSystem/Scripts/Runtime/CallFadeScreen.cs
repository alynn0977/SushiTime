using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScreenSystem
{
    using UnityEngine;
    
    /// <summary>
    /// Send commands to the Fade System via monobehaviour.
    /// Ex: Place on a button.
    /// </summary>
    public class CallFadeScreen : MonoBehaviour
    {
        public void OnFadeScreen(bool fadeIn)
        {
            EventManager.Instance.QueueEvent(new FadeScreenEvent(fadeIn));
        }

        /// <summary>
        /// First, fade the screen in. Then fade out automatically.
        /// </summary>
        /// <param name="fadeIn"></param>
        public void OnFadeScreenAuto()
        {
            EventManager.Instance.QueueEvent(new FadeScreenEvent(true, true));
        }
    }
}
