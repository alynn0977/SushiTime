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
        [Header("Text Setup")]
        [SerializeField]
        private TMP_Text characterName;

        [SerializeField]
        private Color defaultColor;

        // What speech will show up?
        [SerializeField]
        private TMP_Text characterText;

        [Header("Container Setup")]
        // Must be able to turn itself on and off.
        [SerializeField]
        private GameObject speechContainer;

        // It must be able to nudge left or right.
        private HorizontalLayoutGroup layoutGroup;
        [SerializeField]
        private int offsetAmount = 14;

        [Header("Optional")]
        [SerializeField]
        private RectTransform tailLeft;
        [SerializeField]
        private RectTransform tailRight;
        private RectTransform thisRect;

        private RectTransform ThisRect
        {
            get 
            { 
                if (!thisRect)
                {
                    thisRect = GetComponent<RectTransform>();
                }

                return thisRect;
            }
        }

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

            DisableTails();
            IsActive = false;
        }

        [ContextMenu("Nudge Left")]
        public void NudgeLeft()
        {
            layoutGroup.padding.left = -offsetAmount;
            
            if (tailLeft)
            {
                tailLeft.gameObject.SetActive(true);
            }
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(ThisRect);
        }

        [ContextMenu("Nudge Right")]
        public void NudgeRight()
        {
            layoutGroup.padding.left = offsetAmount;
            
            if (tailRight)
            {
                tailRight.gameObject.SetActive(true);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(ThisRect);
        }

        [ContextMenu("Center")]
        public void Center()
        {
            layoutGroup.padding.left = 0;
            LayoutRebuilder.ForceRebuildLayoutImmediate(ThisRect);
        }

        [ContextMenu("Refit Tails")]
        protected void RefitTails()
        {
            if (!tailLeft || !tailRight)
            {
                return;
            }

            int padding = 22;
            // Get the size of the rect transform.
            tailLeft.anchoredPosition = new Vector2(0, -(characterText.rectTransform.sizeDelta.y + padding));
            tailRight.anchoredPosition = new Vector2(characterText.rectTransform.sizeDelta.x, -(characterText.rectTransform.sizeDelta.y + padding));

            LayoutRebuilder.ForceRebuildLayoutImmediate(ThisRect);
        }


        private void EnableAll()
        {
            speechContainer.gameObject.SetActive(true);
            ResizeCharacterBox();
            ResizeTextBox();
            RefitTails();
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
            ThisRect.sizeDelta = new Vector2(ThisRect.sizeDelta.x, resizedTextHeight+12);
            
        }

        private void DisableTails()
        {
            if (tailLeft || tailRight)
            {
                tailLeft.gameObject.SetActive(false);
                tailRight.gameObject.SetActive(false);
            }
        }
    } 
}
