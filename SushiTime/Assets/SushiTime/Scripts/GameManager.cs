using AudioSystem;
using CarterGames.Assets.AudioManager;
using Core;
using ScreenSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject audioPrefab;

    private const string _screenTypeName = "SushiTime";
    private ScreenController _screenController;
    private AudioManager _audioController;
    private MusicPlayer _musicPlayer;
    private RectTransform _homeScreen;

    private void Start()
    {
        InitializeScreenController();
        InitializeAudioController();
    }

    #region Initialize
    private void InitializeScreenController()
    {
        _screenController = ServiceLocator.GetService<ScreenController>();
        _screenController.Initialize(_screenTypeName);
        EventManager.Instance.AddListener<CallNewScreenGameEvent>(GameManagerOnNewScreen);
        EventManager.Instance.AddListener<CallHomeScreenEvent>(GameManagerOnCallHome);
        _homeScreen = _screenController.GetHomeScreen;
    }

    private void InitializeAudioController()
    {
        _audioController = Instantiate(audioPrefab, this.transform.parent).GetComponent<AudioManager>();
        _musicPlayer = _audioController.GetComponent<MusicPlayer>();
        EventManager.Instance.AddListener<RequestMusicPlayerEvent>(GameManagerOnPlayMusic);
        EventManager.Instance.AddListener<RequestMusicPlayerOffEvent>(GameManagerOnPlayMusicOff);
        EventManager.Instance.AddListener<RequestAudioClipEvent>(GameManagerOnPlayClip);
    }

    private void GameManagerOnPlayClip(RequestAudioClipEvent e)
    {
        _audioController.Play(e.Clip.name, volume: e.Volume);
    }
    #endregion

    private void GameManagerOnCallHome(CallHomeScreenEvent e)
    {
        _screenController.GoToHomeScreen(e.ModalMode);

        // Handle Homescreen Restarting music.
        if (!e.ModalMode)
        {
            // Play right away if no modal required.
            _homeScreen.GetComponent<ScreenMusicProperties>().RestartMusic();
        }
    }

    private void GameManagerOnNewScreen(CallNewScreenGameEvent e)
    {
        _screenController.GoToScreen(e.ScreenName);
    }



    private void GameManagerOnPlayMusic(RequestMusicPlayerEvent e)
    {
        _musicPlayer.PlayTrack(e.Track);
    }

    private void GameManagerOnPlayMusicOff(RequestMusicPlayerOffEvent e)
    {
        _musicPlayer.StopTrack();
    }
}
