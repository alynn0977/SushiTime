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
        [SerializeField]
        private bool isTween = true;

        private Vector3 startPosition;

        private PlayerInput playerInput;
        private InputAction leftClickAction;
        private InputAction rightClickAction;
        private InputAction launchAction;
        private InputAction moveAction;
        [Header("Interaction Actions")]
        [Tooltip("Specify actions for when paddle interaction is called.")]
        public UnityEvent OnInteraction = new UnityEvent();
        private Vector3 movementDirection;
        private float speed = 4f;
        private float time = .05f;
        private Transform currentPaddle;
        private Rigidbody2D paddleBody;
        private float targetX;

        /// <inheritdoc/>
        public void Interact()
        {
            OnInteraction?.Invoke();
        }

        public void Initialize()
        {
            isReady = true;
            playerInput = FindObjectOfType<GameManager>().GetComponent<PlayerInput>();
            currentPaddle = transform;

            leftClickAction = playerInput.actions["Paddle/Swing Left"];
            rightClickAction = playerInput.actions["Paddle/Swing Right"];
            moveAction = playerInput.actions["Paddle/Move"];
            launchAction = playerInput.actions["Paddle/LaunchBall"];

            moveAction.performed += OnMoveDetected;
            moveAction.canceled += OnMoveCanceled;

            if (isTween)
            {
                leftClickAction.performed += context => OnLeftClick();
                rightClickAction.performed += context => OnRightClick();
            }
            else
            {
                leftClickAction.performed += context => StartRotating(Swing.z);
                leftClickAction.canceled += context => StopRotating();
            }
            
            launchAction.performed += context => OnLaunchBall();

            PaddleMode();
        }

        private void StartRotating(float angle)
        {
            
        }

        private void StopRotating()
        {

        }

        private void PaddleMode()
        {
            moveAction?.Enable();
            leftClickAction?.Enable();
            rightClickAction?.Enable();
            launchAction?.Disable();
        }

        private void LaunchMode()
        {
            moveAction?.Disable();
            leftClickAction?.Disable();
            rightClickAction?.Disable();
            launchAction?.Enable();
        }

        private void OnMoveDetected(InputAction.CallbackContext context)
        {
            var moveInput = context.ReadValue<Vector2>();

            InputDevice device = context.control.device;

#if UNITY_EDITOR
                DefaultMovementSpeed(moveInput);
#elif UNITY_WEBGL || !UNITY_EDITOR
            // WebGL specific code, to decrease mouse movement speed.
            if (device is Mouse)
            {
                movementDirection = new Vector3((moveInput.x * .1f * speed) * Time.deltaTime, 0f, 0f);
            }
            else if (device is Keyboard)
            {
                DefaultMovementSpeed(moveInput);
            }
#endif
            void DefaultMovementSpeed(Vector2 moveInput)
            {
                movementDirection = new Vector3(moveInput.x * speed * Time.deltaTime, 0f, 0f);
            }

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
            PaddleMode();
        }
        private void OnReset(ResetGameEvent e)
        {
            isReady = false;
            transform.position = startPosition;
            targetX = startPosition.x;
            if (paddleBody != null)
            {
                paddleBody.position = startPosition;
            }

            LaunchMode();
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

        private void OnSwingComplete()
        {
            Tween.LocalRotation(transform, Vector3.zero, .24f);
        }

        private void OnEnable()
        {
            mainCamera = Camera.main;
            startPosition = transform.position;
            targetX = transform.position.x;

            // The paddle needs a kinematic rigidbody moved through physics,
            // otherwise the engine sees a teleporting collider with zero
            // velocity and the ball reacts badly to fast paddle movement.
            if (!TryGetComponent(out paddleBody))
            {
                paddleBody = gameObject.AddComponent<Rigidbody2D>();
            }

            paddleBody.bodyType = RigidbodyType2D.Kinematic;
            paddleBody.useFullKinematicContacts = true;
            paddleBody.interpolation = RigidbodyInterpolation2D.Interpolate;

            isReady = false;
            EventManager.Instance.AddListener<PauseGameEvent>(OnPauseGame);
            EventManager.Instance.AddListener<ResetGameEvent>(OnReset);
        }

        private void OnDestroy()
        {
            mainCamera = null;
            startPosition = default;
            isReady = false;
            if (EventManager.Instance != null)
            {
                EventManager.Instance.RemoveListener<PauseGameEvent>(OnPauseGame);
                EventManager.Instance.RemoveListener<ResetGameEvent>(OnReset);
            }

            // Gates against users shutting down before it's initialized.
            if (playerInput != null)
            {
                moveAction?.Disable();
                leftClickAction?.Disable();
                rightClickAction?.Disable();
                launchAction?.Disable();

                leftClickAction.performed -= context => OnLeftClick();
                rightClickAction.performed -= context => OnRightClick();
                leftClickAction.performed -= context => OnLaunchBall();
                rightClickAction.performed -= context => OnLaunchBall();

                moveAction.performed -= OnMoveDetected;
                moveAction.canceled -= OnMoveCanceled;
                currentPaddle = null;
                moveAction = null;
                leftClickAction = null;
                rightClickAction = null;
                launchAction = null;

                playerInput = null; 
            }
        }

        private void Update()
        {
            if (isReady)
            {
                MovePaddle();
            }
        }

        // Legacy function.
        private void MoveByMouse()
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            if (gameZone.Contains(mousePosition))
            {
                transform.position = new Vector3(mousePosition.x, transform.position.y, transform.position.z);
                Debug.LogError($"Mouse is at {mousePosition}");
            }
        }

        /// <summary>
        /// Translates the paddle based on value in <see cref="movementDirection"/>.
        /// </summary>
        /// <remarks>Direction is calculated by an input function, like <see cref="OnMoveDetected(InputAction.CallbackContext)"/>.
        /// Specifically clamps paddle movement to <see cref="gameZone"/> bounds.</remarks>
        private void MovePaddle()
        {
            // Update the target position, clamped to gameZone. The rigidbody
            // is moved toward it in FixedUpdate so physics sees the paddle's
            // real velocity.
            targetX = Mathf.Clamp(targetX + movementDirection.x, gameZone.xMin, gameZone.xMax);
        }

        private void FixedUpdate()
        {
            paddleBody.MovePosition(new Vector2(targetX, transform.position.y));
        }

        private void SwingPaddle(Vector3 vector3)
        {
            if (currentPaddle == null)
            {
                Debug.LogError($"[{GetType().Name}]: Paddle object is missing.");

                return;
            }
            if (isTween)
            {
                Tween.LocalRotation(currentPaddle, endValue: vector3, duration: time).OnComplete(OnSwingComplete);
            }
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