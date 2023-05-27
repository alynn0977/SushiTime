namespace Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Gamemanager for the system. Tracks only the most universal
    /// elements of any game (score, lives, time, and if it's paused).
    /// </summary>
    public class GameManager : Singleton
    {
        private int _score;
        private int _lives;
        private float _time;
        private bool _isPaused;
        private void PauseGame()
        {
            // TODO: Write code that pauses the game.
            // TODO: Create a pause menu that gets called from screen manager.
            throw new NotImplementedException();
        }

        private void EndGame()
        {
            throw new NotImplementedException();
        }
    }

}