namespace ScreenSystem
{
    using PrimeTween;
    using UnityEngine;

    /// <summary>
    /// Provide a fade to a canvas.
    ///
    /// NOTE: It is assumed that devs are either using DoTween, or an Animator
    /// If an Animator is not serialized, it defaults to DoTween.
    /// </summary>
    public class FadeController : MonoBehaviour
    {
        private const float AutoFadeDelay = 1.2f;
        [Header("Main Options")]
        [SerializeField]
        private float time = 1f;

        [Header("Setup")]
        [SerializeField]
        private CanvasGroup canvasGroup;

        private void Start()
        {
            if (!canvasGroup)
            {
                Debug.LogError($"[FadeController] on {gameObject.name} is missing references.");
                this.enabled = false;
                return;
            }

            EventManager.Instance.AddListener<FadeScreenEvent>(ScreenFade);
            
            if (canvasGroup.alpha == 1)
            {
                FadeAway();
            }
        }

        public void ScreenFade(FadeScreenEvent e)
        {
            if (e.FadeIn)
            {
                FadeIn();
                Debug.Log($"FadeController received call fade in.");
                if (e.AutoTransition)
                {
                    Invoke(nameof(FadeAway), AutoFadeDelay);
                }
            }
            else
            {
                FadeAway();
                Debug.Log($"FadeController received call fade out.");
            }
        }

        /// <summary>
        /// Screen will fade to color.
        /// </summary>
        [ContextMenu("Fade In")]
        public void FadeIn()
        {
            // Animate the Canvas Group from 1 to 0.
            Tween.Custom(0, 1, duration: time, onValueChange: newVal => canvasGroup.alpha = newVal);

            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = true;
        }

        /// <summary>
        /// Dark screen will fade.
        /// </summary>
        [ContextMenu("Fade Away")]
        public void FadeAway()
        {
            // Animate the Canvas Group from 1 to 0.
            Tween.Custom(1, 0, duration: time, onValueChange: newVal => canvasGroup.alpha = newVal).OnComplete(() => FadeScreenPostEvent());

            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = false;
        }

        private void FadeScreenPostEvent()
        {
            EventManager.Instance.QueueEvent(new FadeScreenPostEvent());
        }
    }

}