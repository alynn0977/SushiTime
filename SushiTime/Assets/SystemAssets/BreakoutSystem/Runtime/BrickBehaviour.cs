namespace BreakoutSystem
{
    using Core;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    /// <summary>
    /// Brick behaviour.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class BrickBehaviour : MonoBehaviour, iInteractable
    {
        [SerializeField]
        private string tileName;
        [SerializeField]
        [Tooltip("How many hits can this break take?")]
        private int health = 1;
        [SerializeField]
        [Tooltip("How many points is this brick worth?")]
        private int score = 10;
        [Tooltip("What's the icon for this?")]
        [SerializeField]
        private SpriteRenderer icon;
        private GameZone gameZone;
        
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
        }

        /// <summary>
        /// Brick interactions 
        /// </summary>
        public void Interact()
        {
            // Include what must ALWAYS happen.
            HealthCheck();

            OnBrickInteraction?.Invoke();

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
    }

}