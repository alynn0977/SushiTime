using AudioSystem;
using CarterGames.Assets.AudioManager;
using Core;
using ScreenSystem;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

/// <summary>
/// Provides connection to <see cref="AppManager"/>
/// and other global level commands.
/// </summary>
public class GameManager : MonoBehaviour
{
    private const string GlobalAudioKey = "globalaudiokey";
    private const string GlobalSoundKey = "globalsoundkey";
    private const string GlobalMusicKey = "globalmusickey";

    [TabGroup("Prefabs")]
    [SerializeField]
    private GameObject audioPrefab;

    [TabGroup("Debug")]
    [SerializeField]
    private int TestGlobalVolume;
    [TabGroup("Debug")]
    [SerializeField]
    private int TestAudioVolume;
    [TabGroup("Debug")]
    [SerializeField]
    private int TestMusicVolume;

    private const string _screenTypeName = "SushiTime";
    private ScreenController _screenController;
    private AudioManager _audioController;
    private MusicPlayer _musicPlayer;
    private RectTransform _homeScreen;

    private void Start()
    {
        InitializeScreenController();

        InitializeSoundPref();

        InitializeAudioController();

        EventManager.Instance.AddListener<PauseGameEvent>(OnPauseGameEvent);
    }

    #region Initialize
    private void InitializeScreenController()
    {
        _screenController = ServiceLocator.GetService<ScreenController>();
        _screenController.Initialize(_screenTypeName);
        EventManager.Instance.AddListener<CallNewScreenGameEvent>(OnGameManagerOnNewScreen);
        EventManager.Instance.AddListener<CallHomeScreenEvent>(OnGameManagerOnCallHome);
        _homeScreen = _screenController.GetHomeScreen;
    }

    private void InitializeAudioController()
    {
        _audioController = Instantiate(audioPrefab, this.transform.parent).GetComponent<AudioManager>();
        _musicPlayer = _audioController.GetComponent<MusicPlayer>();
        EventManager.Instance.AddListener<RequestMusicPlayerEvent>(OnGameManagerOnPlayMusic);
        EventManager.Instance.AddListener<RequestMusicPlayerOffEvent>(OnGameManagerOnPlayMusicOff);
        EventManager.Instance.AddListener<RequestAudioClipEvent>(OnGameManagerOnPlayClip);
        EventManager.Instance.AddListener<ChangeVolumeEvent>(OnGameManagerVolumeChange);
    }
    #endregion

    #region Application Commmands
    private void OnPauseGameEvent(PauseGameEvent e)
    {
        // TODO: When or when not to pause music? 

        //if (AppManager.IsGlobalPaused)
        //{
        //    _musicPlayer.SetVolume(0);
        //}

        //if (!AppManager.IsGlobalPaused)
        //{
        //    _musicPlayer.SetVolume(PlayerPrefs.GetFloat(GlobalMusicKey));
        //}
    }
    #endregion

    #region Screen Commands
    private void OnGameManagerOnCallHome(CallHomeScreenEvent e)
    {
        // Go to the modal screen first, if Is Modal is true.
        _screenController.GoToHomeScreen(e.IsModalScreen);

        // Handle Homescreen Restarting music.
        if (!e.IsModalScreen)
        {
            // Play right away if no modal required.
            _homeScreen.GetComponent<ScreenMusicProperties>().RestartMusic();
            EventManager.Instance.QueueEvent(new FadeScreenEvent(false));
        }

    }

    private void OnGameManagerOnNewScreen(CallNewScreenGameEvent e)
    {
        _screenController.GoToScreen(e.ScreenName);
    }
    #endregion

    #region Music & Sound Commands
    [ContextMenu("Test Global Volume")]
    private void TestGlobal()
    {
        PlayerPrefs.SetFloat(GlobalAudioKey, TestGlobalVolume);
        _musicPlayer.SetVolume(PlayerPrefs.GetFloat(GlobalAudioKey));
    }
    [ContextMenu("Test Audio Volume")]
    private void TestAudio()
    {
        PlayerPrefs.SetFloat(GlobalSoundKey, TestAudioVolume);
    }
    [ContextMenu("Test Music Volume")]
    private void TestMusic()
    {
        PlayerPrefs.SetFloat(GlobalMusicKey, TestMusicVolume);
        _musicPlayer.SetVolume(PlayerPrefs.GetFloat(GlobalMusicKey));
    }
    private static void InitializeSoundPref()
    {
        // If no player prefs, set from scratch.
        if (!PlayerPrefs.HasKey(GlobalSoundKey))
        {
            PlayerPrefs.SetFloat(GlobalAudioKey, 10);
            PlayerPrefs.SetFloat(GlobalSoundKey, 10);
            PlayerPrefs.SetFloat(GlobalMusicKey, 10);
        }
        
    }
    private void OnGameManagerOnPlayMusic(RequestMusicPlayerEvent e)
    {
        _musicPlayer.SetVolume(PlayerPrefs.GetFloat(GlobalMusicKey));
        _musicPlayer.PlayTrack(e.Track);
    }

    private void OnGameManagerOnPlayMusicOff(RequestMusicPlayerOffEvent e)
    {
        _musicPlayer.StopTrack();
    }

    private void OnGameManagerOnPlayClip(RequestAudioClipEvent e)
    {
        var volume = PlayerPrefs.GetFloat(GlobalSoundKey);
        _audioController.Play(e.Clip.name, volume);
    }

    private void OnGameManagerVolumeChange(ChangeVolumeEvent e)
    {
        if (e.NewSoundVolume != default)
        {
            Debug.Log($"Changing sound to {PlayerPrefs.GetFloat(GlobalSoundKey, e.NewSoundVolume * .1f)}");
            PlayerPrefs.SetFloat(GlobalSoundKey, e.NewSoundVolume * .1f);
        }

        if (e.NewMusicVolume != default)
        {
            Debug.Log($"Changing sound to {PlayerPrefs.GetFloat(GlobalSoundKey, e.NewSoundVolume * .1f)}");
            PlayerPrefs.SetFloat(GlobalMusicKey,e.NewMusicVolume * .1f);
            _musicPlayer.SetVolume(PlayerPrefs.GetFloat(GlobalMusicKey));
        }
    }

    #endregion
}
