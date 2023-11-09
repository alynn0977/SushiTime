using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ScreenSystem
{
    /// <summary>
    /// Abstract to define behaviours of a screen type.
    /// </summary>
    public abstract class ScreenTypeBehaviour : MonoBehaviour
    {
        [Header("Components to Initialize")]
        [SerializeField]
        protected MonoBehaviour[] initializeComponents;

        [SerializeField]
        protected InitializeProxy[] initializeSubsystems;
        
        [SerializeField]
        protected Button[] initializeButtons;
        public abstract void InitializeScreen();

        public virtual void ActivateSystems()
        {
            foreach (var obj in initializeComponents)
            {
                obj.enabled = true;
                Debug.Log($"[{GetType().Name}]: Activating {gameObject.name}");
            }

            if (initializeButtons.Length > 0)
            {
                foreach (var button in initializeButtons)
                {
                    button.interactable = true;
                }
            }
        }
    }
}
