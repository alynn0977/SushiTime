namespace CustomUI
{
    using PrimeTween;
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// Handles the logic that takes care of showing and hiding the pop-up.
    /// Pop-up disappears after <see cref="appearForSeconds"/>.
    /// </summary>
    public class PopUpGraphic : MonoBehaviour
    {
        [SerializeField]
        private GameObject popupPanel;
        [SerializeField]
        private int appearForSeconds = 8;
        [SerializeField]
        private Ease easeType = Ease.OutSine;
        public void CallPopUp()
        {
            popupPanel.SetActive(true);
            popupPanel.transform.localScale = new Vector2(.8f, .8f);
            Tween.Scale(transform, endValue: 1.1f, duration: .65f, ease: easeType, endDelay: 0.5f, cycles: 9, cycleMode: CycleMode.Yoyo);
            StartCoroutine(DisableAfterSeconds(appearForSeconds));
        }

        public void CallPopUp(int changeAppearanceTime)
        {
            StartCoroutine(DisableAfterSeconds(changeAppearanceTime));
        }

        public void KillPopUp()
        {
            Tween.Scale(transform, endValue: 0f, duration: .45f, ease: easeType, endDelay: 0.15f);
            popupPanel.SetActive(false);
        }

        private void Start()
        {
            popupPanel.SetActive(false);
        }

        private IEnumerator DisableAfterSeconds(int seconds)
        {
            yield return new WaitForSeconds(seconds);
            KillPopUp();
        }
    } 
}
