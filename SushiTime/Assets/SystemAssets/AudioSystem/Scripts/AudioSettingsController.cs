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

        private void OnEnable()
        {
            if (volumeSlider)
            {
                volumeSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat(GlobalSoundKey));
            }
            if (musicSlider)
            {
                musicSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat(GlobalMusicKey));
            }
        }
    }

}