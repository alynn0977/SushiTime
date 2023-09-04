using System;
using UnityEngine;

public static class CoreUtilities
{
    public static void RemoveAllChildObjects(GameObject parentObject)
    {
        var children = parentObject.GetComponentsInChildren<Transform>();
        if (children.Length == 0)
        {
            // Nothing here.
            return;
        }

        for (int i = children.Length - 1; i > 0; i--)
        {
            var child = children[i];
#if UNITY_EDITOR
            UnityEngine.GameObject.DestroyImmediate(child.gameObject);
#else
            UnityEngine.GameObject.Destroy(child.gameObject);
#endif
        }
    }
    
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

    /// <summary>
    /// Convert float to total minutes.
    /// </summary>
    /// <param name="seconds">Total seconds.</param>
    /// <returns>A float representing total minutes.</returns>

    public static float SecToMin(float seconds)
    {
        return Mathf.FloorToInt(seconds / 60);
    }

    /// <summary>
    /// Convert a float to total seconds.
    /// </summary>
    /// <param name="minutes">Total minutes.</param>
    /// <returns>A float representign seconds.</returns>
    public static float MinsToSec(float minutes)
    {
        return minutes * 60;
    }

    /// <summary>
    /// Convert a float (seconds) to a min:sec countdown
    /// </summary>
    /// <param name="time">Total minutes.</param>
    /// <returns>A string in mm:ss format.</returns>
    public static string MinSecCountdown(float time)
    {
        // float minutes = Mathf.FloorToInt(time / 60);
        float minutes = MinCountDOwn(time);

        // float seconds = Mathf.FloorToInt(time % 60);
        float seconds = SecCountdown(time);

        if (seconds < 10)
        {
            return $"{minutes}:0{seconds}";
        }
        else if (minutes < 10)
        {
            return $"0{minutes}:{seconds}";
        }
        else
        {
            return $"{minutes}:{seconds}";
        }
    }

    /// <summary>
    /// Converts total time to seconds only.
    /// </summary>
    /// <param name="time">Total time.</param>
    /// <returns>Seconds as float.</returns>
    public static float SecCountdown(float time)
    {
        return (int)Math.Round(time % 60);
    }

    /// <summary>
    /// Converts total time to minutes only.
    /// </summary>
    /// <param name="time">Total time.</param>
    /// <returns>Minutes as float.</returns>
    public static float MinCountDOwn(float time)
    {
        return Mathf.FloorToInt(time / 60);
    }

    /// <summary>
    /// Creates a total countdown in HH:MM:SS.
    /// </summary>
    /// <returns>Total countdown in HH:MM:SS.</returns>
    public static string ReturnCounterString()
    {
        var seconds = 60 * 15;
        var minutes = seconds / 60;
        seconds %= 3600;
        var hours = seconds / 3600;
        seconds %= 60;

        return $"{hours}:{minutes}:{seconds}";
    }

}
