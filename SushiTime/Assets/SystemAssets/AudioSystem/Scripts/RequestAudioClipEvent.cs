namespace AudioSystem
{
    using UnityEngine;

    /// <summary>
    /// Request the music player stop playing tracks.
    /// </summary>
    public class RequestAudioClipEvent : GameEvent
    {
        /// <summary>
        /// Specify name of the music track to play.
        /// </summary>
        public float Volume;

        /// <summary>
        /// Specify name of the music track to play.
        /// </summary>
        public AudioClip Clip;

        /// <summary>
        /// Request new music track by name.
        /// </summary>
        /// <param name="musicTrack"></param>
        public RequestAudioClipEvent(float volume, AudioClip clip)
        {
            this.Volume = volume;
            this.Clip = clip;
        }
    }

}