namespace ScreenSystem
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using ReadOnlyAttribute = Unity.Collections.ReadOnlyAttribute;

    /// <summary>
    /// Screen Controller class provides access and navigation
    /// through a game screen system.
    /// </summary>
    /// <remarks>Screens for this object are defined by the <see cref="ScreenArray"/> scriptable object.</remarks>
    public class ScreenController : MonoBehaviour
    {
        private const string ModalScreen = "ModalHomeScreen";
        private static Canvas _canvas;

        [Header("To Edit: Find the ScreenArray Scriptable.")]
        [SerializeField]
        [ReadOnly]
        private ScreenType[] _screens;

        private Dictionary<int, ScreenType> _screensCache;
        private RectTransform _currentScreen;
        private RectTransform _homeScreen;
        private RectTransform _lastScreen;
        
        private List<RectTransform> _activeScreens = new();

        /// <summary>
        /// Get list screens available from Screen Controller.
        /// </summary>
        public Dictionary<int, ScreenType> GetScreens
        {
            get
            {
                if (_screensCache == null || _screensCache.Count <= 0)
                {
                    Debug.LogWarning("[ScreenController] is unable to obtain Screen info.");
                    return null;
                }

                return _screensCache;
            }
        }

        /// <summary>
        /// Read-only access of Home Screen assignment.
        /// </summary>
        public RectTransform GetHomeScreen
        {
            get => _homeScreen;
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
                    _canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
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
            _screensCache = new Dictionary<int, ScreenType>();
            for (int i = 0; i < _screens.Length; i++)
            {
                _screensCache.Add(i, _screens[i]);
            }

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

            // Iterate through the dictionary
            foreach (var entry in _screensCache)
            {
                // Check if the value's string field matches the search string
                if (entry.Value.ScreenName == searchByName)
    {
                    GoToScreenIndex(entry.Key);
                    return;
                }
            }

            // The value was not found
            Debug.LogWarning($"ScreenController could not find {searchByName}. Please add one to screen list.");

        }

        public void GoToHomeScreen(bool modalMode)
        {
            if (modalMode)
            {
                GoToScreen(ModalScreen);
            }
            else
            {
                CloseAllScreens();
            }
            
            // Ensure the home screen is active, in case it's off.
            _homeScreen.gameObject.SetActive(true);
        }

        private void CloseAllScreens()
        {
            
            foreach(var screen in _activeScreens)
            {
                if (screen != null)
                {
                    Destroy(screen.gameObject);
                }
            }

            _activeScreens.Clear();
        }

        /// <summary>
        /// Instantiate a game screen by index.
        /// </summary>
        /// <param name="indexInCache">Index of the screen in the array.</param>
        private void GoToScreenIndex(int indexInCache)
        {
            // Confirm index is valid.
            if (_screensCache.ContainsKey(indexInCache))
            {
                if (_currentScreen != null)
                {
                    _lastScreen = _currentScreen;
                    Debug.Log($"[{GetType().Name}]: Caching {_lastScreen}");
                }

                // Instantiate the object into a gameobject.

                var _go = Instantiate(GetScreens[indexInCache].ScreenPrefab);

                if (_go.TryGetComponent(out RectTransform rectTransform))
                {
                    _currentScreen = rectTransform;

                    if (_currentScreen.TryGetComponent(out ScreenTypeBehaviour screenTypeBehaviour))
                    {
                        screenTypeBehaviour.InitializeScreen();
                    }

                    _activeScreens.Add(_currentScreen);
                    CenterAndParentScreen(_currentScreen);
                }
                else
                {
                    // This must be a game screen. Turn off main.
                    Debug.Log($"[{GetType().Name}]: Game Screen detected.");
                    GameObject.Destroy(_lastScreen.gameObject);
                    _currentScreen = _homeScreen;
                    _currentScreen.gameObject.SetActive(false);

                    if (_go.TryGetComponent(out ScreenTypeBehaviour screenTypeBehaviour))
                    {
                        screenTypeBehaviour.InitializeScreen();
                    }
                }
               
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

            if (GetCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                // Set the position, rotation, and scale to 0,0,0.
                screen.anchoredPosition = Vector2.zero;
                screen.sizeDelta = Vector2.zero; 
            }
            else if (GetCanvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                // TODO: ScreenController system is better equipped for screen space overly, for now.
                screen.gameObject.transform.localScale = Vector3.one;
            }
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
