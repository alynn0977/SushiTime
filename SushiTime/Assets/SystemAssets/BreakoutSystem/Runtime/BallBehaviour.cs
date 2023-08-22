namespace BreakoutSystem
{
    using Core;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Behaviour for the break-out ball.
    /// </summary>
    public class BallBehaviour : MonoBehaviour
    {

        private const int boundaryLimit = 3;

        [SerializeField]
        private float speed = 20f;

        [SerializeField]
        private float maxBounceAngle = 75f;

        private GameZone gameZone;
      
        [field: SerializeField]
        public float Speed { get; set; }
        
        [field: SerializeField]
        public Vector2 Vector { get; set; }

        [field: SerializeField]
        public int CurrentPlayerPwr { get; private set; }

        [field: SerializeField]
        public int CurrentBoundaryHits { get; private set; }

        public Rigidbody2D ThisRigidBody { get; private set; }
        


        private void Awake()
        {
            ThisRigidBody = GetComponent<Rigidbody2D>();

            if (GetComponentInParent<GameZone>())
            {
                gameZone = GetComponentInParent<GameZone>();
                CurrentPlayerPwr = gameZone.PlayerPower;
                
                if (EventManager.Instance)
                {
                    EventManager.Instance.AddListener<IncreasePowerEvent>(PlayerPowerIncrease);
                }
            }
            else
            {
                Debug.LogWarning("[BallBehaviour]: No gamezone found. Initializing with default.");
                CurrentPlayerPwr = 1;
            }

        }

        private void Start()
        {
            Invoke(nameof(ReturnRandomTraj), .4f);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out BallBehaviour ballBehaviour))
            {
                BallCollision(collision);
            }
            else if (collision.gameObject.TryGetComponent(out iInteractable interactable))
            {
                Interact(collision);
            }
            else if (collision.gameObject.tag == "Boundary")
            {
                if (CurrentBoundaryHits >= boundaryLimit)
                {
                    ReturnRandomTraj();
                    CurrentBoundaryHits = 0;
                    return;
                }

                CurrentBoundaryHits++;
            }
        }

        private static void Interact(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out iInteractable interactable))
            {
                interactable.Interact();
            }
        }

        private void BallCollision(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out BallBehaviour ball))
            {
                float calculateNewAngle = CalculateRotation(ball, collision);
                Quaternion rotation = Quaternion.AngleAxis(calculateNewAngle, Vector3.forward);
                ball.ThisRigidBody.velocity = rotation * Vector2.up * ball.ThisRigidBody.velocity.magnitude;
            }
        }

        private float CalculateRotation(BallBehaviour ball, Collision2D collision)
        {
            Vector3 paddlePosition = transform.position;
            Vector2 contactPoint = collision.GetContact(0).point;

            float offset = paddlePosition.x - contactPoint.x;
            float width = collision.otherCollider.bounds.size.x * 5f;

            float currentAngle = Vector2.SignedAngle(Vector2.up, ball.ThisRigidBody.velocity);
            float bounceAngle = (offset / width) * maxBounceAngle;

            float newAngle = Mathf.Clamp(currentAngle + bounceAngle, -maxBounceAngle, maxBounceAngle);

            return newAngle;
        }
        private void ReturnRandomTraj()
        {
            Vector2 force = Vector2.zero;
            force.x = Random.Range(-1f, 1f);
            force.y = -1f;

            ThisRigidBody.AddForce(force.normalized * speed);
        }

        private void Update()
        {
            Speed = ThisRigidBody.velocity.magnitude;
        }

        private void FixedUpdate()
        {
            ThisRigidBody.velocity = ThisRigidBody.velocity.normalized* speed;
        }

        private void PlayerPowerIncrease(IncreasePowerEvent e)
        {
            Debug.Log($"Player power increased to {e.SetPower}");
            CurrentPlayerPwr = e.SetPower;
        }
    }

}