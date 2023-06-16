using DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharacterObject))]
public class CharacterObjectEditor : Editor
{
    public Sprite sprite;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // Get the thumbnail for the sprite asset.
        var characterSprite = AssetPreview.GetAssetPreview(serializedObject.FindProperty("CharacterPortrait").objectReferenceValue);
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            // Display the thumbnail in the inspector.
            GUILayout.Label(characterSprite, GUILayout.Width(160), GUILayout.Height(160));

            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndHorizontal();

    }
}
