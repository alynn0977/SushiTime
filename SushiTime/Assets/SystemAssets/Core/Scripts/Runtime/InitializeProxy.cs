namespace Core
{
    using System;
    using UnityEngine;

    [Serializable]
    public class InitializeProxy : MonoBehaviour, ISystemInitializer
    {
        [SerializeField]
        private MonoBehaviour subSystem;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Initialize()
        {
            if(subSystem.TryGetComponent(out ISystemInitializer initialize))
            {
                initialize.Initialize();
            }
            else
            {
                Debug.LogWarning($"[{GetType().Name}]: Sorry, {subSystem.name} is not a proper interface.");
            }
        }
    }

}