namespace BreakoutSystem.UI
{
    using Core;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Sets and reads a Goal Tile for UI and level purposes.
    /// </summary>
    public class GoalTile : iConstructable<SpriteRenderer, string, string>
    {
        [SerializeField]
        private string goalName;
        
        [SerializeField]
        private Image goalImage;

        [SerializeField]
        private string goalQty;

        public void ConstructWithThree(SpriteRenderer imageValue, string qty, string nameValue)
        {
            goalName = nameValue;
            goalImage.sprite = imageValue.sprite;
            goalQty = qty;
        }
    }

}