namespace AudioSystem
{
	using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Request a Volume Change Event. Use <see cref="VolumeType"/>
	/// to determine which key is changed.
    /// </summary>
    public class RequestVolumeChange : MonoBehaviour
	{
		[SerializeField]
		private VolumeType volumeType;

		[SerializeField]
		private float newVolume;

		public void OnChangeVolume(float value)
		{
			EventManager.Instance.QueueEvent(new ChangeVolumeEvent(value, volumeType));
		}

		public void OnChangeVolume(Slider sliderValue)
		{
			Debug.Log($"Changing {volumeType} to {sliderValue.value}.");
			EventManager.Instance.QueueEvent(new ChangeVolumeEvent(sliderValue.value, volumeType));
        }
	} 
}
