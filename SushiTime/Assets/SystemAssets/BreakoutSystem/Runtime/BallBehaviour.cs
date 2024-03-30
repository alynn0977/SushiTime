namespace BreakoutSystem
{
    using Core;
    using System;
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// Core behaviour of the ball.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Sprite))]
    public class BallBehaviour : MonoBehaviour
    {
        private const string boundaryTag = "Boundary";
        private const float upwardBias = 0.1f;
        private const float minVelocity = 4.7f;

        [SerializeField]
        private Rigidbody2D rb;
        [SerializeField]
        private bool isPlayOnStart = false;
        [SerializeField]
        private bool isPlaymode = true;
        [SerializeField]
        private float ballSpeed = 2f;
        [SerializeField]
        private float boost = 2f;
        [Header("Ball States")]
        [SerializeField]
        private SpriteRenderer ballSprite;
        [SerializeField]
        private BallStateProperties defaultState;
        [SerializeField]
        private BallStateProperties[] ballStatus;

        private Vector2 ballVector;

        public Vector2 CurrentDirection { get; private set; }

        private Vector2 cachePosition = Vector2.zero;
        private Vector3 startingPosition;

        public float CurrentAngle { get; private set; }

        private enum BallState
        {
            Default = 0,
            Fireball = 1,
        }

        public void OnCollisionEnter2D(Collision2D collider)
        {
            // Activate the interaction of capable objects.
            if (collider.gameObject.TryGetComponent(out IInteractable interactor))
            {
                interactor.Interact();
            }

            CalculateNewVector(collider);

            // If the ball hits the paddle or wall, add a slight upward bias to the velocity
            if (collider.gameObject.TryGetComponent<PaddleBehaviour>(out _) ||
                collider.gameObject.CompareTag(boundaryTag))
            {
                ballVector += Vector2.up * upwardBias;
            }
            
            Push(ballVector);
        }

        // Method to boost the ball's velocity if it slows down too much.
        public void BoostBall()
        {
            Vector3 boostDirection = (4.8f - rb.velocity.magnitude) * rb.velocity.normalized;
            rb.AddForce(boostDirection * boost, ForceMode2D.Force);
        }

        /// <summary>
        /// Launch the ball in an initialize direction.
        /// </summary>
        public void LaunchBall(LaunchBallEvent e)
        {
            isPlaymode = true;
            ballSprite.enabled = true;
            ballVector = Vector2.up * 4f;
        }

        /// <summary>
        /// Reset the ball back to it's position
        /// </summary>
        public void ResetBall()
        {
            EventManager.Instance.QueueEvent(new ResetGameEvent());
            SwitchBallState(BallState.Default);
            ballSprite.enabled = false;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            transform.position = startingPosition;
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
                    StartCoroutine(CountdownAndResetStatus(time));
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
        private void CalculateNewVector(Collision2D collider)
        {
            // Provide new velocity, based on what was hit.
            ballVector = Vector2.Reflect(ballVector, collider.contacts[0].normal);

            if (collider.relativeVelocity.magnitude < minVelocity)
            {
                BoostBall();
            }
        }

        /// <summary>
        /// Push the ball using parameter.
        /// </summary>
        /// <param name="velocity">The velocity to push towards.</param>
        private void Push(Vector2 velocity)
        {
            ballVector = velocity;
        }

        private void SpriteOn()
        {
            ballSprite.enabled = true;
            EventManager.Instance.AddListener<LaunchBallEvent>(LaunchBall);
        }

        private void OnPowerUp(PowerUpEvent e)
        {
            switch (e.PowerUp)
            {
                case WasabiPowerup wasabiPowerup:
                    SwitchBallState(BallState.Fireball, e.PowerUp.Time);
                    break;
            }
        }

        #region Unity Methods
        private void OnEnable()
        {
            rb = GetComponent<Rigidbody2D>();
            startingPosition = gameObject.transform.position;
            
            if (ballSprite == null)
            {
                ballSprite = GetComponent<SpriteRenderer>();
            }

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
                if (rb.position != cachePosition)
                {
   
                    Vector2 displacement = rb.position - cachePosition;
                    CurrentDirection = displacement.normalized;

                    cachePosition = rb.position;

                    float angleRadians = Mathf.Atan2(CurrentDirection.y, CurrentDirection.x);
                    CurrentAngle = angleRadians * Mathf.Rad2Deg;
                }

                rb.MovePosition(rb.position + ballVector * Time.deltaTime * ballSpeed);
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