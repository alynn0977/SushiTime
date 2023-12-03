namespace Core
{
    /// <summary>
    /// Provides a contract for initializing systems within the application.
    /// </summary>
    public interface ISystemInitializer
	{
        /// <summary>
        /// Initialize this system.
        /// <remarks>
        /// This method is resonsible for initialize a specific system
        /// at a given time. This is useful when enforcing an exact order of
        /// activating things, beyond just enable/disable.
        /// </remarks>
        /// </summary>
		public void Initialize();
	}

}