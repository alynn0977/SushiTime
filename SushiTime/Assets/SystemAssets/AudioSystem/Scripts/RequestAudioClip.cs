namespace AudioSystem
{
    using UnityEngine;

    /// <summary>
    /// Request an audio clip.
    /// </summary>
    public class RequestAudioClip : MonoBehaviour
    {
        [SerializeField]
        private AudioClip clip;

        [SerializeField]
        private float volume = .5f;
        /// <summary>
        /// Play assigned audio clip.
        /// </summary>

        public void PlayAudioClipOther(AudioClip newClip)
        {
            EventManager.Instance.QueueEvent(new RequestAudioClipEvent(volume, newClip));
        }
    }

}