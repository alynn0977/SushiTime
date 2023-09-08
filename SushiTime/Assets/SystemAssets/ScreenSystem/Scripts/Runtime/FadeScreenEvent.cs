namespace ScreenSystem
{
    /// <summary>
    /// Control the Fade System with this event.
    /// </summary>
    public class FadeScreenEvent : GameEvent
    {
        /// <summary>
        /// Name of the screen to call.
        /// </summary>
        public bool FadeIn;

        /// <summary>
        /// If true, the Fade Controller
        /// will automatically fade away.
        /// </summary>
        public bool AutoTransition = false;

        /// <summary>
        /// Send commands to the screen fade system.
        /// </summary>
        /// <param name="fadeIn">If true, screen fades to solid. If false, fades away.</param>
        public FadeScreenEvent(bool fadeIn)
        {
            UnityEngine.Debug.Log($"Screen fade called with {fadeIn}");
            this.FadeIn = fadeIn;
        }

        /// <summary>
        /// Send commands to the screen fade system.
        /// </summary>
        /// <param name="fadeIn">If true, screen fades to solid. If false, fades away.</param>
        public FadeScreenEvent(bool fadeIn, bool autoTransition = false)
        {
            UnityEngine.Debug.Log($"Screen fade called with {fadeIn}");
            this.FadeIn = fadeIn;
            this.AutoTransition = autoTransition;
        }
    }

}