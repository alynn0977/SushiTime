using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScreenSystem
{
    public class BasicScreen : ScreenTypeBehaviour
    {

        [Header("Components to Initialize")]
        // List of components to turn off/on.
        [SerializeField]
        private MonoBehaviour[] initializeComponents;

        public override void InitializeScreen()
        {

            EventManager.Instance.QueueEvent(new FadeScreenEvent(false));
            EventManager.Instance.AddListenerOnce<FadeScreenPostEvent>(ActivateSystems);
        }

        private void ActivateSystems(FadeScreenPostEvent e)
        {
            foreach(var obj in initializeComponents)
            {
                obj.enabled = true;
            }
        }

    }
}
