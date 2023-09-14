
namespace ScreenSystem
{
    using UnityEngine;

    /// <summary>
    /// Class that calls <see cref="GameOverScreenEvent"/> and
    /// related.
    /// </summary>
    public class GameOver : MonoBehaviour
    {
        /// <summary>
        /// Fire the gameover screen.
        /// </summary>
        public void CallGameOver(float delay)
        {
            EventManager.Instance.QueueEvent(new GameOverScreenEvent(delay));
        }
    }
}
