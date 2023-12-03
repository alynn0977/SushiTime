using Core;
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
        [Tooltip("These are disabled, and then start themselves once Enabled.")]
        protected MonoBehaviour[] initializeComponents;

        [SerializeField]
        [Tooltip("These need to initialize in a special way, but are still enabled.")]
        protected InitializeProxy[] initializeSubsystems;
        
        [SerializeField]
        protected Button[] initializeButtons;
        public abstract void InitializeScreen();

        public virtual void ActivateSystems()
        {
            foreach (var system in initializeSubsystems)
            {
                if (system == null)
                {
                    Debug.LogWarning($"[{GetType().Name}]: Null subsystem on {system.gameObject.name}.");
                }
                system.Initialize();
            }

            foreach (var obj in initializeComponents)
            {
                
                // obj.enabled = true;
                Debug.LogWarning($"[{GetType().Name}]: attempted to activate {obj.gameObject.name}. Should it be converted?");
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
