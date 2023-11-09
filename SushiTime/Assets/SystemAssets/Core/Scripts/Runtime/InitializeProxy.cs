namespace Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
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
        }
    }

}