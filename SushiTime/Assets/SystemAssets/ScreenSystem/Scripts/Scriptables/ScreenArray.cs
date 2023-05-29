namespace ScreenSystem
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Scriptable Object that provides the list of screens
    /// to be available in this game.
    /// </summary>
    [CreateAssetMenu(menuName = "My Assets/Screens Object")]
    public class ScreenArray : ScriptableObject
    {
        [SerializeField]
        private ScreenType[] _screens;

        /// <summary>
        /// Read-only access to the screen array object.
        /// </summary>
        public ScreenType[] Screens => _screens;
    } 
}
