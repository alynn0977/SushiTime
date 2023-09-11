namespace AudioSystem
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    /// <summary>
    /// Fire an event to change audio volume.
    /// </summary>
    public class ChangeVolumeEvent : GameEvent
    {
        /// <summary>
        /// Set volume for both sound and music.
        /// </summary>
        public float NewGlobalVolume;

        /// <summary>
        /// Change volume for sound FX.
        /// </summary>
        public float NewSoundVolume;

        /// <summary>
        /// Change volume for music.
        /// </summary>
        public float NewMusicVolume;

        /// <summary>
        /// Build a new Change Volume event.
        /// </summary>
        /// <param name="newGlobalVolume">Specify new master volume.</param>
        /// <param name="newSoundVolume">Specify new sound FX volume.</param>
        /// <param name="newMusicVolume">Specify new music volume.</param>
        public ChangeVolumeEvent(float newGlobalVolume = default, float newSoundVolume = default, float newMusicVolume = default)
        {
            this.NewGlobalVolume = newGlobalVolume;
            this.NewSoundVolume = newSoundVolume;
            this.NewMusicVolume = newMusicVolume;
        }
    }

}