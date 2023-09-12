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
        private const string GlobalSoundKey = "globalsoundkey";
        private const string GlobalMusicKey = "globalmusickey";

        [SerializeField]
        private Slider volumeSlider;
        [SerializeField]
        private Slider musicSlider;


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
        /// Update Sound volume.
        /// </summary>
        /// <param name="soundVolume">Amount of sound.</param>
        public void UpdateSoundVolume(float soundVolume)
        {
            if (EventManager.Instance)
            {
                EventManager.Instance.QueueEvent(new ChangeVolumeEvent(soundVolume));
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
                var currentVolume = PlayerPrefs.GetFloat(GlobalSoundKey);
                EventManager.Instance.QueueEvent(new ChangeVolumeEvent(currentVolume, musicVolume));
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
            if (volumeSlider)
            {
                volumeSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat(GlobalSoundKey) * 10);
            }
            if (musicSlider)
            {
                musicSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat(GlobalMusicKey) * 10);
            }
        }
    }

}