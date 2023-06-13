namespace AudioSystem
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Request audio track be played by music player.
    /// </summary>
    public class RequestMusicPlayer : MonoBehaviour
    {
        [SerializeField]
        private AudioClip musicTrack;
        public void PlayMusicTrack()
        {
            EventManager.Instance.QueueEvent(new RequestMusicPlayerEvent(musicTrack));
        }

        public void StopMusicTrack(float fade)
        {
            EventManager.Instance.QueueEvent(new RequestMusicPlayerOffEvent(fade));
        } 
    } 
}
