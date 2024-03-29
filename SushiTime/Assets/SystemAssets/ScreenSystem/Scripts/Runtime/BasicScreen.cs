using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ScreenSystem
{
    public class BasicScreen : ScreenTypeBehaviour
    {
        public override void InitializeScreen()
        {
            EventManager.Instance.QueueEvent(new FadeScreenEvent(false));
            EventManager.Instance.AddListenerOnce<FadeScreenPostEvent>(e => ActivateSystems());
        }

        private void OnDestroy()
        {
            if (EventManager.Instance != null)
            {
                EventManager.Instance.RemoveListener<FadeScreenPostEvent>(e => ActivateSystems()); 
            }
        }
    }
}
