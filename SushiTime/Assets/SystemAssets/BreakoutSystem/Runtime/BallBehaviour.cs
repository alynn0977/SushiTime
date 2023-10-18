namespace BreakoutSystem
{
    using Core;
    using UnityEngine;

    /// <summary>
    /// Core behaviour of the ball.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Sprite))]
    public class BallBehaviour : MonoBehaviour
    {
        private const string boundaryTag = "Boundary";
        
        [SerializeField]
        private Rigidbody2D rb;
        [SerializeField]
        private bool isPlayOnStart = false;
        [SerializeField]
        private bool isPlaymode = true;
        [SerializeField]
        private float delayRelaunch = 3f;
        [SerializeField]
        private float ballSpeed = 2f;
        private Vector2 newVelocity;
        private Vector3 startingPosition;
        private SpriteRenderer ballSprite;
        public void OnCollisionEnter2D(Collision2D collider)
        {
            // Activate the interaction of capable objects.
            if (collider.gameObject.TryGetComponent(out IInteractable interactor))
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
            isPlaymode = true;
            ballSprite.enabled = true;
            newVelocity = Vector2.up * 4f;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetBall()
        {
            EventManager.Instance.QueueEvent(new ResetGameEvent());
            ballSprite.enabled = false;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            transform.position = startingPosition;
            isPlaymode = false;
            Invoke(nameof(SpriteOn), delayRelaunch-1);
        }

        private void Push(Vector2 velocity)
        {
            newVelocity = velocity;
        }

        private void SpriteOn()
        {
            ballSprite.enabled = true;
            Invoke(nameof(LaunchBall), delayRelaunch);
        }

        private void Start()
        {
            if (rb!= null)
            {
               rb = GetComponent<Rigidbody2D>();
            }

            EventManager.Instance.AddListenerOnce<KillPlayerEvent>(OnKillPlayer);
            startingPosition = gameObject.transform.position;
            ballSprite = GetComponent<SpriteRenderer>();

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
                rb.MovePosition(rb.position + newVelocity * Time.deltaTime * ballSpeed); 
            }
        }

    }
}