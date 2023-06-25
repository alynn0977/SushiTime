namespace DialogueSystem
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Defines the behaviours for the speech bubbles.
    /// </summary>
    public class SpeechObject : MonoBehaviour
    {
        // A speech object needs to know the following:
        // What character name.
        [SerializeField]
        private TMP_Text characterName;
       
        // What speech will show up?
        [SerializeField]
        private TMP_Text characterText;

        // Must be able to turn itself on and off.
        [SerializeField]
        private GameObject speechContainer;

        [SerializeField]
        private Color defaultColor;

        private RectTransform thisRect;
        // It must be able to nudge left or right.
        private HorizontalLayoutGroup layoutGroup;
        [SerializeField]
        private int offsetAmount = 14;
        
        /// <summary>
        /// Returns true if Speech Bubble is in active use.
        /// </summary>
        public bool IsActive
        {
            get;
            private set;
        }

        public void InitializeSpeechBubble(string initializeName, string initializeText, Color newTextColor)
        {
            characterText.color = newTextColor;
            InitializeSpeechBubble(initializeName, initializeText);
        }

        public void InitializeSpeechBubble(string initializeName, string initializeText)
        {
            Debug.Log($"[{GetType().Name}]: Initializing Speech Bubble.");
            if (!characterName || !characterText || !speechContainer)
            {
                Debug.LogError($"[{GetType().Name}]: {gameObject.name} has missing references. Check inspector.");
                return;
            }
            thisRect = GetComponent<RectTransform>();
            layoutGroup = GetComponent<HorizontalLayoutGroup>();
            characterName.text = initializeName;
            characterText.text = initializeText;
            
            EnableAll();
            LayoutRebuilder.ForceRebuildLayoutImmediate(characterText.rectTransform);
        }

        public void DisableSpeechBubble()
        {
            characterText.color = defaultColor;
            speechContainer.gameObject.SetActive(false);
            IsActive = false;
        }

        [ContextMenu("Nudge Left")]
        public void NudgeLeft()
        {
            layoutGroup.padding.left = -offsetAmount;
            LayoutRebuilder.ForceRebuildLayoutImmediate(thisRect);
        }

        [ContextMenu("Nudge Right")]
        public void NudgeRight()
        {
            layoutGroup.padding.left = offsetAmount;
            LayoutRebuilder.ForceRebuildLayoutImmediate(thisRect);
        }

        [ContextMenu("Center")]
        public void Center()
        {
            layoutGroup.padding.left = 0;
            LayoutRebuilder.ForceRebuildLayoutImmediate(thisRect);
        }

        private void EnableAll()
        {
            speechContainer.gameObject.SetActive(true);
            ResizeCharacterBox();
            ResizeTextBox();
            IsActive = true;
        }

        private void ResizeCharacterBox()
        {
            float resizedNameBox = characterName.preferredWidth + 10;
            characterName.rectTransform.sizeDelta = new Vector2(resizedNameBox, characterName.rectTransform.sizeDelta.y);
        }
        
        private void ResizeTextBox()
        {
            var resizedTextHeight = characterText.preferredHeight + 6;
            characterText.rectTransform.sizeDelta = new Vector2(characterText.rectTransform.sizeDelta.x, resizedTextHeight);
            thisRect.sizeDelta = new Vector2(thisRect.sizeDelta.x, resizedTextHeight+12);
        }
    } 
}
