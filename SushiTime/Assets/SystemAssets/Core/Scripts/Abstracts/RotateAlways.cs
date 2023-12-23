namespace Core
{
    using UnityEngine;

    /// <summary>
    /// Perpetually rotates an object around one axis, at a constant speed.
    /// </summary>
    public class RotateAlways : MonoBehaviour
    {
        [SerializeField]
        private bool isRotateOnAwake = false;
        [SerializeField]
        private Transform target;
        [SerializeField]
        private float speed;
        [SerializeField]
        private Vector3 rotationalAxis;

        private bool isReady;

        public void BeginRotation()
        {
            isReady = true;
        }

        private void Start()
        {
            if (isRotateOnAwake)
            {
                isReady = true;
            }

            if (target == null)
            {
                target = transform;
                Debug.LogWarning($"[{GetType().Name}]: Null target. Defaulting to self.");
            }
        }

        private void Update()
        {
            if (isReady)
            {
                target.RotateAround(target.position, rotationalAxis, speed * Time.deltaTime);
                Debug.Log($"[{GetType().Name}]: Rotating {target.gameObject.name}");
            }
        }
    }
}