using DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogueObject))]
public class DialogueObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {

        // Check if the leftCharacter property exists.
        if (serializedObject.FindProperty("leftCharacter") != null)
        {
            EditorGUILayout.BeginHorizontal();
            {
                //*** Left Character
                var leftCharacter = (CharacterObject)serializedObject.FindProperty("leftCharacter").objectReferenceValue;
                GUILayout.Label(leftCharacter.CharacterPortrait.texture, GUILayout.Width(100), GUILayout.Height(100));

                GUILayout.FlexibleSpace();

                var rightCharacter = (CharacterObject)serializedObject.FindProperty("rightCharacter").objectReferenceValue;
                GUILayout.Label(rightCharacter.CharacterPortrait.texture, GUILayout.Width(100), GUILayout.Height(100));

            }
            EditorGUILayout.EndHorizontal();
            DrawGUILine();
        }

        base.OnInspectorGUI();

    }

    private void DrawGUILine(int i_height = 1)
    {

        Rect rect = EditorGUILayout.GetControlRect(false, i_height);

        rect.height = i_height;

        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
    }

}
