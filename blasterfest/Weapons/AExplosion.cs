using UnityEngine;
using System.Collections;

public abstract class AExplosion : MonoBehaviour {

	[SerializeField]
	private LayerMask playerMask;
	[SerializeField]
	private LayerMask destructableTileMask;
	[SerializeField] [Range(0, 1)]
	protected float _radius = 1;
	[Header("Audio")]
	[SerializeField]
	protected AudioClip _explosionSound;

	protected AudioSource _audioSource;
	protected Animator _anim;
	protected LayerMask layerMask;
	protected SpriteRenderer _explosionRenderer;

	protected virtual void Awake ()
	{
		layerMask = playerMask | destructableTileMask;
		_anim = GetComponentInChildren<Animator>();
		_audioSource = GetComponent<AudioSource>();
		_explosionRenderer = GetComponentInChildren<SpriteRenderer> ();
	}

	public abstract void Explode (Collider2D colToIgnore, SpinePlayerController owner, Collider2D ownerCol,
		bool wasRevengeBullet);


	#if UNITY_EDITOR
	private void OnDrawGizmos ()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, _radius);
	}
	#endif
}
