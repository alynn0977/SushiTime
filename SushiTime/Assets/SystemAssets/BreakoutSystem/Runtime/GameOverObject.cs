namespace BreakoutSystem
{
	using Core;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

    public class GameOverObject : MonoBehaviour, iInteractable
    {
        public void Interact()
        {
            Debug.Log($"Ball interacting with game over object.");
            EventManager.Instance.QueueEvent(new ChangeLivesEvent(-1));
        }
    }

}