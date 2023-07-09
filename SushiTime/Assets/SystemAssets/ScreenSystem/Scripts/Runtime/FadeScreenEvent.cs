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
        /// Send commands to the screen fade system.
        /// </summary>
        /// <param name="fadeIn">If true, screen fades to solid. If false, fades away.</param>
        public FadeScreenEvent(bool fadeIn)
        {
            UnityEngine.Debug.Log($"Screen fade called with {fadeIn}");
            this.FadeIn = fadeIn;
        }
    }

}