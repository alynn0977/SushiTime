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
        /// Change volume for sound FX.
        /// </summary>
        public float NewSoundVolume;

        /// <summary>
        /// Change volume for music.
        /// </summary>
        public float NewMusicVolume;


        public ChangeVolumeEvent(float newSoundVolume)
        {
            this.NewSoundVolume = newSoundVolume;
        }

        public ChangeVolumeEvent(float newSoundVolume, float newMusicVolume) 
        {
            this.NewSoundVolume = newSoundVolume;
            this.NewMusicVolume = newMusicVolume;
        }
    }

}