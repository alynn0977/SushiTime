namespace BreakoutSystem
{
	using Core;
	using UnityEngine;
    using UnityEngine.Events;

    public class GameOverObject : MonoBehaviour, IInteractable
    {
        public UnityEvent OnInteract = new UnityEvent();
        public void Interact()
        {
            OnInteract?.Invoke();
        }
    }

}