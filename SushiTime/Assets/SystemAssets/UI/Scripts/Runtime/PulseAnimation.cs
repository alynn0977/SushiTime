namespace CustomUI
{
    using DG.Tweening;
    using UnityEngine;

    public class PulseAnimation : MonoBehaviour
    {
        // This should "pulse" whenever it sees an certain marker.

        // First, handle the lerping.

        // Then handle how to "tie" the system together.

        [SerializeField]
        private float sizePercent;

        private Vector3 _cacheOriginal;

        private void Start()
        {
            _cacheOriginal = transform.localScale;
        }


        [ContextMenu("Pulse")]
        public void Pulse()
        {
            transform.DOScale(_cacheOriginal.x * sizePercent, 1.1f).SetEase(Ease.OutBounce);
            Invoke(nameof(ResetPulse), 1.4f);
        }

        public void ResetPulse()
        {
            transform.DOScale(_cacheOriginal, .5f).SetEase(Ease.Linear);
        }
    }

}