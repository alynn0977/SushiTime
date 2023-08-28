namespace BreakoutSystem.UI
{
    using Core;
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.UI;

    /// <summary>
    /// Sets and reads a Goal Tile for UI and level purposes.
    /// </summary>
    public class GoalTile : MonoBehaviour, iConstructable<SpriteRenderer, string, string>
    {
        [SerializeField]
        private string goalName;
        
        [SerializeField]
        private Image goalImage;

        [SerializeField]
        private string goalQty;

        public void ConstructWithThree(SpriteRenderer imageValue, string qty, string nameValue)
        {
            if (imageValue == null || string.IsNullOrEmpty(qty) || string.IsNullOrEmpty(nameValue)){
                Debug.LogError("[GoalTile] detected incorrector null parameters.");
                return;
            }

            goalName = nameValue;
            goalImage = GetComponentInChildren<Image>();
            goalImage.sprite = imageValue.sprite;

            goalQty = GetComponentInChildren<TMP_Text>().text = $"x{qty}";
        }
    }

}