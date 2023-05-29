namespace ScreenSystem
{
    using UnityEngine;

    /// <summary>
    /// Scriptable Object that binds a prefab to a screen name.
    /// Scriptable Object is called by ScreenController.
    /// </summary>
    [CreateAssetMenu(menuName = "My Assets/Screen Type")]
    public class ScreenType : ScriptableObject
    {
        [SerializeField]
        private string _screenName;

        [SerializeField]
        private GameObject _screenPrefab;

        /// <summary>
        /// Name of screen. Read-only.
        /// </summary>
        public string ScreenName => _screenName;
        /// <summary>
        /// Object Prefab. Read-only.
        /// </summary>
        public GameObject ScreenPrefab => _screenPrefab;

        private void Awake()
        {
            if (_screenName == null || _screenPrefab == null)
            {
                Debug.LogWarning("ScreenType is missing information!");
            }
        }
    } 
}
