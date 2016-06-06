using UnityEngine;
using System.Collections;
using Game.Extensions;
using MEC;

public class Car : MonoBehaviour {

	private EnvironmentEvents.EnvironmentType type = EnvironmentEvents.EnvironmentType.Car;
	private Vector2[] _goToPositions;
	private SpriteRenderer _spriteRenderer;
	private AudioSource _audioSource;
	private PolygonCollider2D _collider;
	private BoxCollider2D _triggerCollider;
	private bool _bLeftSideOfScreen;
	private GameObject _marker;
	private Camera _mainCamera;
	[SerializeField]
	private float _movementSpeed = 0.1f;
	[Header("Audio")]
	[SerializeField]
	private AudioClip _carSound;

	void Start () {
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_audioSource = GetComponent<AudioSource> ();
		_collider = GetComponent<PolygonCollider2D>();
		_triggerCollider = GetComponent<BoxCollider2D>();
		_mainCamera = Camera.main;
		_marker = transform.GetChild(0).gameObject;
		_marker.SetActive(false);

		_goToPositions = new Vector2[6];
		_goToPositions[0] = new Vector2(-2, 3.67f);
		_goToPositions[1] = new Vector2(22, 3.67f);
		_goToPositions[2] = new Vector2(-2, 6.05f);
		_goToPositions[3] = new Vector2(22, 6.05f);
		_goToPositions[4] = new Vector2(-2, 9.05f);
		_goToPositions[5] = new Vector2(22, 9.05f);

		_audioSource.clip = _carSound;

		RandomPosition ();
	}

	void OnEnable ()
	{
		EnvironmentEvents.onEnvironmentCall += Move;
	}

	void OnDisable ()
	{
		EnvironmentEvents.onEnvironmentCall -= Move;
	}

	private void RandomPosition ()
	{
		int _index = Random.Range(0, _goToPositions.Length);
		transform.localPosition = _goToPositions[_index];
		_bLeftSideOfScreen = transform.position.x < 0;
		_spriteRenderer.flipX = !_bLeftSideOfScreen;
		_collider.enabled = false;
		_triggerCollider.enabled = false;
	}

	private void Move (EnvironmentEvents.EnvironmentType environmentType)
	{
		if (this.type == environmentType)
		{
			_marker.SetActive(true);

			Vector3 lowerLeft = _mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
			Vector3 lowerRight = _mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0));

			_marker.transform.position = _bLeftSideOfScreen ? new Vector3(lowerLeft.x + 0.5f, transform.position.y)
				: new Vector3(lowerRight.x - 0.5f, transform.position.y);

			StartCoroutine(GoMove());
		}
	}

	private IEnumerator GoMove ()
	{
		yield return new WaitForSeconds(5);
		float timer = 0;
		Vector3 startPos = transform.position;
		Vector3 newPos = startPos;

		newPos.x += _bLeftSideOfScreen ? 24 : -24;

		_marker.SetActive(false);
		while(Vector3.Distance(transform.position, newPos) > 0.01f) {
			timer += _movementSpeed * Time.deltaTime;
			transform.position = Vector3.Lerp(startPos, newPos, timer);
			yield return new WaitForEndOfFrame();
		}
		RandomPosition();
	}

	private void OnTriggerEnter2D (Collider2D col)
	{
		IKillable killable = col.GetComponent(typeof(IKillable)) as IKillable;
		if (killable != null)
			killable.Kill(true);
	}

	private void OnBecameVisible ()
	{
		_audioSource.PlayOneShot(_carSound);
		_collider.enabled = true;
		_triggerCollider.enabled = true;
	}
}
