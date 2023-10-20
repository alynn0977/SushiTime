namespace Core
{
    using UnityEngine;
    using UnityEngine.Events;

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

        [Header("Optional")]
        public UnityEvent OnTick = new UnityEvent();

        private float remainingTime;
        private bool isReady = false;

        /// <summary>
        /// Read-only access to remaining time.
        /// </summary>
        public float RemainingTime => remainingTime;

        [Tooltip("True = counts down. False = counts up from 0.")]
        public bool IsCountDown
        {
            get;
            private set;
        }

        public void SetCountDown(float setValue, bool isCountDown)
        {
            IsCountDown = isCountDown;
            startingValue = CoreUtilities.MinsToSec(setValue);
            BeginCountdown();
        }

        private void Awake()
        {
            if (isStartAwake && startingValue <= 0)
            {
                Debug.LogWarning($"[{GetType().Name}] can't start with 0.");
                return;
            }
            else if (isStartAwake && startingValue >= 0)
            {
                BeginCountdown();
            }
        }
        private void BeginCountdown()
        {
            remainingTime = IsCountDown ? startingValue : 0f;
            isReady = true;
        }

        private void Update()
        {
            if (isReady)
            {
                if (IsCountDown)
                {
                    remainingTime -= Time.deltaTime;
                }
                else
                {
                    remainingTime += Time.deltaTime;
                }

                if (CoreUtilities.SecCountdown(remainingTime) % 10 <= 0)
                {
                    OnTick?.Invoke();
                }
            }
        }
    }

}