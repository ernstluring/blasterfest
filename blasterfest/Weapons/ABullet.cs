using UnityEngine;
using System.Collections.Generic;
using MEC;

public abstract class ABullet : MonoBehaviour {

	protected Rigidbody2D _rb;
	protected Animator _animator;
	protected Collider2D _collider;
	protected SpriteRenderer _spriteRenderer;

	[Range(0, 1)]
	[SerializeField]
	protected float _explosionRadius = 0.1f;

	protected virtual void Awake () {
		_rb = GetComponent<Rigidbody2D>();
		_animator = GetComponent<Animator>();
		_collider = GetComponent<Collider2D>();
		_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	protected virtual void OnBecameInvisible ()
	{
		Reset ();
	}

	protected virtual void Reset ()
	{
		_rb.isKinematic = false;
		_collider.enabled = true;
		gameObject.SetActive(false);
		transform.position = Vector3.zero;
	}

	public abstract void Move ();

	#if UNITY_EDITOR
	protected void OnDrawGizmos ()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, _explosionRadius);
	}
	#endif
}
