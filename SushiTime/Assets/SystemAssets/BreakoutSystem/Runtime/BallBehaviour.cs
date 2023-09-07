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
        [SerializeField]
        private float speed = 20f;
        [SerializeField]
        private float initialSpeed = 2f;
        
        private Vector3 initialDirection;

        private GameZone gameZone;
        
        [field: SerializeField]
        public Vector2 Velocity { get; set; }

        [field: SerializeField]
        public int CurrentPlayerPwr { get; private set; }

        public Rigidbody2D ThisRigidBody { get; private set; }

        private void Awake()
        {
            ThisRigidBody = GetComponent<Rigidbody2D>();
            ThisRigidBody.velocity = ReturnRandomTraj().normalized * initialSpeed;
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

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Ball activates certain game objects.
            if (collision.gameObject.TryGetComponent(out iInteractable interactable))
            {
                Interact(collision);
            }
        }

        private static void Interact(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out iInteractable interactable))
            {
                interactable.Interact();
            }
        }

        private Vector3 ReturnRandomTraj()
        {
            Vector3 force = Vector2.zero;
            force.x = Random.Range(-1f, 1f);
            force.y = -1f;

            return force;
        }

        private void FixedUpdate()
        {
            if (Mathf.Abs(ThisRigidBody.velocity.y) < 0.5f)
            {
                Vector3 newVelocity = ThisRigidBody.velocity;
                newVelocity.y = newVelocity.y < 0 ? -1f : 1f;
                ThisRigidBody.velocity = newVelocity;
            }

            // Normalize the velocity to maintain a constant speed
            ThisRigidBody.velocity = ThisRigidBody.velocity.normalized * speed;
        }

        private void PlayerPowerIncrease(IncreasePowerEvent e)
        {
            Debug.Log($"Player power increased to {e.SetPower}");
            CurrentPlayerPwr = e.SetPower;
        }
    }

}