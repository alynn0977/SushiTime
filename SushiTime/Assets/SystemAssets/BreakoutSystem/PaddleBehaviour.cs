namespace BreakoutSystem
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PaddleBehaviour : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed = 1.2f;
        [SerializeField]
        private float deadzone = .45f;
        private Vector3 mousePosition;
        private Collider2D thisCollider;

        public Vector2 direction { get; private set; }
        public Rigidbody2D rigidBody { get; private set; }
        private void Start()
        {
            thisCollider = GetComponent<Collider2D>();
            rigidBody = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            Vector3 direction = mousePosition - transform.position;

            if (Vector3.Distance(transform.position, mousePosition) > deadzone)
            {
                Debug.Log($"{Vector3.Distance(transform.position, mousePosition)}");
                rigidBody.AddForceAtPosition(direction * moveSpeed, transform.position);
            }
            else
            {
                Debug.Log("DEADZONE!");
                rigidBody.velocity = Vector3.zero;
            }

        }

    } 
}