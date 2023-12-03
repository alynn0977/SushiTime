using Core;
using System;
using UnityEngine;

namespace ScreenSystem
{
    /// <summary>
    /// A type of game screen.
    /// </summary>
    public class GameScreen : ScreenTypeBehaviour, ISystemInitializer
    {
        public override void InitializeScreen()
        {
            Initialize();
        }

        public override void ActivateSystems()
        {
            Debug.Log("Game screen snould be activating.");

            base.ActivateSystems();
        }

        /// <inheritdoc/>
        public void Initialize()
        {
            EventManager.Instance.QueueEvent(new FadeScreenEvent(false));

            EventManager.Instance.AddListenerOnce<FadeScreenPostEvent>(e => ActivateSystems());
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
