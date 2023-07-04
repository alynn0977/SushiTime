using DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDialogue : MonoBehaviour
{
    [SerializeField]
    private DialogueObject dialoguObject;

    [SerializeField]
    private int index;

    [SerializeField]
    private SpeechObject speechObject;

    [ContextMenu("Test 1")]
    public void InitializeTest()
    {
        if (speechObject)
        {
            var tryThis = dialoguObject.TheScript[index].textBubble;
            string tryName;

            if (dialoguObject.TheScript[index].Character == CharacterSide.Left)
            {
                tryName = dialoguObject.GetLeftCharacter.GetCharacterName;
            }
            else
            {
                tryName = dialoguObject.GetRightCharacter.GetCharacterName;
            }

            speechObject.InitializeSpeechBubble(tryName, tryThis);
        }
    }

    [ContextMenu("Test 2")]
    public void InitializeTest2()
    {
        speechObject.DisableSpeechBubble();
    }
}
