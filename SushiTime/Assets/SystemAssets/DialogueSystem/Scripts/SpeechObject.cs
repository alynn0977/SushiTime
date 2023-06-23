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
        // It must resize the rectTransform to fit the character name.

        [SerializeField]
        [Tooltip("What is max characters bubble should have on one line?")]
        private int maxWidth = 33;

        // There's only so much a user will read. C'mon.
        private int maxStringLength = 100;

        public void InitializeSpeechBubble(string initializeName, string initializeText)
        {
            Debug.Log($"[{GetType().Name}]: Initializing Speech Bubble.");
            if (!characterName || !characterText || !speechContainer)
            {
                Debug.LogError($"[{GetType().Name}]: {gameObject.name} has missing references. Check inspector.");
                return;
            }

            characterName.text = initializeName;
            characterText.text = initializeText;

            EnableAll();
            LayoutRebuilder.ForceRebuildLayoutImmediate(characterText.rectTransform);
        }

        public void DisableSpeechBubble()
        {
            speechContainer.gameObject.SetActive(false);
        }

        private void EnableAll()
        {
            speechContainer.gameObject.SetActive(true);
            ResizeCharacterBox();
            ResizeTextBox();
        }

        private void ResizeCharacterBox()
        {
            float resizedNameBox = characterName.preferredWidth + 10;
            characterName.rectTransform.sizeDelta = new Vector2(resizedNameBox, characterName.rectTransform.sizeDelta.y);
        }
        
        private void ResizeTextBox()
        {
            var resizedTextHeight = characterText.preferredHeight + 6;
            // var resizedTextWidth = characterText.preferredWidth + 5;
            characterText.rectTransform.sizeDelta = new Vector2(characterText.rectTransform.sizeDelta.x, resizedTextHeight);
        }

    } 
}
