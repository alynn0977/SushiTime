using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScreenSystem
{
    /// <summary>
    /// A type of game screen.
    /// </summary>
    public class GameScreen : ScreenTypeBehaviour
    {
        public override void InitializeScreen()
        {
            EventManager.Instance.QueueEvent(new FadeScreenEvent(false));

            // Only add listener once, to prevent spamming and errors.
            EventManager.Instance.AddListenerOnce<GameOverScreenEvent>(OnGameOver);
        }

        private void OnGameOver(GameOverScreenEvent e)
        {
            // TODO: Save information goes here.
            // TODO: Stop the music.

            if (e.Delay <= 0)
            {
                CallHomeScreen();
            }
            else
            {
                try
                {
                    Invoke(nameof(CallHomeScreen), e.Delay);
                }
                catch (Exception)
                {
                    Debug.Log($"[{GetType().Name}] has experienced an error while calling home.");
                }
            }

        }

        private void CallHomeScreen()
        {
            EventManager.Instance.QueueEvent(new CallHomeScreenEvent(false));
            CloseGameScreen();
        }

        private void CloseGameScreen()
        {
            Destroy(this.gameObject);
        }
    }
}
