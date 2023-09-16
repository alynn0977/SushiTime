namespace ScreenSystem
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Close the current screen.
    /// Ex: Attach to a button and assign the gameobject to close.
    /// </summary>
    [Serializable]
    public class CloseCurrentScreen : MonoBehaviour
    {
        [SerializeField]
        private float delayClose = 0f;

        private GameObject destroyThis;
        /// <summary>
        /// Close a current screen by destroying it.
        /// </summary>
        /// <param name="gameObjectToClose">Object to be closed (screen)</param>
        public void CloseScreen(GameObject gameObjectToClose)
        {
            destroyThis = gameObjectToClose; 
            if (delayClose > 0f)
            {
                Invoke(nameof(DelayClose), delayClose);
            }
            else
            {
                Destroy(gameObjectToClose);
            }
            
        }

        private void DelayClose()
        {
            Debug.Log("Delay closing.");
            Destroy(destroyThis);
        }
    } 
}
