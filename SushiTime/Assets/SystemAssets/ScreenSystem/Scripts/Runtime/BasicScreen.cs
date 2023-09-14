using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ScreenSystem
{
    public class BasicScreen : ScreenTypeBehaviour
    {

        [Header("Components to Initialize")]
        // List of components to turn off/on.
        [SerializeField]
        private MonoBehaviour[] initializeComponents;

        // List of buttons to turn off/on.
        [SerializeField]
        private Button[] initializeButtons;

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

            if (initializeButtons.Length > 0)
            {
                foreach(var button in initializeButtons)
                { 
                    button.interactable = true;
                }
            }
        }

    }
}
