namespace CustomUI
{
    using System;
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// A clutch of icons that turn off or on, or provide other icons.
    /// </summary>
    public class IconArray : MonoBehaviour
    {
        [SerializeField]
        private GameObject testObject;

        /// <summary>
        /// Array of available slots.
        /// </summary>
        public Icon[] Icons;

        public void SetIcon(GameObject iconToAdd, float timeToRemain = 1.5f)
        {
            // Early return.
            if (iconToAdd == null)
            {
                Debug.LogError("[IconArray] null referene.");
                return;
            }

            if (!CheckAvailability())
            {
                Debug.LogWarning($"[IconArray] No available icons in {gameObject.name}.");
                return;
            }
            else
            {
                var openSlot = Icons[FindFirstSlot()];
                var go = Instantiate(iconToAdd);
                go.transform.position = openSlot.Slot.transform.position;
                go.transform.SetParent(openSlot.Slot.transform);
                openSlot.isAvailable = false;

                var convertTime = CoreUtilities.MinsToSec(timeToRemain);
                Debug.Log($"Reseting this slot after: {convertTime}");
                StartCoroutine(DeleteChildAfterDelay(openSlot, convertTime));
            }
        }

        [ContextMenu("Set Icon")]
        [ExecuteInEditMode]
        public void SetIconTest()
        {
            if (testObject == null)
            {
                Debug.LogWarning("[IconArray] Set Test Object Please.");
                return;
            }

            SetIcon(testObject);
        }

        [ContextMenu("Reset Icons")]
        private void ResetIcons()
        {
            for (int i = 0; i < Icons.Length; i++)
            {
                CoreUtilities.RemoveAllChildObjects(Icons[i].Slot);
                Icons[i].isAvailable = true;
            }
        }

        private IEnumerator DeleteChildAfterDelay(Icon slotToReset, float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            CoreUtilities.RemoveAllChildObjects(slotToReset.Slot);
            slotToReset.isAvailable = true;
        }

        private bool CheckAvailability()
        {
            foreach (var icon in Icons)
            {
                if (icon.isAvailable)
                {
                    return true;
                }
            }

            return false;
        }

        private int FindFirstSlot()
        {
            int i;
            for (i = 0; i < Icons.Length; i++)
            {
                if (Icons[i].isAvailable)
                {
                    Icons[i].isAvailable = false;
                    return i;
                }
            }

            return default;
        }

        private void Awake()
        {
            if (Icons.Length == 0)
            {
                Debug.LogError($"[{gameObject.name}] Icon Array is empty.");
                gameObject.SetActive(true);
                return;
            }

            ResetIcons();
        }
    }

    /// <summary>
    /// Icon struct object to support an <see cref="IconArray"/>
    /// </summary>
    [Serializable]
    public struct Icon
    {
        /// <summary>
        /// The slot in the array.
        /// </summary>
        public GameObject Slot;
        
        /// <summary>
        /// Confirms if Slot is open.
        /// </summary>
        public bool isAvailable;

        /// <summary>
        /// Current gameobject icon on this slot.
        /// </summary>
        public GameObject ActiveIcon;
    }

}