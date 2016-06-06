using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Game.Extensions;
using MEC;

public class BaseBullet : MonoBehaviour, IPoolableObject {
	protected Rigidbody2D rb;
	public float speed = 10;
	[Range(0, 1)]
	[SerializeField] protected float explosionRadius = 0.1f;
	[SerializeField] protected AExplosion _explosion;
	protected Animator _animator;
	protected Collider2D _collider;
	protected LineRenderer _lr;

	public LayerMask playerMask;

	protected SpinePlayerController _owner;
	protected Collider2D _ownerCollider;
	protected Collider2D[] splashCollisions;
	protected SpriteRenderer _spriteRenderer;
	protected bool _bIsRevengeBullet;

	[Header("Audio")]
	protected AudioSource _audioSource;
	[SerializeField]
	protected AudioClip _bulletSound;

	protected virtual void Awake () {
		rb = GetComponent<Rigidbody2D>();
		_animator = GetComponent<Animator>();
		_collider = GetComponent<Collider2D>();
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_audioSource = GetComponent<AudioSource>();
		_lr = GetComponent<LineRenderer>();
	}

	public virtual void Move (Vector2 dir, SpinePlayerController owner, Collider2D colToIgnore, bool isRevengeBullet, Vector3 rot) {
		Physics2D.IgnoreCollision (_ownerCollider, _collider);
		_audioSource.PlayOneShot (_bulletSound);
//		_owner = owner;
		Invoke ("Reset", 2);
		SetBulletColor (owner.PlayerColor);
		SetTrailColor ( new Color(_owner.PlayerColor.r, _owner.PlayerColor.g, _owner.PlayerColor.b, 0));
		_lr.SetPosition(1, new Vector3(0, 0, -1));
//		_ownerCollider = colToIgnore;
		_bIsRevengeBullet = isRevengeBullet;
//		Physics2D.IgnoreCollision(colToIgnore, _collider);
		transform.rotation = Quaternion.Euler(rot);
		rb.AddForce(dir.normalized * speed, ForceMode2D.Impulse);
		Timing.RunCoroutine(SetTrailLength());
	}

	public virtual void Init (SpinePlayerController owner, Collider2D colToIgnore)
	{
		_owner = owner;
		_ownerCollider = colToIgnore;
	}

	protected virtual void SetTrailColor (Color c)
	{
		_lr.SetColors(_owner.PlayerColor, c);
	}

	protected virtual void SetBulletColor (Color c)
	{
		_spriteRenderer.color = c;
	}

	private IEnumerator<float> SetTrailLength ()
	{
		float t = 0;
		float maxT = -5;
		while (t > maxT) {
			yield return 0;
			_lr.SetPosition(1, new Vector3(t, 0, -1));
			t -= 0.08f;
		}
	}

	protected void OnCollisionEnter2D (Collision2D col)
	{
		CameraController.Instance.Shake();
		Vector3 hitPosition = transform.position;
		Collider2D directHitCollider = col.gameObject.GetComponent<Collider2D>();
		rb.isKinematic = true;
		_collider.enabled = false;
		bool bulletHasToKill = false;

		IDestructable destructable = col.gameObject.GetComponent(typeof(IDestructable)) as IDestructable;
		IKillable killable = col.gameObject.GetComponent(typeof(IKillable)) as IKillable;
		// When bullet hits destructable environment
		if (destructable != null)
			destructable.Destroy(_ownerCollider, _owner, _bIsRevengeBullet);
		// When bullet hits killable object
		if (killable != null)
		{
			if (killable.Kill(_owner.IsWanted()))
			{
				// Wanted Game Mode Kill
				if (GameModeManager.Instance.GetGameMode () == GameMode.Wanted) {

					if (_owner.IsWanted ()) {
						ScoreManager.Instance.CalculateScore(_owner.Score, _owner.IsWanted(), killable.IsWanted(), true, _bIsRevengeBullet);
					}
					if (killable.IsWanted ()) {
						ScoreManager.Instance.CalculateScore(_owner.Score, _owner.IsWanted(), killable.IsWanted(), true, _bIsRevengeBullet);
						GameManager.Instance.WantedGame.DisableTimer (_owner);
					}

					bool firstShot = !GameManager.Instance.WantedGame.FirstBulletIsShot;
					if (firstShot) {
						ScoreManager.Instance.CalculateScore(_owner.Score, _owner.IsWanted(), killable.IsWanted(), true, _bIsRevengeBullet);
						GameManager.Instance.WantedGame.FirstBullet (_owner);
					}
				}

				if (GameModeManager.Instance.GetGameMode () == GameMode.DeathMatch) {
					ScoreManager.Instance.CalculateScore(_owner.Score, _owner.IsWanted(), killable.IsWanted(), true, _bIsRevengeBullet);
				}
			}
		}
		AExplosion explosion = Instantiate(_explosion, hitPosition, Quaternion.identity) as Explosion;
		explosion.Explode (directHitCollider, _owner, _ownerCollider, _bIsRevengeBullet);
		Reset();
	}

	protected virtual void Reset ()
	{
		rb.isKinematic = false;
		rb.velocity = Vector3.zero;
		rb.angularVelocity = 0;
		_collider.enabled = true;
		_bIsRevengeBullet = false;
		gameObject.SetActive(false);
		transform.position = Vector3.zero;
	}

	public void Spawned ()
	{
		// First initialization here
	}

	public void Despawned ()
	{
		// reset data which allows the object to be recycled here
		Reset ();
	}

	#if UNITY_EDITOR
	private void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, explosionRadius);
	}
	#endif
}