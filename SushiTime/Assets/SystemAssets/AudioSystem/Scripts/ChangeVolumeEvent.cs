namespace AudioSystem
{
    using System;
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
        public float NewVolume;

        public VolumeType VolumeToSet;

        public ChangeVolumeEvent(float newSoundVolume, VolumeType volumeToSet)
        {
            this.NewVolume = newSoundVolume;
            this.VolumeToSet = volumeToSet;
        }
    }

    /// <summary>
    /// Define what volume this is changing.
    /// </summary>
    [Serializable]
    public enum VolumeType
    {
        Sound = 0,
        Music = 1,
    }

}