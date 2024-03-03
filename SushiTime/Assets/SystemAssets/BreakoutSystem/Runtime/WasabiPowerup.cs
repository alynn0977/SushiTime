namespace BreakoutSystem
{
    using UnityEngine;

    public class WasabiPowerup : PowerupBehaviour, IPowerUp
    {
        [SerializeField]
        private string powerName;
        [SerializeField]
        private GameObject prefab;
        [SerializeField]
        private float time;

        public string PowerName => powerName;

        public GameObject PowerPrefab => prefab;

        public float Time => time;

        protected override void PowerUp()
        {
            Debug.Log($"{GetType().Name} powerup activated!");
            EventManager.Instance.QueueEvent(new IncreasePowerEvent(2));
            EventManager.Instance.QueueEvent(new PowerUpEvent(this));
            Destroy(gameObject);
        }

        private void Awake()
        {
            if (powerName == null || prefab == null) 
            {
                Debug.LogWarning($"[{GetType().Name}] on {gameObject.name} is missing references.");
            }
        }
    }
}
