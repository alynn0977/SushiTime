namespace BreakoutSystem
{
    using Core;
    using System;
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
        private Vector2 newVector;
        private Vector3 startingPosition;
        private SpriteRenderer ballSprite;

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
                newVector += Vector2.up * upwardBias;
            }
            
            Push(newVector);
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
            newVector = Vector2.up * 4f;
        }

        /// <summary>
        /// Reset the ball back to it's position
        /// </summary>
        public void ResetBall()
        {
            EventManager.Instance.QueueEvent(new ResetGameEvent());
            ballSprite.enabled = false;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            transform.position = startingPosition;
            isPlaymode = false;
            SpriteOn();
        }
        private void CalculateNewVector(Collision2D collider)
        {
            // Provide new velocity, based on what was hit.
            newVector = Vector2.Reflect(newVector, collider.contacts[0].normal);
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
            newVector = velocity;
        }

        private void SpriteOn()
        {
            ballSprite.enabled = true;
            EventManager.Instance.AddListener<LaunchBallEvent>(LaunchBall);
        }

        private void OnEnable()
        {
            rb = GetComponent<Rigidbody2D>();
            startingPosition = gameObject.transform.position;
            ballSprite = GetComponent<SpriteRenderer>();

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
            }
        }

        private void FixedUpdate()
        {
            if (isPlaymode)
            {
                rb.MovePosition(rb.position + newVector * Time.deltaTime * ballSpeed);
            }
        }

    }
}