using UnityEngine;
using System.Collections.Generic;
using MEC;

public class Explosion : AExplosion {

	public override void Explode (Collider2D colToIgnore, SpinePlayerController owner, Collider2D ownerCol,
		bool wasRevengeBullet)
	{
		_audioSource.PlayOneShot (_explosionSound);
		Collider2D[] splash = Physics2D.OverlapCircleAll(transform.position, base._radius, base.layerMask);
		for (int i = 0; i < splash.Length; i++) {
			if (splash[i] == ownerCol)
			{
				// Vector that points from the explosion position to the player's position
				Vector2 dir =  splash[i].transform.position - transform.position;
				owner.RocketJump(dir);
			}
			else if (splash[i] != colToIgnore)
			{
				IDestructable destructable = splash[i].GetComponent(typeof(IDestructable)) as IDestructable;
				IKillable k = splash[i].GetComponent(typeof(IKillable)) as IKillable;
				if (destructable != null)
					destructable.Destroy(ownerCol, owner, wasRevengeBullet);
				if (k != null && k.Kill(owner.IsWanted()))
				{
					// Wanted Game Mode Kill
					if (GameModeManager.Instance.GetGameMode () == GameMode.Wanted) {

						if (owner.IsWanted ()) {
							ScoreManager.Instance.CalculateScore(owner.Score, owner.IsWanted(), k.IsWanted(), false, wasRevengeBullet);
						}
						if (k.IsWanted ()) {
							ScoreManager.Instance.CalculateScore(owner.Score, owner.IsWanted(), k.IsWanted(), false, wasRevengeBullet);
							GameManager.Instance.WantedGame.DisableTimer (owner);
						}

						bool firstShot = !GameManager.Instance.WantedGame.FirstBulletIsShot;
						if (firstShot) {
							ScoreManager.Instance.CalculateScore(owner.Score, owner.IsWanted(), k.IsWanted(), false, wasRevengeBullet);
							GameManager.Instance.WantedGame.FirstBullet (owner);
						}
					}
				}
			}
		 }
		Timing.RunCoroutine(FadeAway());
	}

	private IEnumerator<float> FadeAway ()
	{
		yield return Timing.WaitForSeconds (_anim.GetCurrentAnimatorStateInfo (0).length);
		_explosionRenderer.enabled = false;
		Destroy (gameObject, 1.5f);
	}
}
