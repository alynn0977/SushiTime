namespace ScreenSystem
{
    /// <summary>
    /// Call a new screen by string name.
    /// </summary>
    public class CallNewScreenGameEvent : GameEvent
    {
        /// <summary>
        /// Name of the screen to call.
        /// </summary>
        public string ScreenName;

        /// <summary>
        /// Construct a new CallNewScreenGameEvent.
        /// Gets passed by the event System.
        /// </summary>
        /// <param name="screenName">Name of the screen to call.</param>
        public CallNewScreenGameEvent(string screenName)
        {
            this.ScreenName = screenName;
        }
    }

}