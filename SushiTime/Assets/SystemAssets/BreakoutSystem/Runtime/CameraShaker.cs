namespace BreakoutSystem
{
    using PrimeTween;
    using UnityEngine;

    public class CameraShaker : MonoBehaviour
    {
        [SerializeField]
        private float cameraShakeStrength = 0.5f;
        [SerializeField]
        private float delay = .7f;
        private Camera mainCamera;

        public void ShakeCamera()
        {
            Tween.ShakeCamera(mainCamera, cameraShakeStrength, startDelay: delay);
        }

        private void Start()
        {
            mainCamera = Camera.main;
        }
    }
}