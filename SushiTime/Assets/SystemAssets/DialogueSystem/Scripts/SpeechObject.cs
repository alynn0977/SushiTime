namespace DialogueSystem
{
    using Core;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Defines the behaviours for the speech bubbles.
    /// </summary>
    public class SpeechObject : MonoBehaviour, IFlippable
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
        private Image flippable;

        [SerializeField]
        private Sprite[] switchableSpriteAssets;

        private RectTransform thisRect;

        private characterSide thisSide = characterSide.center;
        private Animator anim;
#pragma warning disable CS0649 // Add readonly modifier
        private string OnInitialize;
#pragma warning restore CS0649 // Add readonly modifier
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
            // Debug.Log($"[{GetType().Name}]: Initializing Speech Bubble.");
            if (!characterName || !characterText || !speechContainer)
            {
                Debug.LogError($"[{GetType().Name}]: {gameObject.name} has missing references. Check inspector.");
                return;
            }
            layoutGroup = GetComponent<HorizontalLayoutGroup>();
            characterName.text = initializeName;
            characterText.text = initializeText;

            anim = GetComponent<Animator>();
            if (anim)
            {
                Animator.StringToHash("OnOpenSpeechBubble");
            }

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
            thisSide = characterSide.left;
            FlipAsset();
            LayoutRebuilder.ForceRebuildLayoutImmediate(ThisRect);
        }

        [ContextMenu("Nudge Right")]
        public void NudgeRight()
        {
            layoutGroup.padding.left = offsetAmount;
            thisSide = characterSide.right;
            FlipAsset();
            LayoutRebuilder.ForceRebuildLayoutImmediate(ThisRect);
        }

        [ContextMenu("Center")]
        public void Center()
        {
            layoutGroup.padding.left = 0;
            thisSide = characterSide.center;
            FlipAsset();
            LayoutRebuilder.ForceRebuildLayoutImmediate(ThisRect);
        }

        public void FlipAsset()
        {

            if (!flippable || switchableSpriteAssets.Length == 0)
            {
                return;
            }

            switch (thisSide)
            {
                case characterSide.left:
                    flippable.sprite = switchableSpriteAssets[1];
                    break;
                case characterSide.right:
                    flippable.sprite = switchableSpriteAssets[2];
                    break;
                default:
                    flippable.sprite = switchableSpriteAssets[0];
                    break;
            }

        }

        [ContextMenu("Resize Character Box")]
        protected void ResizeCharacterBox()
        {
            float resizedNameBox = characterName.preferredWidth + 10;
            characterName.rectTransform.sizeDelta = new Vector2(resizedNameBox, characterName.rectTransform.sizeDelta.y);
        }

        [ContextMenu("Resize Text Box")]
        protected void ResizeTextBox()
        {
            var resizedTextHeight = characterText.preferredHeight+6;
            characterText.rectTransform.sizeDelta = new Vector2(characterText.rectTransform.sizeDelta.x, resizedTextHeight);
            ThisRect.sizeDelta = new Vector2(ThisRect.sizeDelta.x, resizedTextHeight + 12);

            if (anim)
            {
                anim.enabled = true;
                anim.Play(OnInitialize);
            }
        }

        /*
        protected void RefitTails()
        {
            //if (!tailLeft || !tailRight)
            //{
            //    return;
            //}

            // Get the size of the character rectTransform size delta, to inform where to place tails.
            //var rect tailLeft.anchoredPosition = new Vector2(0, -(characterText.rectTransform.sizeDelta.y + padding));
            //tailRight.anchoredPosition = new Vector2(characterText.rectTransform.sizeDelta.x, -(characterText.rectTransform.sizeDelta.y + padding));

            //LayoutRebuilder.ForceRebuildLayoutImmediate(ThisRect);
        }*/

        private void EnableAll()
        {
            speechContainer.gameObject.SetActive(true);
            ResizeCharacterBox();
            ResizeTextBox();
            FlipAsset();
            IsActive = true;
        }

        private enum characterSide
        {
            center,
            left,
            right,
        }
    } 
}
