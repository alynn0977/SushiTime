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
        }
    }
}
