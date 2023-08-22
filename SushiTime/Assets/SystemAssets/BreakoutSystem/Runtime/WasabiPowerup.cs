namespace BreakoutSystem
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class WasabiPowerup : PowerupBehaviour
    {
        protected override void PowerUp()
        {
            Debug.Log($"{GetType().Name} powerup activated!");
            EventManager.Instance.QueueEvent(new IncreasePowerEvent(2));
            Destroy(gameObject);
        }
    }
}
