namespace BreakoutSystem
{
    using Core;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class PowerupBehaviour : MonoBehaviour, iInteractable
    {
        // Interact wth the powerup.
        public void Interact()
        {
            PowerUp();   
        }

        protected abstract void PowerUp();
    } 
}
