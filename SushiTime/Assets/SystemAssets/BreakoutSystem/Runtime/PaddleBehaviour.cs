namespace BreakoutSystem
{
    using Core;
    using PrimeTween;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.InputSystem;

    /// <summary>
    /// Behaviour for paddle interaction.
    /// </summary>
    public class PaddleBehaviour : MonoBehaviour, IInteractable, ISystemInitializer
    {
        [SerializeField]
        private bool isReady = true;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Rect gameZone;

        [Header("Swing Options")]
        [SerializeField]
        private Vector3 Swing = new Vector3(0,0, 22f);

        private Vector3 startPosition;
        private bool isLaunchMode = false;

        private PlayerInput playerInput;
        private InputAction leftClickAction;
        private InputAction rightClickAction;
        private InputAction moveAction;
        [Header("Interaction Actions")]
        [Tooltip("Specify actions for when paddle interaction is called.")]
        public UnityEvent OnInteraction = new UnityEvent();
        private Vector3 movementDirection;
        private float speed = 4f;
        private float leftZone;
        private float rightZone;

        /// <inheritdoc/>
        public void Interact()
        {
            OnInteraction?.Invoke();
        }

        public void Initialize()
        {
            isReady = true;
            playerInput = GetComponent<PlayerInput>();
            PaddleMode();
        }

        private void PaddleMode()
        {
            playerInput.SwitchCurrentActionMap("Paddle");
            leftClickAction = playerInput.actions["Paddle/Swing Left"];
            rightClickAction = playerInput.actions["Paddle/Swing Right"];
            moveAction = playerInput.actions["Paddle/Move"];
            moveAction.Enable();
            moveAction.performed += Move;
            moveAction.canceled += OnMoveCanceled;
            leftClickAction.performed += context => OnLeftClick();
            rightClickAction.performed += context => OnRightClick();

            leftClickAction.performed -= context => OnLaunchBall();
            rightClickAction.performed -= context => OnLaunchBall();
        }

        private void LaunchMode()
        {
            playerInput.SwitchCurrentActionMap("Launch");
            leftClickAction = playerInput.actions["Launch/LaunchBall"];
            rightClickAction = playerInput.actions["Launch/LaunchBall"];

            leftClickAction.performed -= context => OnLeftClick();
            rightClickAction.performed -= context => OnRightClick();

            leftClickAction.performed += context => OnLaunchBall();
            rightClickAction.performed += context => OnLaunchBall();

            Debug.LogWarning("Switched to Launch Mode.");
        }

        private void Move(InputAction.CallbackContext context)
        {
            var moveInput = context.ReadValue<Vector2>();
            movementDirection = new Vector3(moveInput.x * speed * Time.deltaTime, 0f, 0f);
        }

        private void OnMoveCanceled(InputAction.CallbackContext obj)
        {
            movementDirection = Vector3.zero;
        }

        private void OnRightClick()
        {
            SwingPaddle(Swing);
        }

        private void OnLeftClick()
        {
            SwingPaddle(Swing * -1);
        }
        
        private void OnLaunchBall()
        {
            EventManager.Instance.QueueEvent(new LaunchBallEvent());
            isReady = true;
            isLaunchMode = false;
            PaddleMode();
        }
        private void OnReset(ResetGameEvent e)
        {
            isReady = false;
            transform.position = startPosition;
            isLaunchMode = true;
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

        private void OnEnable()
        {
            mainCamera = Camera.main;
            startPosition = transform.position;

            isReady = false;
            EventManager.Instance.AddListener<PauseGameEvent>(OnPauseGame);
            EventManager.Instance.AddListener<ResetGameEvent>(OnReset);
        }
        private void OnDisable()
        {
            moveAction.performed -= Move;
            moveAction.canceled -= OnMoveCanceled;
            leftClickAction.performed -= context => OnLeftClick();
            rightClickAction.performed -= context => OnRightClick();
            leftClickAction.performed -= context => OnLaunchBall();
            rightClickAction.performed -= context => OnLaunchBall();

            if (EventManager.Instance != null)
            {
                EventManager.Instance.RemoveListener<PauseGameEvent>(OnPauseGame);
                EventManager.Instance.RemoveListener<ResetGameEvent>(OnReset); 
            }

            moveAction.Disable();
            leftClickAction.Disable();
            rightClickAction.Disable();
        }

        private void Update()
        {
            if (isReady)
            {
                MovePaddle();
            }

            if (isLaunchMode)
            {
                LaunchMode();
            }
        }

        private void MoveByMouse()
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            if (gameZone.Contains(mousePosition))
            {
                transform.position = new Vector3(mousePosition.x, transform.position.y, transform.position.z);
            }
        }

        /// <summary>
        /// Translates the paddle based on value in <see cref="movementDirection"/>.
        /// </summary>
        /// <remarks>Direction is calculated by an input function, like <see cref="Move(InputAction.CallbackContext)"/>.
        /// Specifically clamps paddle movement to <see cref="gameZone"/> bounds.</remarks>
        private void MovePaddle()
        {
            // Update the position, but clamped to gameZone.
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x + movementDirection.x, gameZone.xMin, gameZone.xMax),
                transform.position.y,
                transform.position.z);
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