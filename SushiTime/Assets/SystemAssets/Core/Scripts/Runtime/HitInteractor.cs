namespace Core
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Activates the objects interaction methods.
    /// </summary>
    public class HitInteractor : MonoBehaviour, IInteractable
    {
        public UnityEvent OnInteract = new UnityEvent();
        public void Interact()
        {
            OnInteract?.Invoke();
            Debug.Log($"Interaction called on {gameObject.name}");
        }
    }
}
