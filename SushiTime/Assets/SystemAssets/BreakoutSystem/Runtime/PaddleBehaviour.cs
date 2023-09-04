namespace BreakoutSystem
{
    using DG.Tweening;
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

        [Header("Swing Options")]
        [SerializeField]
        private Vector3 Swing = new Vector3(0,0, 22f);

        private Vector3 mousePosition;
        private Collider2D thisCollider;
        private Collider2D ThisCollider
        {
            get
            {
                if (!thisCollider)
                {
                    thisCollider =  GetComponent<Collider2D>();
                }
                return thisCollider;
            }
        }

        public Vector2 direction { get; private set; }
        public Rigidbody2D rigidBody { get; private set; }
        private void Start()
        {
            rigidBody = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (playZone != null && playZone.OverlapPoint(mousePosition))
            {
                if (mouseEnabled)
                {
                    MoveByMouse();
                }
                else
                {
                     Vector2 direction = Vector2.zero;

                     if (Input.GetKeyDown(KeyCode.A) | Input.GetKeyDown(KeyCode.LeftArrow))
                     {
                         direction = Vector2.left;
                     }
                     else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                     {
                         direction = Vector2.right;
                     }
                     else
                     {
                         direction = Vector2.zero;
                     }

                     rigidBody.AddForceAtPosition(direction * moveSpeed, transform.position);
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                SwingPaddle(Swing * -1);
            }

            if (Input.GetMouseButtonDown(1))
            {
                SwingPaddle(Swing);
            }
        }

        private void MoveByMouse()
        {
            gameObject.transform.position = new Vector3(mousePosition.x, transform.position.y);
        }

        private void SwingPaddle(Vector3 vector3)
        {
            var time = .05f;
            var swing = transform.DORotate(vector3, time, RotateMode.Fast);
            var swingDone = swing.OnComplete(() => transform.DORotate(Vector3.zero, .24f));
            Invoke(nameof(swingDone), time+.4f);
        }
    } 
}