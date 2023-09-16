namespace AudioSystem
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Same as <see cref="RequestAudioClip"/>, but can
	/// play from a pool of multiple clips.
	/// </summary>

	public class RequestMultiAudioClip : MonoBehaviour
	{
        private const string GlobalSoundKey = "globalsoundkey";

        [SerializeField]
        private AudioClip[] clipPool;
        [SerializeField]
        private bool useGlobalAudio = true;
        [SerializeField]
        private float volume = .5f;
		[SerializeField]
		private bool isRandomized = true;

        /// <summary>
        /// Play assigned audio clip.
        /// </summary>
        [ContextMenu("Play Audio Clip")]
        public void PlayAudioClips()
        {
            AudioClip audioClip = default;
            if (clipPool.Length <= 0)
            {
                Debug.LogWarning($"[{GetType().Name}] does not have any audio clips assigned.");
                return;
            }

            if (isRandomized)
            {
                audioClip = clipPool[Random.Range(0, clipPool.Length - 1)];
            }

            // Debug.Log($"[{GetType().Name}] Requesting audio clip {newClip.name}");
            // Or default straight to global.
            if (useGlobalAudio)
            {
                var newVolume = PlayerPrefs.GetFloat(GlobalSoundKey);

                EventManager.Instance.QueueEvent(new RequestAudioClipEvent(volume, audioClip));
            }
            // Or override entirely.
            else
            {
                EventManager.Instance.QueueEvent(new RequestAudioClipEvent(volume, audioClip));
            }
        }
    }
}
