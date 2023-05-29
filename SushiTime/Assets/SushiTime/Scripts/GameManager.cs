using Core;
using ScreenSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    private const string screenTypeName = "SushiTime";
    private ScreenController screenController;

    private void Start()
    {
        InitializeScreenController();
    }

    private void InitializeScreenController()
    {
        screenController = ServiceLocator.GetService<ScreenController>();
        screenController.Initialize(screenTypeName);
        EventManager.Instance.AddListener<CallNewScreenGameEvent>(GameManagerOnNewScreen);
    }

    private void GameManagerOnNewScreen(CallNewScreenGameEvent e)
    {
        screenController.GoToScreen(e.ScreenName);
    }
}
