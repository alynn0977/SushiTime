namespace BreakoutSystem
{
    using Core;
    using UnityEngine;

    /// <summary>
    /// Core behaviour of the ball.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class BallBehaviour : MonoBehaviour
    {
        private const string boundaryTag = "Boundary";
        [SerializeField] Rigidbody2D _rb;
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

        private void Push(Vector2 velocity)
        {
            newVelocity = velocity;
        }
        private void LaunchBall()
        {
            newVelocity = Vector2.up * 4f;
        }
        private void Start()
        {
            if (_rb!= null)
            {
               _rb = GetComponent<Rigidbody2D>();
            }

            // Set the initial direction
            LaunchBall();
        }

        private void FixedUpdate()
        {
            _rb.MovePosition(_rb.position + newVelocity * Time.deltaTime);
        }

    }
}