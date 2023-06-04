namespace ScreenSystem
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Screen Controller class provides access and navigation
    /// through a game screen system.
    /// </summary>
    public class ScreenController : MonoBehaviour
    {
        [SerializeField]
        private ScreenType[] _screens;
        private RectTransform _currentScreen;
        private RectTransform _homeScreen;
        private RectTransform _lastScreen;
        private Canvas _canvas;
        

        /// <summary>
        /// Get list screens available from Screen Controller.
        /// </summary>
        public ScreenType[] GetScreens
        {
            get
            {
                if (_screens == null || _screens.Length <= 0)
                {
                    Debug.LogWarning("[ScreenController] is unable to obtain Screen info.");
                    return null;
                }

                return _screens;
            }
        }

        /// <summary>
        /// Get current canvas active in screen.
        /// </summary>
        public Canvas GetCanvas
        {
            get
            {
                if (_canvas == null)
                {
                    _canvas = FindObjectOfType<Canvas>();
                }

                return _canvas;
            }
        }

        /// <summary>
        /// Initialize new Screen Controller service.
        /// </summary>
        /// <param name="screenObject">Which object array in resources to initialize with.</param>
        public void Initialize(string screenObject)
        {
            _screens = Resources.Load<ScreenArray>(screenObject).Screens;
            InitializeHomeScreen();
        }

        /// <summary>
        /// Go to screen as specified by name.
        /// </summary>
        /// <param name="searchByName">Name of screen to search for.</param>
        public void GoToScreen(string searchByName)
        {
            // Null string check.
            if (String.IsNullOrEmpty(searchByName))
            {
                Debug.LogError($"ScreenController given bad/null string: {searchByName}");
                return;
            }

            foreach (ScreenType screenType in GetScreens)
            {
                if (screenType.name == searchByName)
                {
                    int i = Array.IndexOf(GetScreens, screenType);

                    // Pass index off.
                    GoToScreenIndex(i);
                    break;
                }
            }

            Debug.LogWarning($"Screen Name {searchByName} is not assigned to the screen array list.");
        }

        /// <summary>
        /// Instantiate a game screen by index.
        /// </summary>
        /// <param name="indexInArray">Index of the screen in the array.</param>
        private void GoToScreenIndex(int indexInArray)
        {
            // Confirm index is valid.
            if (IsValidIndex(indexInArray))
            {
                if (_currentScreen != null)
                {
                    _lastScreen = _currentScreen;
                    Debug.Log($"Caching {_lastScreen}");
                }

                // Instantiate the object into a gameobject.
                _currentScreen = Instantiate(GetScreens[indexInArray].ScreenPrefab).GetComponent<RectTransform>();
                CenterAndParentScreen(_currentScreen);
            }
            else
            {
                Debug.LogError("ScreenController was provided bad index.");
                return;
            }
        }

        private void InitializeHomeScreen()
        {
            _homeScreen = Instantiate(GetScreens[0].ScreenPrefab).GetComponent<RectTransform>();
            CenterAndParentScreen(_homeScreen);
        }

        private void CenterAndParentScreen(RectTransform screen)
        {
            // Set Canvas as the parent.
            screen.SetParent(GetCanvas.transform);

            // Set the position, rotation, and scale to 0,0,0.
            screen.anchoredPosition = Vector2.zero;
            screen.sizeDelta = Vector2.zero;
        }

        private bool IsValidIndex(int index)
        {
            // Check if the index is within the bounds of the array.
            if (index < 0 || index >= _screens.Length)
            {
                return false;
            }

            // The index is valid.
            return true;
        }
    } 
}
