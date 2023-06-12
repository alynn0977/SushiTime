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

        private void Start()
        {
            if (!backgroundMusic)
            {
                Debug.LogWarning($"[{GetType().Name}] {gameObject.name} missing audio track.");
                return;
            }
            else
            {
                EventManager.Instance.QueueEvent(new RequestMusicPlayerEvent(backgroundMusic));
            }
        }

        private void OnDisable()
        {
            // TODO: Fade the music.
        }
    }

}