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
    private AudioClip _currentClip;
    private RectTransform _homeScreen;

    /// <summary>
    /// Global volume of the game.
    /// </summary>
    public int GlobalVolume
    {
        get { return PlayerPrefs.GetInt(GlobalAudioKey); }
        set { PlayerPrefs.SetInt(GlobalAudioKey, value); }
    }

    /// <summary>
    /// Sound volume of the game.
    /// </summary>
    public int GlobalSound
    {
        get { return PlayerPrefs.GetInt(GlobalSoundKey); }
        set { PlayerPrefs.SetInt(GlobalSoundKey, value); }
    }

    /// <summary>
    /// Sound volume of the game.
    /// </summary>
    public int GlobalMusic
    {
        get { return PlayerPrefs.GetInt(GlobalMusicKey); }
        set { PlayerPrefs.SetInt(GlobalMusicKey, value); }
    }

    private void Start()
    {
        InitializeScreenController();

        InitializeSoundPref();

        InitializeAudioController();

        EventManager.Instance.AddListener<PauseGameEvent>(OnPauseGameEvent);
    }

    private void OnPauseGameEvent(PauseGameEvent e)
    {
        if (e.IsPause && _musicPlayer.IsTrackPlaying)
        {
            _musicPlayer.SetVolume(0);
            _musicPlayer.StopTrack();
        }

        if (!e.IsPause)
        {
            _musicPlayer.SetVolume(PlayerPrefs.GetInt(GlobalMusicKey));
        }
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
        EventManager.Instance.AddListener<ChangeAudioEvent>(OnGameManagerVolumeChange);
    }
    #endregion

    #region Screen Commands
    private void OnGameManagerOnCallHome(CallHomeScreenEvent e)
    {
        _screenController.GoToHomeScreen(e.ModalMode);
        // Handle Homescreen Restarting music.
        if (!e.ModalMode)
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
        Debug.Log("Test Global");
        PlayerPrefs.SetInt(GlobalAudioKey, TestGlobalVolume);
        _musicPlayer.SetVolume(PlayerPrefs.GetFloat(GlobalAudioKey));
    }
    [ContextMenu("Test Audio Volume")]
    private void TestAudio()
    {
        Debug.Log("Test Audio");
        PlayerPrefs.SetInt(GlobalSoundKey, TestAudioVolume);
    }
    [ContextMenu("Test Music Volume")]
    private void TestMusic()
    {
        Debug.Log("Test Music");
        PlayerPrefs.SetInt(GlobalMusicKey, TestMusicVolume);
        _musicPlayer.SetVolume(PlayerPrefs.GetFloat(GlobalMusicKey));
    }
    private static void InitializeSoundPref()
    {
        if (!PlayerPrefs.HasKey(GlobalAudioKey))
        {
            PlayerPrefs.SetInt(GlobalAudioKey, 10);
            PlayerPrefs.SetInt(GlobalSoundKey, 10);
            PlayerPrefs.SetInt(GlobalMusicKey, 10);
        }
    }
    private void OnGameManagerOnPlayMusic(RequestMusicPlayerEvent e)
    {
        _musicPlayer.SetVolume(GlobalMusic);
        _musicPlayer.PlayTrack(e.Track);
    }

    private void OnGameManagerOnPlayMusicOff(RequestMusicPlayerOffEvent e)
    {
        _musicPlayer.StopTrack();
    }

    private void OnGameManagerOnPlayClip(RequestAudioClipEvent e)
    {
        _audioController.Play(e.Clip.name, GlobalSound);
    }

    private void OnGameManagerVolumeChange(ChangeAudioEvent e)
    {
        if (e.NewGlobalVolume != default)
        {
            Debug.Log($"[GameManager] now changing global volume to {e.NewGlobalVolume}");
            GlobalVolume = e.NewGlobalVolume;
            GlobalSound = e.NewGlobalVolume;
            GlobalMusic = e.NewGlobalVolume;

            _musicPlayer.SetVolume(GlobalMusic);
        }

        if (e.NewSoundVolume != default)
        {
            GlobalSound = e.NewSoundVolume;
        }

        if (e.NewMusicVolume != default)
        {
            GlobalMusic = e.NewMusicVolume;
            _musicPlayer.SetVolume(GlobalMusic);
        }
    }

    #endregion
}
