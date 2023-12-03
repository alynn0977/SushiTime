namespace CustomUI
{
    using Core;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class ModalController : MonoBehaviour, ISystemInitializer
	{
        [Header("Screens")]
        [SerializeField]
        private GameObject[] popUpsToActivate;
        [Header("Startup")]
        [SerializeField]
        private bool isInitializeOnAwake = false;
        [Space]
        [Header("Controls & Animation")]
        [SerializeField]
        private Button backButton;
        [SerializeField]
        private Button forwardButton;
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private string animationTrigger = "TurnOff";

        [Space]
        [Header("On Pop Up End")]
        [SerializeField]
        private UnityEvent onPopUpEnd = new UnityEvent();

        private int modalIndex;

        public void Initialize()
        {
            InitiateFirstPopup();
        }

        public void InitiateFirstPopup()
        {
            if (animator == null)
            {
                Debug.LogWarning($"[{GetType().Name}]: Null reference to animator component. Cannot initialize.");
                return;
            }

            popUpsToActivate[0].SetActive(true);
            backButton.interactable = false;
            modalIndex = 0;
        }

        public void NextPopUp()
        {
            if (modalIndex < popUpsToActivate.Length - 1)
            {
                // Deactivate first screen
                popUpsToActivate[modalIndex].SetActive(false);

                // Update index.
                modalIndex = modalIndex + 1;

                // Open new screen.
                popUpsToActivate[modalIndex].SetActive(true);

                // Should back button be on?
                if (modalIndex <= 0)
                {
                    backButton.interactable = true;
                }
            }
            else if ( modalIndex == popUpsToActivate.Length - 1)
            {
                // This indicates we're at the end. Turn off screen.
                if (animator != null)
                {
                    animator.SetTrigger(animationTrigger);
                }
                else
                {
                    popUpsToActivate[modalIndex].SetActive(false);
                }

                onPopUpEnd?.Invoke();

                KillModal();
            }
        }

        public void PriorPopup()
        {
            if (modalIndex != 0)
            {
                // Deactivate current screen
                popUpsToActivate[modalIndex].SetActive(false);

                // Update index.
                modalIndex = modalIndex - 1;

                // Open prior screen.
                popUpsToActivate[modalIndex].SetActive(true);

                // Should back button be on?
                if (modalIndex == 0)
                {
                    backButton.interactable = false;
                }
            }
        }

        public void KillModal()
        {
            this.gameObject.SetActive(false);
        }

        public void InitializeNext(MonoBehaviour component)
        {
            component.enabled = true;
        }

        private void Start()
        {
            if (popUpsToActivate == null || popUpsToActivate.Length == 0)
            {
                Debug.LogWarning($"[{GetType().Name}] - {gameObject.name}: You're an idiot and should make better choices.");
                return;
            }

            if (isInitializeOnAwake)
            {
                InitiateFirstPopup();
            }
        }
    }

}