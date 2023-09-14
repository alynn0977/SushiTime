
namespace ScreenSystem
{
    /// <summary>
    /// Pass a gameover event.
    /// </summary>
    public class GameOverScreenEvent : GameEvent
    {
        /// <summary>
        /// Insert a delay to use before invoking the gameover event.
        /// </summary>
        public float Delay = 0f;
        
        /// <summary>
        /// Construct a new game over screen event.
        /// </summary>
        /// <param name="delay">Optional delay until event is called.</param>
        public GameOverScreenEvent(float delay)
        {
            this.Delay = delay;
        }
    }
}
