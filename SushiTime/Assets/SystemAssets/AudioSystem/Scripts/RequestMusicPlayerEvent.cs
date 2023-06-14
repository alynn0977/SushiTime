namespace AudioSystem
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Request music from active music player.
    /// </summary>
    public class RequestMusicPlayerEvent : GameEvent
    {
        /// <summary>
        /// Specify name of the music track to play.
        /// </summary>
        public string MusicTrackName;

        /// <summary>
        /// Specify name of music track to play by audio clip.
        /// </summary>
        public AudioClip Track;

        /// <summary>
        /// Request new music track by name.
        /// </summary>
        /// <param name="musicTrack"></param>
        public RequestMusicPlayerEvent(string musicTrack)
        {
            this.MusicTrackName = musicTrack;
        }

        /// <summary>
        /// Request new music track by audio clip file.
        /// </summary>
        /// <param name="newTrack"></param>
        public RequestMusicPlayerEvent(AudioClip newTrack)
        {
            this.Track = newTrack;
        }
    }
}