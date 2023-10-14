namespace BreakoutSystem
{
    using Core;
    using PrimeTween;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Brick behaviour.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class BrickBehaviour : MonoBehaviour, IInteractable
    {
        private const float nudgeDistance = 0.1f;
        [TabGroup("Tile Properties")]
        [SerializeField]
        private string tileName;
        [TabGroup("Tile Properties")]
        [SerializeField]
        [Tooltip("How many hits can this break take?")]
        private int health = 1;
        [TabGroup("Tile Properties")]
        [SerializeField]
        [Tooltip("How many points is this brick worth?")]
        private int score = 10;
        [TabGroup("Tile Properties")]
        [Tooltip("What's the icon for this?")]
        [SerializeField]
        private SpriteRenderer icon;
        [TabGroup("Animation Properties")]
        [SerializeField]
        private Ease easeType;
        [TabGroup("Animation Properties")]
        [SerializeField]
        private float animateSpeed = .07f;
        [TabGroup("Animation Properties")]
        [SerializeField]
        private float reverseSpeed = .02f;

        private Vector3 collisionDirection;
        private GameZone gameZone;
        private Vector3 cachePosition;

        [Tooltip("Events for Brick Interaction.")]
        public UnityEvent OnBrickInteraction = new UnityEvent();

        [Tooltip("Events for when Brick is Destroyed")]
        public UnityEvent<GameObject> OnBrickDestroy = new UnityEvent<GameObject>();

        /// <summary>
        /// Read-only access to brick behaviour sprite.
        /// </summary>
        public SpriteRenderer TileSprite => icon;

        /// <summary>
        /// Read-only access of tile name.
        /// </summary>
        public string TileName => tileName;
        private void Awake()
        {
            if (GetComponentInParent<GameZone>())
            {
                gameZone = GetComponentInParent<GameZone>();
            }
            cachePosition = transform.localPosition;
        }

        /// <summary>
        /// Brick interactions 
        /// </summary>
        public void Interact()
        {
            // Include what must ALWAYS happen.
            // Note: Health check also checks if brick should animate.
            HealthCheck();
            OnBrickInteraction?.Invoke();
        }

        /// <summary>
        /// Nudges the brick in the direction the
        /// ball hits it.
        /// </summary>
        public void Animate()
        {
            Tween.LocalPosition(
                transform,
                CalculateNudgeDirection(),
                animateSpeed,
                easeType).OnComplete(ReversePosition, warnIfTargetDestroyed: false);
        }

        public void ReversePosition()
        {
           Tween.LocalPosition(
                transform,
                cachePosition,
                reverseSpeed,
                easeType) ;
        }
        private Vector3 CalculateNudgeDirection()
        {
            return cachePosition + (collisionDirection * nudgeDistance);
        }

        private void HealthCheck()
        {
            if (health >= 100)
            {
                // This brick is unbreakable.
                return;
            }

            if (gameZone)
            {
                health = health - gameZone.PlayerPower;
            }
            else
            {
                health--;
            }
            
            if (health <= 0)
            {
                EventManager.Instance.QueueEvent(new ChangeScoreEvent(score));
                OnBrickDestroy?.Invoke(gameObject);
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D collidingObject)
        {
            // If this is a ball behaviour...
            if (collidingObject.gameObject.TryGetComponent(out BallBehaviour ball))
            {
                // Get it's relative velocity.
                collisionDirection = collidingObject.relativeVelocity.normalized;
            }
        }
    }

}