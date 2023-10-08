namespace BreakoutSystem.UI
{
    using Core;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.UI;

    /// <summary>
    /// Sets and reads a Goal Tile for UI and level purposes.
    /// </summary>
    public class GoalTile : MonoBehaviour, IConstructable<SpriteRenderer, int, string>
    {
        [SerializeField]
        private string goalName;
        
        [SerializeField]
        private Image goalImage;

        [SerializeField]
        private int goalQty;

        private string goalText;
        /// <summary>
        /// Name of the goal tile.
        /// </summary>
        public string GoalName => goalName;

        public int GoalQty => goalQty;
        public void ConstructWithThree(SpriteRenderer imageValue, int qty, string nameValue)
        {
            if (imageValue == null || qty <= 0 || string.IsNullOrEmpty(nameValue)){
                Debug.LogError("[GoalTile] detected incorrect null parameters.");
                return;
            }

            goalName = nameValue;
            goalImage.sprite = imageValue.sprite;
            goalQty = qty;
            goalText = GetComponentInChildren<TMP_Text>().text = $"x{goalQty}";
        }

        /// <summary>
        /// Update goal text and qty.
        /// </summary>
        /// <param name="newQty">Int qty.</param>
        public void UpdateQTY(int newQty)
        {
            goalQty = newQty;
            goalText = GetComponentInChildren<TMP_Text>().text = $"x{goalQty}";
        }
    }

}