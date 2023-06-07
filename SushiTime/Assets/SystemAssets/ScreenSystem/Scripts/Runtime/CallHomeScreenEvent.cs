namespace ScreenSystem
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Call the homescreen event.
    /// </summary>
    public class CallHomeScreenEvent : GameEvent
    {
        /// <summary>
        /// Call a modal screen to confirm first.
        /// </summary>
        public bool ModalMode;

        /// <summary>
        /// Event to close all screens but hom.
        /// </summary>
        /// <param name="modalMode">Ask to confirm first.</param>
        public CallHomeScreenEvent(bool modalMode)
        {
            this.ModalMode = modalMode;
        }
    } 
}
