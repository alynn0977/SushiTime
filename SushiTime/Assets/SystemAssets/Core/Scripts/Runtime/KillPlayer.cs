namespace Core
{
    using UnityEngine;

    public class KillPlayer : MonoBehaviour
    {
        /// <summary>
        /// Send signal for <see cref="KillPlayerEvent"/>.
        /// </summary>
        public void OnKillPlayer()
        {
           AppManager.EndGame();
        } 
    }
}
