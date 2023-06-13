namespace AudioSystem
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Specify the music properties of a given
    /// screen.
    /// </summary>
    public class ScreenMusicProperties : MonoBehaviour
    {
        [SerializeField]
        private AudioClip backgroundMusic;

        [SerializeField]
        private bool isFadeIn;

        [SerializeField]
        private bool isFadeOut;

        private AudioClip BackgroundMusic
        {
            get
            {
                if (backgroundMusic == null)
                {
                    Debug.LogWarning($"[{GetType().Name}] {gameObject.name} missing audio track.");
                    return null;
                }

                return backgroundMusic;
            }
        }
        public void RestartMusic()
        {
            if (BackgroundMusic)
            {
                EventManager.Instance.QueueEvent(new RequestMusicPlayerEvent(backgroundMusic));
            }
        }
        private void OnEnable()
        {
            if (BackgroundMusic)
            {
                Invoke(nameof(DelayPlayMusic), 1f);
            }
        }

        private void DelayPlayMusic()
        {
            EventManager.Instance.QueueEvent(new RequestMusicPlayerEvent(backgroundMusic));
        }
    }

}