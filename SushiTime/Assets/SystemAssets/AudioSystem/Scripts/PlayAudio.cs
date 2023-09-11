namespace AudioSystem
{
    using UnityEngine;

    public class PlayAudio : MonoBehaviour
    {
        [SerializeField]
        private AudioSource audioSource;

        [SerializeField]
        private AudioClip audioClip;

        public float volume = 1f;

        public void PlayAudioClip()
        {
            audioSource.PlayOneShot(audioClip, volume);
        }

        private void Awake()
        {
            if (audioSource == null || audioClip == null)
            {
                Debug.LogWarning($"[{this.GetType()}] on {gameObject.name} is missing references.");
            }
        }
    }

}