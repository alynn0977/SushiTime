using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScreenSystem
{
    /// <summary>
    /// Abstract to define behaviours of a screen type.
    /// </summary>
    public abstract class ScreenTypeBehaviour : MonoBehaviour
    {
        public abstract void InitializeScreen();
    }
}
