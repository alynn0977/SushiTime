namespace BreakoutSystem
{
    using Core;
    using ScreenSystem;
    using System;
    using UnityEngine;

    /// <summary>
    /// Core behaviour of the ball.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class BallBehaviour : MonoBehaviour
    {
        private const string boundaryTag = "Boundary";
        
        [SerializeField]
        private Rigidbody2D _rb;
        [SerializeField]
        private bool isPlayOnStart = false;
        [SerializeField]
        private bool isPlaymode = true;
        private Vector2 newVelocity;

        public void OnCollisionEnter2D(Collision2D collider)
        {
            // Activate the interaction of capable objects.
            if (collider.gameObject.TryGetComponent(out iInteractable interactor))
            {
                interactor.Interact();
            }

            // Provide new velocity, based on what was hit.
            newVelocity = Vector2.Reflect(newVelocity, collider.contacts[0].normal);

            // If the ball hits the paddle or wall, add a slight upward bias to the velocity
            if (collider.gameObject.TryGetComponent<PaddleBehaviour>(out _) ||
                collider.gameObject.CompareTag(boundaryTag))
            {
                newVelocity += Vector2.up * 0.1f;
            }

            Push(newVelocity);
        }
        
        /// <summary>
        /// Launch the ball in an initialize direction.
        /// </summary>
        public void LaunchBall()
        {
            newVelocity = Vector2.up * 4f;
        }

        private void Push(Vector2 velocity)
        {
            newVelocity = velocity;
        }
        private void Start()
        {
            if (_rb!= null)
            {
               _rb = GetComponent<Rigidbody2D>();
            }

            EventManager.Instance.AddListenerOnce<KillPlayerEvent>(OnKillPlayer);

            if (isPlayOnStart)
            {
                // Set the initial direction
                LaunchBall(); 
            }
        }

        private void OnKillPlayer(KillPlayerEvent e)
        {
            gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            if (isPlaymode)
            {
                _rb.MovePosition(_rb.position + newVelocity * Time.deltaTime); 
            }
        }

    }
}