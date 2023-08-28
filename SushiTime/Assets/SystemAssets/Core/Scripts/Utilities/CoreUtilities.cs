using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoreUtilities
{
    /// <summary>
    /// Add or get a component from an object.
    /// </summary>
    /// <typeparam name="T">Component type.</typeparam>
    /// <param name="obj">Gameobject to check.</param>
    /// <returns></returns>
    public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
    {
        // Try to get the Component
        var component = obj.GetComponent<T>();

        // If the Component doesn't exist, add it
        if (!component)
        {
            component = obj.AddComponent<T>();
        }

        // Return the Component
        return component;
    }
}
