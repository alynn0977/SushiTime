namespace Core
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    
    /// <summary>
    /// Service locator creates instances of vital classes and dispatches them.
    /// </summary>

    public class ServiceLocator : MonoBehaviour
    {
        private static Dictionary<Type, object> services = new();

        /// <summary>
        /// Get a specific service, or create instance of it if null.
        /// </summary>
        /// <typeparam name="T">Type of object to retrieve.</typeparam>
        /// <returns></returns>
        public static T GetService<T>() where T : Component
        {
            if (!services.ContainsKey(typeof(T)))
            {
                GameObject gameObject = Activator.CreateInstance<GameObject>();
                gameObject.name = typeof(T).Name;
                services[typeof(T)] = gameObject.AddComponent<T>();
            }

            return services[typeof(T)] as T;
        }

        // TODO: Create disposable.
        private static void Dispose()
        {
            foreach (var service in services.Values)
            {
                if (service != null)
                {
                    // TODO: Make an iDisposable.
                }
            }

            services.Clear();
        }
    } 
}
