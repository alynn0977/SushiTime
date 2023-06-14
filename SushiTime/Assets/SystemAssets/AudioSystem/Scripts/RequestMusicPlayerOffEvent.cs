namespace AudioSystem
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Request the music player stop playing tracks.
    /// </summary>
    public class RequestMusicPlayerOffEvent : GameEvent
    {
        /// <summary>
        /// Specify name of the music track to play.
        /// </summary>
        public float Fade;

        /// <summary>
        /// Request new music track by name.
        /// </summary>
        /// <param name="musicTrack"></param>
        public RequestMusicPlayerOffEvent(float fade)
        {
            this.Fade = fade;
        }
    }

}