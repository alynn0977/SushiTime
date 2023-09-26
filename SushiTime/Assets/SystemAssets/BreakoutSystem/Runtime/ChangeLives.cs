namespace BreakoutSystem
{
    using Core;
    using UnityEngine;

	public class ChangeLives : MonoBehaviour
	{
		/// <summary>
		/// Tick lives down by 1.
		/// </summary>
		public void TickLives()
		{
            EventManager.Instance.QueueEvent(new ChangeLivesEvent(-1));
        }

		/// <summary>
		/// Custom update lives.
		/// </summary>
		/// <param name="i">Add or subtract lives by this amount.</param>
		public void UpdateLives(int i)
		{
            EventManager.Instance.QueueEvent(new ChangeLivesEvent(i));
        }
	}

}