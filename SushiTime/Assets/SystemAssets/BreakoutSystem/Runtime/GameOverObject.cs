namespace BreakoutSystem
{
	using Core;
	using UnityEngine;
    using UnityEngine.Events;

    public class GameOverObject : MonoBehaviour, iInteractable
    {
        public UnityEvent OnInteract = new UnityEvent();
        public void Interact()
        {
            OnInteract?.Invoke();
        }
    }

}