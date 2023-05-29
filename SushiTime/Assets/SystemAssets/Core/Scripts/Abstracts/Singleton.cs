namespace Core
{
    using UnityEngine;

    /// <summary>
    /// Singleton Abstract. Ensures all Singleton objects
    /// will behave ina  similar manner.
    /// </summary>
    public abstract class Singleton : MonoBehaviour
    {
        private static Singleton instance;

        protected Singleton()
        {
            if (instance != null)
            {
                Destroy(this);
            }
            else
            {
                instance = this;
            }
        }

        /// <summary>
        /// Instance of given singleotn object.
        /// </summary>
        public static Singleton Instance
        {
            get => instance;
        }

        /// <summary>
        /// Confirms if there is an instance.
        /// Useful for when objects try to access a
        /// destroyed instance.
        /// </summary>
        public virtual bool HasInstance
        {
            get
            {
                return instance != null;
            }
        }
    } 
}
