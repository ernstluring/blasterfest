using UnityEngine;
using System.Collections;

public class TileExplosion : AExplosion {

	public override void Explode (Collider2D colToIgnore, SpinePlayerController owner, Collider2D ownerCol, bool wasRevengeBullet)
	{
		_audioSource.PlayOneShot (_explosionSound);
		Collider2D[] splash = Physics2D.OverlapCircleAll(transform.position, _radius, base.layerMask);
		for (int i = 0; i < splash.Length; i++) {
			if (splash[i] != colToIgnore && splash[i] != ownerCol)
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
		Destroy(gameObject, _anim.GetCurrentAnimatorStateInfo(0).length);
	}
}
