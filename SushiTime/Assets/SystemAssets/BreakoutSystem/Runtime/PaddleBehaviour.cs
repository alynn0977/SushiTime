namespace BreakoutSystem
{
    using DG.Tweening;
    using UnityEngine;

    /// <summary>
    /// Behaviour for paddle interaction.
    /// </summary>
    public class PaddleBehaviour : MonoBehaviour
    { 
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Rect gameZone;

        [Header("Swing Options")]
        [SerializeField]
        private Vector3 Swing = new Vector3(0,0, 22f);


        private void Start()
        {
            mainCamera = Camera.main;
        }

        void Update()
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            if (gameZone.Contains(mousePosition))
            {
                transform.position = new Vector3(mousePosition.x, transform.position.y, transform.position.z);

                if (Input.GetMouseButtonDown(0))
                {
                    SwingPaddle(Swing * -1);
                }

                if (Input.GetMouseButtonDown(1))
                {
                    SwingPaddle(Swing);
                }
            }
        }

        private void SwingPaddle(Vector3 vector3)
        {
            var time = .05f;
            var swing = transform.DORotate(vector3, time, RotateMode.Fast);
            var swingDone = swing.OnComplete(() => transform.DORotate(Vector3.zero, .24f));
            Invoke(nameof(swingDone), time+.4f);
        }

        /// <summary>
        /// Use this gizmo to define the playable
        /// area where the paddle must follow the 
        /// </summary>
        [ExecuteInEditMode]

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(gameZone.center, new Vector3(gameZone.width, gameZone.height, 0));
        }
    } 
}