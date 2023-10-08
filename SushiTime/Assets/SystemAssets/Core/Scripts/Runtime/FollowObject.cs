namespace Core
{
    using UnityEngine;

    /// <summary>
    /// Match this object's transform to another's.
    /// </summary>
    public class FollowObject : MonoBehaviour
    {
        /// <summary>
        /// The object to follow.
        /// </summary>
        [SerializeField]
        private Transform targetObject;

        void LateUpdate()
        {
            if (targetObject != null)
            {
                // Set this object's position to the target's position
                transform.position = targetObject.position;
            }
        }
    }
}