namespace BreakoutSystem
{
    using Core;
    // using DG.Tweening;
    using PrimeTween;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Behaviour for paddle interaction.
    /// </summary>
    public class PaddleBehaviour : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private bool isReady = true;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Rect gameZone;

        [Header("Swing Options")]
        [SerializeField]
        private Vector3 Swing = new Vector3(0,0, 22f);

        [Header("Interaction Actions")]
        [Tooltip("Specify actions for when paddle interaction is called.")]
        public UnityEvent OnInteraction = new UnityEvent();

        /// <inheritdoc/>
        public void Interact()
        {
            OnInteraction?.Invoke();
        }

        private void Start()
        {
            mainCamera = Camera.main;
            EventManager.Instance.AddListener<PauseGameEvent>(OnPauseGame);
        }

        private void OnPauseGame(PauseGameEvent e)
        {
            if (e.IsPause)
            {
                isReady = false;
            }
            else
            {
                isReady = true;
            }
        }

        private void Update()
        {
            if (isReady)
            {
                MoveByMouse();
            }
        }

        private void MoveByMouse()
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            if (gameZone.Contains(mousePosition))
            {
                transform.position = new Vector3(mousePosition.x, transform.position.y, transform.position.z);

                if (Input.GetMouseButtonDown(0))
                {
                    SwingPaddle(Swing * -1);
                }

                if (Input.GetMouseButtonDown(1))
                {
                    SwingPaddle(Swing);
                }
            }
        }

        private void SwingPaddle(Vector3 vector3)
        {
            var time = .05f;

            Tween.LocalRotation(transform, endValue: vector3, duration: time).OnComplete(() => 
            {
                Tween.LocalRotation(transform, Vector3.zero, .24f);
            });
        }

        /// <summary>
        /// Use this gizmo to define the playable
        /// area where the paddle must follow the 
        /// </summary>
        [ExecuteInEditMode]

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(gameZone.center, new Vector3(gameZone.width, gameZone.height, 0));
        }
    } 
}