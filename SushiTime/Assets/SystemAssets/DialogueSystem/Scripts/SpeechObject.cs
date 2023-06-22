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
        private int maxWidth = 30;

        public void InitializeSpeechBubble(string initializeName, string initializeText)
        {
            Debug.Log($"[{GetType().Name}]: Initializing Speech Bubble.");
            if (!characterName || !characterText || !speechContainer)
            {
                Debug.LogError($"[{GetType().Name}]: {gameObject.name} has missing references. Check inspector.");
                return;
            }

            characterName.text = initializeName;
            characterText.text = ProcessedText(initializeText);

            EnableAll();
        }

        public void DisableSpeechBubble()
        {
            speechContainer.gameObject.SetActive(false);
        }

        private void EnableAll()
        {
            ResizeCharacterBox();
            ResizeTextBox();

            speechContainer.gameObject.SetActive(true);
        }

        private string ProcessedText(string oldString)
        {
            // Create a StringBuilder to hold the new string.
            StringBuilder newString = new StringBuilder();

            // Iterate over the characters in the old string.
            int i = 0;
            while (i < oldString.Length)
            {
                // If the current character is a space, and the next character is less than 30 characters away,
                // insert a linebreak.
                if (oldString[i] == ' ' && i + 30 <= oldString.Length && oldString[i + 30] != ' ')
                {
                    newString.Append("\n");
                }

                // Append the current character to the new string.
                newString.Append(oldString[i]);

                // Increment the counter.
                i++;
            }

            // Return the new string.
            return newString.ToString();
        }

        private void ResizeCharacterBox()
        {
            float resizedNameBox = characterName.preferredWidth + 10;
            characterName.rectTransform.sizeDelta = new Vector2(resizedNameBox, characterName.rectTransform.sizeDelta.y);
        }
        
        private void ResizeTextBox()
        {
            var resizedTextHeight = characterText.preferredHeight + 6;
            var resizedTextWidth = characterText.preferredWidth + 5;
            characterText.rectTransform.sizeDelta = new Vector2(resizedTextWidth, resizedTextHeight);
        }

    } 
}
