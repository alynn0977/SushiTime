namespace DialogueSystem
{
    using UnityEngine;
    using UnityEditor;

    [CreateAssetMenu(menuName = "My Assets/Character")]
    public class CharacterObject : ScriptableObject
    {
        [SerializeField]
        private string characterName;

        public Sprite CharacterPortrait;
    } 
}
