using UnityEngine;
using System.Collections;

public class BigBullet : BaseBullet {

	private void OnTriggerEnter2D (Collider2D col)
	{
		Vector2 hitPosition = transform.position;
		IKillable killable = col.GetComponent(typeof(IKillable)) as IKillable;
		if (killable != null)
		{
			Collider2D directHitCollider = col.GetComponent<Collider2D> ();
			CameraController.Instance.Shake();

			if (killable.Kill(_owner.IsWanted()))
			{
				// Wanted Game Mode Kill
				if (GameModeManager.Instance.GetGameMode () == GameMode.Wanted) {

					if (_owner.IsWanted ()) {
						ScoreManager.Instance.CalculateScore(_owner.Score, _owner.IsWanted(), killable.IsWanted(), true, false);
					}
					if (killable.IsWanted ()) {
						ScoreManager.Instance.CalculateScore(_owner.Score, _owner.IsWanted(), killable.IsWanted(), true, false);
						GameManager.Instance.WantedGame.DisableTimer (_owner);
					}

					bool firstShot = !GameManager.Instance.WantedGame.FirstBulletIsShot;
					if (firstShot) {
						ScoreManager.Instance.CalculateScore(_owner.Score, _owner.IsWanted(), killable.IsWanted(), true, false);
						GameManager.Instance.WantedGame.FirstBullet (_owner);
					}
				}
				if (GameModeManager.Instance.GetGameMode () == GameMode.DeathMatch) {
					ScoreManager.Instance.CalculateScore(_owner.Score, _owner.IsWanted(), killable.IsWanted(), true, false);
				}
			}

//			if (killable.Kill(_owner.IsWanted()))
//			{
//				bool firstShot = !GameManager.Instance.WantedGame.FirstBulletIsShot
//					&& GameModeManager.Instance.GetGameMode() == GameMode.Wanted;
//				if (firstShot) {
//					GameManager.Instance.WantedGame.FirstBullet (_owner);
//				}
//				ScoreManager.Instance.CalculateScore(_owner.Score, _owner.IsWanted(), killable.IsWanted(), true, false);
//			}

			AExplosion explosion = Instantiate(_explosion, hitPosition, Quaternion.identity) as Explosion;
			explosion.Explode (directHitCollider, _owner, _ownerCollider, _bIsRevengeBullet);

		}
	}

	protected override void SetTrailColor (Color c)
	{
		
	}

	protected override void SetBulletColor (Color c)
	{
		
	}

	protected override void Reset ()
	{
		gameObject.SetActive(false);
		transform.position = Vector3.zero;
	}
}
