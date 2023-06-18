namespace DialogueSystem
{
    using Core;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class CharacterPortrait : MonoBehaviour, iPortrait
    {
        private static Vector2 Vector2Half = new Vector2(.5f, .5f);
        [Header("Setup")]
        [SerializeField]
        private Image characterSprite;
        [SerializeField]
        private RectTransform characterFrame;
        [Space]
        [Header("Misc")]
        [SerializeField]
        [Tooltip("If given bad vector size, this will be the default.")]
        private Vector2 defaultSize = new Vector2(200f, 200f);

        public RectTransform PortraitRect
        {
            get => gameObject.GetComponent<RectTransform>();
        }
        public RectTransform CharacterRect 
        {
            get => characterSprite.rectTransform;
        }

        private Image CharacterSprite
        {
            get
            {
                if (!characterSprite)
                {
                    Debug.LogError($"[{GetType().Name}]: {gameObject.name} sprite is not assigned.");
                    return null;
                }

                return characterSprite;
            }
        }
        
        /// <summary>
        /// Flip the entire portrait asset.
        /// </summary>
        public void FlipAsset()
        {
            gameObject.transform.localScale = new Vector3(
                gameObject.transform.localScale.x * -1,
                gameObject.transform.localScale.y,
                gameObject.transform.localScale.z);
        }

        public void FlipCharacterArt()
        {
            CharacterSprite.transform.localScale = new Vector3(
                CharacterSprite.transform.localScale.x * -1,
                CharacterSprite.transform.localScale.y,
                CharacterSprite.transform.localScale.z);
        }

        public void SetCharactertArt(Sprite sprite, Vector2 size, Vector3 offset)
        {
            CharacterSprite.sprite = sprite;

            if (!characterFrame)
            {
                Debug.LogWarning($"[{GetType().Name}]: {gameObject.name} is missing field references and cannot resize.");
                return;
            }

            CharacterSprite.preserveAspect = true;
            CharacterSprite.rectTransform.anchorMin = Vector2Half;
            CharacterSprite.rectTransform.anchorMax = Vector2Half;

            if (size.Equals(Vector2.zero))
            {
                Debug.LogWarning($"[{GetType().Name}]: {gameObject.name} was given zero size vector. Defaulting to one instead.");
                CharacterSprite.rectTransform.sizeDelta = new Vector2(size.x, size.y);
            }
            else
            {
                CharacterSprite.rectTransform.sizeDelta = new Vector2(size.x, size.y);
            }
            
            CharacterSprite.rectTransform.position = CharacterSprite.rectTransform.position + offset;



        }
    } 
}
