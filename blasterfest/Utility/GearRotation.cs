using UnityEngine;
using System.Collections.Generic;
using MEC;

public class GearRotation : MonoBehaviour {

	public Vector3 direction1, direction2;
	public float speed;

	private bool doDir1, doDir2;

	private void Update () {
		if (doDir1) {
			transform.Rotate (direction1 * speed * Time.deltaTime);
		} else if (doDir2) {
			transform.Rotate (direction2 * speed * Time.deltaTime);
		}
	}

	public void Rotate1 () {
		doDir1 = true;
		Timing.RunCoroutine (DoRotation(true));
	}

	public void Rotate2 () {
		doDir2 = true;
		Timing.RunCoroutine (DoRotation(false));
	}

	private IEnumerator<float> DoRotation (bool isDoDir1) {
		yield return Timing.WaitForSeconds (0.5f);
		if (isDoDir1)
			doDir1 = false;
		else
			doDir2 = false;
	}
}
