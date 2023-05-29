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
        /// <summary>
        /// Close a current screen by destroying it.
        /// </summary>
        /// <param name="gameObjectToClose">Object to be closed (screen)</param>
        public void CloseScreen(GameObject gameObjectToClose)
        {
            Debug.Log($"Closing {gameObjectToClose}");
            Destroy(gameObjectToClose);
        }
    } 
}
