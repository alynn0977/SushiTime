namespace BreakoutSystem
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Behaviour for paddle interaction.
    /// </summary>
    public class PaddleBehaviour : MonoBehaviour
    {
        [Header("Input Options")]
        [SerializeField]
        private bool mouseEnabled = false;

        [Header("Paddle Options")]
        [SerializeField]
        private float moveSpeed = 1.2f;
        [SerializeField]
        private float deadzone = .45f;
        [SerializeField]
        private BoxCollider2D playZone;

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
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (playZone && playZone.OverlapPoint(mousePosition))
            {
                Debug.LogWarning("Mouse in play!");
                if (mouseEnabled)
                {
                    MoveByMouse();
                }
                else
                {
                    // Vector2 direction = Vector2.zero;

                    // if (Input.GetKeyDown(KeyCode.A) | Input.GetKeyDown(KeyCode.LeftArrow))
                    // {
                    //     direction = Vector2.left;
                    // }
                    // else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                    // {
                    //     direction = Vector2.right;
                    // }
                    // else
                    // {
                    //     direction = Vector2.zero;
                    // }

                    // rigidBody.AddForceAtPosition(direction * moveSpeed, transform.position);
                }
            }
        }

        private void MoveByMouse()
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            gameObject.transform.position = new Vector3(mousePosition.x, transform.position.y);
        }
    } 
}