namespace DialogueSystem
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;
    /// <summary>
    /// Reads through a provided <see cref="DialogueObject"/> and
    /// displays it to a UI for users to read a story.
    /// Currently compatible with only two characters.
    /// </summary>
    public class DialogueReader : MonoBehaviour
    {
        private const float DivideScreen = .3f;
        private const float DivideByHalf = .5f;
        private const float DivideByThirds = .33f;

        [Header("Setup")]
        // It needs to know if it should play on awake/enable
        [SerializeField]
        private bool isPlayOnStart = true;

        // This system needs a dialogue object
        [SerializeField]
        private DialogueObject dialogueObject;

        // It needs to know what center frame object it should be displaying characters on
        [SerializeField]
        private RectTransform dialogueFrame;
        private Vector2 thirds;
        private GameObject dialogueHolder;

        // It needs to instantiate the bubble on the proper side of the screen (near character)
        [SerializeField]
        private SpeechObject bubblePrefab;
        [SerializeField]
        private int maxBubblePool = 3;
        [SerializeField]
        [Tooltip("How many bubbles should appear on screen at one time?")]
        private int segmentsPerScreen = 3;
        private List<SpeechObject> bubblePool = new List<SpeechObject>();
        private int currentTrackIndex = 0;

        [Header("Left Character Properties")]
        [SerializeField]
        private bool flipLeftPortrait = true;
        [SerializeField]
        private bool flipLeftArt = false;

        [Header("Right Character Properties")]
        [SerializeField]
        private bool flipRightPortrait = false;
        [SerializeField]
        private bool flipRightArt = false;

        [Header("Prefabs")]
        // It needs to what "Character Frames" it's using
        [SerializeField]
        [Tooltip("Must have iPortrait interface attached.")]
        private GameObject portraitPrefab;

        // It needs to know the max amount of bubbles that can fit on screen.
        // This means it needs to know the size of the dialogue area.
        private Vector2 screenSpace;

        // Needs to flip portraits and bubbles as needed.
        private iPortrait character1;
        private iPortrait character2;

        private string leftCharacterName;
        private string rightCharacterName;

        /// <summary>
        /// Stores the various script lines into an indexed list of tracks.
        /// </summary>
        private Dictionary<int, List<SpeechBubble>> trackList = new Dictionary<int, List<SpeechBubble>>();


        /// <summary>
        /// Initialize a Dialogue Scene. Used if scene does not play on start.
        /// </summary>
        public void InitializeDialogue()
        {
            // Perform null checks first.
            if (!dialogueObject || !dialogueFrame || !portraitPrefab || !bubblePrefab)
            {
                Debug.LogError($"[{GetType().Name}] has missing fields. Check inspector.");
                return;
            }

            DivideScreenByThirds();

            // TODO: Make ONE function that can instantiate either.
            InstantiateLeftCharacter();
            InstantiateRightCharacter();
            
            leftCharacterName = dialogueObject.GetLeftCharacter.GetCharacterName;
            rightCharacterName = dialogueObject.GetRightCharacter.GetCharacterName;
            
            ParseDialogueList();
            InitializeDialogueHolder();

        }

        public IEnumerator PlayFirstTrack()
        {
            yield return new WaitForSeconds(4f);
            PlayCurrentTrack();
        }

        public IEnumerator PlayCurrentTrack()
        {
            Debug.Log($"Now playing track: {currentTrackIndex}");
            if (trackList.ContainsKey(currentTrackIndex))
            {
                int i = 0;
                foreach(var track in trackList[currentTrackIndex])
                {
                    // TODO: Make the wait for seconds a customizeable preference.
                    FireTrack(track, i);
                    i++;
                    yield return new WaitForSeconds(1f);
                }
            }
        }

        private void FireTrack(SpeechBubble track, int poolIndex)
        {
            bubblePool[poolIndex].InitializeSpeechBubble(GetCharacterName(track), ConvertSpeechToString(track),GetCharacterColor(track) );
            if (track.Character == CharacterSide.Left)
            {
                bubblePool[poolIndex].NudgeLeft();
            }
            else
            {
                bubblePool[poolIndex].NudgeRight();
            }
        }

        private void ClearCurrentDialogue()
        {
            Debug.Log("Clearing Dialogue.");
            foreach(var bubble in bubblePool)
            {
                if (bubble.IsActive)
                {
                    bubble.Center();
                    bubble.DisableSpeechBubble();
                }
            }
        }

        public void StepForwardDialogue()
        {
            Debug.Log("Stepping forward.");
            ClearCurrentDialogue();

            if ( (currentTrackIndex + 1) > trackList.Count-1)
            {
                Debug.Log("We're at the end. Next.");
                StartCoroutine(PlayCurrentTrack());
                return;
            }

            // Update track.
            currentTrackIndex++;

            // Now play current.
            StartCoroutine(PlayCurrentTrack());
        }

        public void StepBackwardDialogue()
        {
            Debug.Log("Stepping back.");
            ClearCurrentDialogue();
            if (currentTrackIndex == 0 )
            {
                // We're already at the beginning, so just replay current.
                StartCoroutine(PlayCurrentTrack());
                return;
            }

            

            // Update track.
            currentTrackIndex--;

            StartCoroutine(PlayCurrentTrack());
        }

        /// <summary>
        /// Split the list of speech lines up into tracks.
        /// Uses the <see cref="DialogueReader.segmentsPerScreen"/> variable
        /// to dictate how many lines fit per track.
        /// </summary>
        private void ParseDialogueList()
        {
            // Iterate over the list of items and add them to the dictionary.
            for (int i = 0; i < dialogueObject.TheScript.Length; i++)
            {
                // Get the current key.
                int key = i / segmentsPerScreen;

                // If the key does not exist in the dictionary, create it.
                if (!trackList.ContainsKey(key))
                {
                    trackList.Add(key, new List<SpeechBubble>());
                }

                // Add the item to the group.
                trackList[key].Add(dialogueObject.TheScript[i]);
            }
        }

        private void PlayAllTracks()
        {
            foreach (var track in trackList)
            {
                foreach (var item in track.Value)
                {
                    Debug.Log($"Track {track.Key}: {ConvertSpeechToString(item)}");
                }
            }
        }

        private void InitializeDialogueHolder()
        {
            //**** Working with the bubbles.
            dialogueHolder = dialogueFrame.GetComponentInChildren<VerticalLayoutGroup>().gameObject;

            // Instantiate the bubble pool.
            for (int i = 0; i <= maxBubblePool - 1; i++)
            {
                var bubble = Instantiate(bubblePrefab, dialogueHolder.transform);
                bubblePool.Add(bubble);
                bubble.DisableSpeechBubble();
            }

            dialogueHolder.transform.SetAsLastSibling();
            StartCoroutine(nameof(PlayCurrentTrack));
        }

        private string GetCharacterName(SpeechBubble speechBubble)
        {
            if (string.IsNullOrEmpty(speechBubble.textBubble))
            {
                Debug.LogWarning($"[{GetType().Name}]: Given incorrect speech object.");
                return string.Empty;
            }

            return ReturnCharacterName(speechBubble.Character);
        }
        private string ConvertSpeechToString(SpeechBubble speechBubble) 
        {
            if (string.IsNullOrEmpty(speechBubble.textBubble))
            {
                Debug.LogWarning($"[{GetType().Name}]: Given incorrect speech object.");
                return string.Empty;
            }

            return speechBubble.textBubble;
        }

        private string ReturnCharacterName(CharacterSide side)
        {
            if (side == CharacterSide.Left)
            {
                return leftCharacterName;
            }
            else
            {
                return rightCharacterName;
            }
        }

        private Color GetCharacterColor(SpeechBubble speechBubble)
        {
            if (speechBubble.Character == CharacterSide.Left)
            {
                return dialogueObject.GetLeftCharacter.GetCharacterColor;
            }
            else
            {
                return dialogueObject.GetRightCharacter.GetCharacterColor;
            }
        }

        private void InstantiateLeftCharacter()
        {
            // Calculate a new offset that takes half of thirds x, and .3 of thirds y.
            var offsetLeft = new Vector3(thirds.x * DivideByThirds, ((thirds.y * .3f) * .5f), dialogueFrame.position.z);

            // Instantiate LEFT portrait with the offset. Anchor it to bottom left.
            character1 = Instantiate(portraitPrefab, dialogueFrame.position + offsetLeft, dialogueFrame.rotation, dialogueFrame).GetComponent<iPortrait>();
            character1.CharacterRect.SetAsFirstSibling();
            character1.PortraitRect.anchorMin = Vector2.zero;
            character1.PortraitRect.anchorMax = Vector2.zero;

            if (flipLeftPortrait)
            {
                character1.FlipAsset();
            }

            if (flipLeftArt)
            {
                character1.FlipCharacterArt();
            }

            // Then set it's art.
            character1.SetCharactertArt(
               dialogueObject.GetLeftCharacter.CharacterPortrait,
               dialogueObject.GetLeftCharacter.GetCharacterSize,
               dialogueObject.GetLeftCharacter.GetCharacterOffset);
        }

        private void InstantiateRightCharacter()
        {
            // Calculate a new offset that takes half of thirds x, and .3 of thirds y.
            var offsetRight = new Vector3(thirds.x * DivideByThirds * -1, ((thirds.y * .3f) * .5f), dialogueFrame.position.z);

            // Instantiate RIGHT portrait with the offset. Anchor it to bottom left.
            character2 = Instantiate(portraitPrefab, dialogueFrame.position + offsetRight, dialogueFrame.rotation, dialogueFrame).GetComponent<iPortrait>();
            character2.CharacterRect.SetAsFirstSibling();
            character2.PortraitRect.anchorMin = new Vector2(1, 0);
            character2.PortraitRect.anchorMax = new Vector2(1, 0);

            if (flipRightPortrait)
            {
                character2.FlipAsset();
            }

            if (flipRightArt)
            {
                character2.FlipCharacterArt();
            }

            // Then set it's art.
            character2.SetCharactertArt(
                dialogueObject.GetRightCharacter.CharacterPortrait,
                dialogueObject.GetRightCharacter.GetCharacterSize,
                dialogueObject.GetRightCharacter.GetCharacterOffset);
        }

        /// <summary>
        /// Divide the screen into thirds and cache value.
        /// </summary>
        private void DivideScreenByThirds()
        {
            // I need to find the size of my frame area.
            screenSpace = new Vector2(dialogueFrame.rect.width, dialogueFrame.rect.height);

            // Divide Screen space by thirds.
            thirds = new Vector2(screenSpace.x * DivideScreen, screenSpace.y);
        }

        private void Start()
        {
            if (isPlayOnStart)
            {
                InitializeDialogue();
            }
        }


    }

    

}