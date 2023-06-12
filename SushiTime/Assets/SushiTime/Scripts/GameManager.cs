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

    private const string screenTypeName = "SushiTime";
    private ScreenController screenController;
    private AudioManager audioController;
    private MusicPlayer musicPlayer;

    private void Start()
    {
        InitializeScreenController();
        InitializeAudioController();
    }

    private void InitializeScreenController()
    {
        screenController = ServiceLocator.GetService<ScreenController>();
        screenController.Initialize(screenTypeName);
        EventManager.Instance.AddListener<CallNewScreenGameEvent>(GameManagerOnNewScreen);
        EventManager.Instance.AddListener<CallHomeScreenEvent>(GameManagerOnCallHome);
    }

    private void InitializeAudioController()
    {
        audioController = Instantiate(audioPrefab, this.transform.parent).GetComponent<AudioManager>();
        musicPlayer = audioController.GetComponent<MusicPlayer>();
        EventManager.Instance.AddListener<RequestMusicPlayerEvent>(GameManagerOnPlayMusic);
    }

    private void GameManagerOnCallHome(CallHomeScreenEvent e)
    {
        screenController.GoToHomeScreen(e.ModalMode);
    }

    private void GameManagerOnNewScreen(CallNewScreenGameEvent e)
    {
        screenController.GoToScreen(e.ScreenName);
    }

    private void GameManagerOnPlayMusic(RequestMusicPlayerEvent e)
    {
        musicPlayer.PlayTrack(e.Track);
    }
}
