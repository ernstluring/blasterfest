using UnityEngine;
using System.Collections.Generic;
using MEC;

public class PowerUp_BigBullet : APowerUp {

	/**
	 *	Inspector Variables
	 */
	[SerializeField]
	private int _startSpawnTime = 20;
	[SerializeField]
	private int _spawnTime = 40;


	/**
	 *	Member Variables
	 */
	private BoxCollider2D _collider;
	private SpriteRenderer _spriteRenderer;
	private Transform[] _children;
	private AudioSource _audioSource;

	[Header("Audio")]
	[SerializeField]
	private AudioClip _pickupSound;
	[SerializeField]
	private AudioClip _spawnSound;

	private void Start ()
	{
		_children = GetComponentsInChildren<Transform>();
		_collider = GetComponent<BoxCollider2D>();
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_audioSource = GetComponent<AudioSource> ();
		Reset();
		Invoke("FirstSpawn", _startSpawnTime);
	}

	private void FirstSpawn ()
	{
		_audioSource.PlayOneShot(_spawnSound);
		_spriteRenderer.enabled = true;
		_collider.enabled = true;
		for (int i = 0; i < _children.Length; i++) {
			_children[i].gameObject.SetActive(true);
		}
		Timing.RunCoroutine(Spawn(), Segment.Update);
	}

	private IEnumerator<float> Spawn ()
	{
		yield return Timing.WaitForSeconds(_spawnTime);
		_audioSource.PlayOneShot(_spawnSound);
		_spriteRenderer.enabled = true;
		_collider.enabled = true;
		for (int i = 0; i < _children.Length; i++) {
			_children[i].gameObject.SetActive(true);
		}
	}

	private void OnTriggerEnter2D (Collider2D col)
	{
		_audioSource.PlayOneShot(_pickupSound);
		SpineBaseGun gun = col.GetComponent<SpinePlayerController>().Gun;
		gun.LoadSpecialBullet();
		Reset();
		Timing.RunCoroutine(Spawn(), Segment.Update);
	}

	private void Reset ()
	{
		_collider.enabled = false;
		_spriteRenderer.enabled = false;
		foreach (Transform child in transform) {
			child.gameObject.SetActive (false);
		}
	}

}
