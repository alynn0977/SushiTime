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

        public void PlayAudioClipOther(AudioClip newClip)
        {
            // If set to use Global Audio,
            // Then intercept and change.

            if (useGlobalAudio)
            {
                volume = PlayerPrefs.GetFloat(GlobalSoundKey);
            }

            EventManager.Instance.QueueEvent(new RequestAudioClipEvent(volume, newClip));
        }
    }

}