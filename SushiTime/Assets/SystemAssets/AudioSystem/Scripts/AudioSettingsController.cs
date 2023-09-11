namespace AudioSystem
{
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Controls Audio Settings. Relays audio events.
    /// Caches to preferences.
    /// </summary>
    public class AudioSettingsController : MonoBehaviour
    {
        private const string GlobalAudioKey = "globalaudiokey";
        private const string GlobalSoundKey = "globalsoundkey";
        private const string GlobalMusicKey = "globalmusickey";

        [SerializeField]
        private Slider globalSlider;
        [SerializeField]
        private Slider volumeSlider;
        [SerializeField]
        private Slider musicSlider;

        /// <summary>
        /// Update master volume via a UI slider.
        /// </summary>
        /// <param name="slider">UI slider.</param>
        public void UpdateMasterVolume(Slider slider)
        {
            UpdateMasterVolume(slider.value);
        }
        /// <summary>
        /// Update Sound Volume via a UI slider.
        /// </summary>
        /// <param name="slider">UI Slider.</param>
        public void UpdateSoundVolume(Slider slider)
        {
            UpdateSoundVolume(slider.value);
        }
        /// <summary>
        /// Update music volume via a UI slider.
        /// </summary>
        /// <param name="slider">UI slider.</param>
        public void UpdateMusicVolume(Slider slider)
        {
            UpdateMusicVolume(slider.value);
        }
        /// <summary>
        /// Update master volume.
        /// </summary>
        /// <param name="volume">Amount for sound.</param>
        public void UpdateMasterVolume(float volume)
        {
            if (EventManager.Instance)
            {
                EventManager.Instance.QueueEvent(new ChangeVolumeEvent(newGlobalVolume: (int)volume));
            }
            else
            {
                NullEventManager();
            }
        }

        /// <summary>
        /// Update Sound volume.
        /// </summary>
        /// <param name="soundVolume">Amount of sound.</param>
        public void UpdateSoundVolume(float soundVolume)
        {
            if (EventManager.Instance)
            {
                EventManager.Instance.QueueEvent(new ChangeVolumeEvent(newGlobalVolume: default, newSoundVolume: (int)soundVolume, newMusicVolume: default));
            }
            else
            {
                NullEventManager();
            }
        }
        /// <summary>
        /// Update music volume.
        /// </summary>
        /// <param name="musicVolume">Amount of sound.</param>
        public void UpdateMusicVolume(float musicVolume)
        {
            if (EventManager.Instance)
            {
                EventManager.Instance.QueueEvent(new ChangeVolumeEvent(newGlobalVolume: default, newSoundVolume: default, newMusicVolume: musicVolume));
            }
            else
            {
                NullEventManager();
            }
        }

        private void NullEventManager()
        {
            Debug.LogWarning($"[{GetType().Name}] cannot find EventManager");
        }

        private void Awake()
        {
            if (globalSlider)
            {
                Debug.Log($"Global Slider found, and should get value of {PlayerPrefs.GetFloat(GlobalAudioKey)}");
                globalSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat(GlobalAudioKey));
            }
        }
    }

}