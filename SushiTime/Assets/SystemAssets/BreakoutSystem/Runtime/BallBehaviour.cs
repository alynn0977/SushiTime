namespace BreakoutSystem
{
    using Core;
    using System;
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// Core behaviour of the ball.
    /// The ball is velocity-driven: it keeps a normalized travel direction and
    /// re-asserts a constant speed every physics step, so it can never slow
    /// down, stall, or get dragged by friction.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class BallBehaviour : MonoBehaviour
    {
        // Minimum vertical share of the travel direction. Prevents the ball
        // from grinding back and forth in near-horizontal lines.
        private const float minVerticalComponent = 0.25f;

        [SerializeField]
        private Rigidbody2D rb;
        [SerializeField]
        private bool isPlayOnStart = false;
        [SerializeField]
        private bool isPlaymode = true;

        [Header("Speed")]
        [SerializeField]
        [Tooltip("Travel speed in units per second at launch.")]
        private float ballSpeed = 8f;
        [SerializeField]
        [Tooltip("Speed added on every paddle hit, for a gradual ramp-up.")]
        private float paddleHitSpeedGain = 0.25f;
        [SerializeField]
        [Tooltip("Speed ceiling after ramp-up and boosts.")]
        private float maxSpeed = 14f;
        [SerializeField]
        [Tooltip("Multiplier applied by BoostBall().")]
        private float boost = 2f;
        [SerializeField]
        [Range(0f, 1f)]
        [Tooltip("How strongly the hit position on the paddle steers the ball. 0 = plain reflection.")]
        private float paddleControl = 0.75f;

        [Header("Ball States")]
        [SerializeField]
        private SpriteRenderer ballSprite;
        [SerializeField]
        private BallStateProperties defaultState;
        [SerializeField]
        private BallStateProperties[] ballStatus;

        private Vector2 direction = Vector2.up;
        private float currentSpeed;
        private Vector3 startingPosition;

        public Vector2 CurrentDirection { get; private set; }

        public float CurrentAngle { get; private set; }

        private enum BallState
        {
            Default = 0,
            Fireball = 1,
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            // Activate the interaction of capable objects.
            if (collision.gameObject.TryGetComponent(out IInteractable interactor))
            {
                interactor.Interact();
            }

            if (collision.gameObject.TryGetComponent(out PaddleBehaviour _))
            {
                BounceOffPaddle(collision);
            }
            else
            {
                direction = Vector2.Reflect(direction, collision.GetContact(0).normal);
            }

            ClampDirection();
            ApplyVelocity();
        }

        /// <summary>
        /// Give the ball a temporary speed burst, capped at max speed.
        /// </summary>
        public void BoostBall()
        {
            currentSpeed = Mathf.Min(currentSpeed * boost, maxSpeed);
        }

        /// <summary>
        /// Launch the ball in an initialize direction.
        /// </summary>
        public void LaunchBall(LaunchBallEvent e)
        {
            isPlaymode = true;
            ballSprite.enabled = true;
            direction = Vector2.up;
            currentSpeed = ballSpeed;
            ApplyVelocity();
        }

        public void OnPowerUp(PowerUpEvent e)
        {
            switch (e.PowerUp)
            {
                case WasabiPowerup wasabiPowerup:
                    if (e.IsReset)
                    {
                        SwitchBallState(BallState.Default);
                    }
                    else
                    {
                        SwitchBallState(BallState.Fireball, e.PowerUp.Time);
                    }
                    break;
            }
        }

        /// <summary>
        /// Reset the ball back to it's position
        /// </summary>
        public void ResetBall()
        {
            EventManager.Instance.QueueEvent(new ResetGameEvent());
            SwitchBallState(BallState.Default);
            ballSprite.enabled = false;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            transform.position = startingPosition;
            direction = Vector2.up;
            currentSpeed = ballSpeed;
            isPlaymode = false;
            SpriteOn();
        }

        private void SwitchBallState(BallState newBallState, float time = -1)
        {
            if (ballStatus == null )
            {
                Debug.LogWarning("Ball behaviour cannot switch states without proper references. Check prefab.");
                return;
            }

            switch (newBallState)
            {
                case BallState.Fireball:
                    // First, disable current gameObject
                    defaultState.ballGameObject.SetActive(false);

                    // Pull the number out of the Ball State enum, and create a new state with it.
                    var newState = ballStatus[(int)BallState.Fireball];

                    // Parse new state.
                    NewBallState(newState);
                    // Reset after i seconds.
                    // StartCoroutine(CountdownAndResetStatus(time));
                    break;
                case BallState.Default:

                    Debug.Log($"[{GetType().Name}] Switching back to default.");
                    // Turn off all active children in the hiearchy.
                    ResetChildrenObjects();

                    // Bring back default state.
                    NewBallState(defaultState);
                    break;
                default:
                    SwitchBallState(BallState.Default);
                    break;
            }

            void NewBallState(BallStateProperties newState)
            {
                ballSprite.color = newState.spriteColor;
                newState.ballGameObject.SetActive(true);
            }
        }

        private void ResetChildrenObjects()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i).gameObject;
                if (child != null)
                    child.SetActive(false);
            }
        }

        private IEnumerator CountdownAndResetStatus(float countdownTime)
        {
            var convertTime = CoreUtilities.MinsToSec(countdownTime);
            Debug.Log($"[{GetType().Name}] Started new reset timer for {convertTime} seconds.");
            yield return new WaitForSeconds(convertTime);
            SwitchBallState(BallState.Default);
        }

        /// <summary>
        /// Bounce off the paddle, steered by where the ball landed on it:
        /// far left edge sends the ball sharply left, center sends it straight
        /// up, far right sharply right. Also ramps the speed up slightly.
        /// </summary>
        private void BounceOffPaddle(Collision2D collision)
        {
            Bounds paddleBounds = collision.collider.bounds;
            float hitOffset = Mathf.Clamp(
                (rb.position.x - paddleBounds.center.x) / paddleBounds.extents.x, -1f, 1f);

            Vector2 reflected = Vector2.Reflect(direction, collision.GetContact(0).normal);
            Vector2 steered = new Vector2(hitOffset, 1f).normalized;

            direction = Vector2.Lerp(reflected, steered, paddleControl).normalized;

            // Paddle hits always send the ball upward, even on edge/side
            // contacts, so it can never get wedged against the paddle.
            if (direction.y <= 0f)
            {
                direction = steered;
            }

            currentSpeed = Mathf.Min(currentSpeed + paddleHitSpeedGain, maxSpeed);
        }

        /// <summary>
        /// Normalizes the direction and enforces a minimum vertical angle so
        /// the ball can't grind along walls in a near-horizontal line.
        /// </summary>
        private void ClampDirection()
        {
            if (direction.sqrMagnitude < 0.0001f)
            {
                direction = Vector2.up;
                return;
            }

            direction.Normalize();

            if (Mathf.Abs(direction.y) < minVerticalComponent)
            {
                float ySign = direction.y >= 0f ? 1f : -1f;
                direction.y = ySign * minVerticalComponent;
                direction.x = Mathf.Sign(direction.x) * Mathf.Sqrt(1f - minVerticalComponent * minVerticalComponent);
            }
        }

        /// <summary>
        /// Pushes the current direction and speed into the rigidbody and
        /// refreshes <see cref="CurrentDirection"/> / <see cref="CurrentAngle"/>.
        /// </summary>
        private void ApplyVelocity()
        {
            rb.linearVelocity = direction * currentSpeed;
            CurrentDirection = direction;
            CurrentAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }

        private void SpriteOn()
        {
            ballSprite.enabled = true;
            EventManager.Instance.AddListener<LaunchBallEvent>(LaunchBall);
        }

        #region Unity Methods
        private void OnEnable()
        {
            rb = GetComponent<Rigidbody2D>();
            startingPosition = gameObject.transform.position;
            currentSpeed = ballSpeed;

            // Enforce the physics setup the ball depends on, so prefab
            // settings can't reintroduce gravity, drag, or tunneling.
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 0f;
            rb.linearDamping = 0f;
            rb.freezeRotation = true;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;

            if (TryGetComponent(out Collider2D ballCollider))
            {
                ballCollider.sharedMaterial = new PhysicsMaterial2D("Ball (Frictionless)")
                {
                    friction = 0f,
                    bounciness = 0f,
                };
            }

            if (ballSprite == null)
            {
                ballSprite = GetComponent<SpriteRenderer>();
            }

            // The ball must hold still until a LaunchBallEvent arrives
            // (unless explicitly flagged to play on start). The serialized
            // isPlaymode value can't be trusted for this.
            isPlaymode = isPlayOnStart;

            // Listen for the launch event from the start; AddListener is
            // duplicate-safe, so SpriteOn re-registering later is harmless.
            EventManager.Instance.AddListener<LaunchBallEvent>(LaunchBall);

            if (ballStatus == null)
            {
                Debug.Log($"Ball behaviour lacks status associations. Ball will not switch states.");
            }
            else
            {
                EventManager.Instance.AddListener<PowerUpEvent>(OnPowerUp);
            }

            if (isPlayOnStart)
            {
                // Set the initial direction
                LaunchBall(null);
            }
        }
        private void OnDestroy()
        {
            if (EventManager.Instance != null)
            {
                EventManager.Instance.RemoveListener<LaunchBallEvent>(LaunchBall);
                EventManager.Instance.RemoveListener<PowerUpEvent>(OnPowerUp);
            }
        }

        private void FixedUpdate()
        {
            if (isPlaymode)
            {
                // Re-assert the velocity every physics step. Collisions,
                // corner catches, or contact friction can never bleed speed.
                ApplyVelocity();
            }
            else
            {
                // Hold position while waiting for launch, so paddle bumps
                // or other contacts can't nudge the idle ball around.
                rb.linearVelocity = Vector2.zero;
            }
        }
        #endregion
    }

    [Serializable]
    public class BallStateProperties
    {
        public GameObject ballGameObject;
        public Color spriteColor;
    }
}
