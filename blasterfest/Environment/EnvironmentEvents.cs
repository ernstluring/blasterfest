using UnityEngine;
using System.Collections.Generic;
using MEC;

public class EnvironmentEvents : MonoBehaviour {

	public enum EnvironmentType { Car }

	public delegate void OnEnvironmentCall (EnvironmentType environmentType);
	public static event OnEnvironmentCall onEnvironmentCall;

	public delegate void OnWanted (SpinePlayerController playerController);
	public static event OnWanted onWanted;

	public bool flyingCar = true;

	#if UNITY_EDITOR
	public bool debugFlyingCar = false;
	#endif

	[SerializeField]
	private int[] _waitTimes;

	private void Start ()
	{
		if (_waitTimes == null)
			Debug.LogError("The 'waitTimes' array is empty, there should atleast by one value", this);
		Timing.RunCoroutine(CarEvent(), Segment.Update);
	}


	private IEnumerator<float> CarEvent ()
	{
		while (flyingCar)
		{
			int waitValue = _waitTimes[Random.Range(0, _waitTimes.Length)];

			#if UNITY_EDITOR
			if (debugFlyingCar) {
				waitValue = 10;
			}
			#endif

			yield return Timing.WaitForSeconds(waitValue);
			if (onEnvironmentCall != null)
				onEnvironmentCall(EnvironmentType.Car);
		}
	}

	public static void OnWantedEvent (SpinePlayerController playerController)
	{
		if (onWanted != null)
			onWanted (playerController);
	}
}
