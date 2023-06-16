namespace DialogueSystem
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(menuName = "My Assets/Dialogue")]
    public class DialogueObject : ScriptableObject
    {
        [Header("Characters")]
        // First, need left character.
        [SerializeField]
        private CharacterObject leftCharacter;

        // Then, need right character.
        [SerializeField]
        private CharacterObject rightCharacter;

        [Space(20)]
        [Header("Dialogue")]
        // Then an arrive of lots o' dialogue a plenty.
        public SpeechBubble[] TheScript;

    }

    [Serializable]
    public struct SpeechBubble
    {
        public CharacterSide Character;

        [TextArea(1, 20)]
        public string textBubble;
    }

    [Serializable]
    public enum CharacterSide
    {
        Left,
        Right,
    }

}