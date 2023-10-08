namespace BreakoutSystem
{
    using Core;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;

    public abstract class PowerupBehaviour : MonoBehaviour, IInteractable
    {
        public UnityEvent OnInteract = new UnityEvent();

        // Interact wth the powerup.
        public void Interact()
        {
            OnInteract?.Invoke();
            PowerUp();
        }

        protected abstract void PowerUp();
    } 
}
