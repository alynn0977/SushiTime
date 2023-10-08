namespace Core
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Defines a subject that reacts when the player interacts with said object.
    /// </summary>
    public interface IInteractable
    {
        // Interact method for this object.
        public void Interact();
    } 
}
