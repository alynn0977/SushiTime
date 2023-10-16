namespace AudioSystem
{
    using UnityEngine;

    /// <summary>
    /// Request an audio clip.
    /// </summary>
    public class RequestAudioClip : MonoBehaviour
    {
        private const string GlobalSoundKey = "globalsoundkey";

        [SerializeField]
        private AudioClip clip;
        [SerializeField]
        private bool useGlobalAudio = true;
        [SerializeField]
        private float volume = .5f;
        /// <summary>
        /// Play assigned audio clip.
        /// </summary>
        [ContextMenu("Play Audio Clip")]
        public void PlayAudioClipOther(AudioClip newClip)
        {
            // Or default straight to global.
            if (useGlobalAudio)
            {
                var newVolume = PlayerPrefs.GetFloat(GlobalSoundKey); 
                if (newVolume < 0)
                {
                    newVolume = 0;
                }

                EventManager.Instance.QueueEvent(new RequestAudioClipEvent(newVolume, newClip));
            }
            // Or override entirely.
            else
            {
                EventManager.Instance.QueueEvent(new RequestAudioClipEvent(volume, newClip));
            }
        }
    }
}