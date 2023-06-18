namespace DialogueSystem
{
    using UnityEngine;
    using UnityEditor;

    [CreateAssetMenu(menuName = "My Assets/Character")]
    public class CharacterObject : ScriptableObject
    {
        [Header("Character Info")]
        [SerializeField]
        private string characterName;

        [Header("Framing Properties")]
        [SerializeField]
        [Tooltip("What size should the character be, when in frame?")]
        private Vector2 characterSize;

        [SerializeField]
        [Tooltip("Where should the character appear, when in frame?")]
        private Vector3 characterOffset = new Vector3(0, 0, 0);

        /// <summary>
        /// Read-only access to character name.
        /// </summary>
        public string GetCharacterName => characterName;

        /// <summary>
        /// Read-only access to character sprite;
        /// </summary>
        public Sprite CharacterPortrait;

        /// <summary>
        /// Read-only access to what size character should be in the frame.
        /// </summary>
        public Vector2 GetCharacterSize => characterSize;

        /// <summary>
        /// Read-only access to how character should be offset in a frame.
        /// </summary>
        public Vector3 GetCharacterOffset => characterOffset;
    } 
}
