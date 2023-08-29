namespace Core
{
    using UnityEngine;

    /// <summary>
    /// Generic countdown class.
    /// </summary>

    public class CountDown : MonoBehaviour
    {
        [Header("Required")]
        [SerializeField]
        private bool isStartAwake = false;

        [SerializeField]
        [Tooltip("Value in total minutes.")]
        private float startingValue;

        private float remainingTime;

        private bool isReady = false;

        /// <summary>
        /// Read-only access to remaining time.
        /// </summary>
        public float RemainingTime => remainingTime;

        public void SetCountDown(float setValue)
        {
            startingValue = CoreUtilities.ConvertMinutesToSeconds(setValue);
            BeginCountdown();
        }
        private void BeginCountdown()
        {
            if (startingValue != default)
            {
                remainingTime = startingValue;
                isReady = true;
            }
        }

        private void Update()
        {
            if (isReady)
            {
                remainingTime -= Time.deltaTime;
                Debug.Log($"Time Remaining: {remainingTime} total seconds.");
            }
        }
    }

}