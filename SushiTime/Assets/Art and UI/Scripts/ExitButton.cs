using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ExitButton : MonoBehaviour
{
    private Button button;
    
    /// <summary>
    /// Exit game or application.
    /// </summary>
    public void ExitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();

    }

    private void Awake()
    {
        button = GetComponent<Button>();
    }
}
