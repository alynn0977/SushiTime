namespace AudioSystem
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PlayAudio : MonoBehaviour
    {
        [SerializeField]
        private AudioSource audioSource;

        [SerializeField]
        private AudioClip audioClip;

        public float volume = 0.5f;

        public void PlayAudioClip()
        {
            Debug.LogWarning($"[{this.GetType()}] playing audio clip on {gameObject.name}.");
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