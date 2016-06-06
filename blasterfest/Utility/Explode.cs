using UnityEngine;
using System.Collections.Generic;
using MEC;

public class Explode : MonoBehaviour, IDestructable {

	[SerializeField]
	private AExplosion _explosion;

	public void Destroy (Collider2D playerCol, SpinePlayerController player, bool wasRevengeBullet)
	{
		Timing.RunCoroutine(Explosion(GetComponent<Collider2D>(), playerCol, player, wasRevengeBullet));
		gameObject.SetActive(false);
		Timing.RunCoroutine (Respawn ());
	}

	private IEnumerator<float> Explosion (Collider2D ignoreCol, Collider2D playerCol, SpinePlayerController player, bool wasRevengeBullet)
	{
		yield return Timing.WaitForSeconds(0.1f);
		AExplosion explosion = Instantiate(_explosion, transform.position, Quaternion.identity) as TileExplosion;
		explosion.Explode(ignoreCol, player, playerCol, wasRevengeBullet);
	}

	private IEnumerator<float> Respawn ()
	{
		yield return Timing.WaitForSeconds (10);
		gameObject.SetActive (true);
	}
}
