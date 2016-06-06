using UnityEngine;
using System.Collections.Generic;
using Game.Extensions;
using Spine.Unity;

public class SpineBaseGun : MonoBehaviour
{
	[HideInInspector]
	public enum GunArrowStyles { Normal, Wanted, Reload }

	public SkeletonAnimation skeletonAnimation;
	private MeshRenderer _meshRenderer;

	[SpineAnimation]
	public string reload = "Reload";

	[SerializeField] private BaseBullet _bullet;
	[SerializeField] private BaseBullet _bigBullet;
	[SerializeField] private float _recoil = 0.5f;
	[SerializeField] private float _fireRate = 1;
	[SerializeField] private SpriteRenderer _aimArrow;
	[SerializeField] private ParticleSystem _slugs;

	[SerializeField] private Transform _bulletSpawnPos;

	[SerializeField] private Transform _playerTransform;
	[Range (0, 1)]
	[SerializeField] private float _rotationThreshold = 0.6f;

	[SerializeField]
	private Animator _muzzleFlashAnim;

	[Header("Audio")]
	[SerializeField]
	private AudioClip _shootSound;

	private float _nextFire;

	private SpinePlayerController _playerController;
	private Collider2D _playerCollision;
	private AudioSource _audioSource;

	private Queue<BaseBullet> _clipQueue;
	private Stack<BaseBullet> _specialClipStack;
	private const int _clipSize = 3;
	private bool _bIsSpecial = false;

	public bool CanShoot
	{
		get { return Time.time > _nextFire; }
	}

	public float Recoil
	{
		get { return _recoil; }
	}

	protected virtual void Start ()
	{
		_audioSource = GetComponent<AudioSource>();
		_meshRenderer = GetComponentInChildren<MeshRenderer> ();
		_playerController = _playerTransform.GetComponent<SpinePlayerController> ();
		_playerCollision = _playerTransform.GetComponent<Collider2D> ();

		_clipQueue = new Queue<BaseBullet>(_clipSize);
		for (int i = 0; i < _clipSize; i++)
		{
			BaseBullet bullet = Instantiate (_bullet) as Bullet;
			bullet.Init (_playerController, _playerCollision);
			bullet.gameObject.SetActive (false);
			_clipQueue.Enqueue(bullet);
		}

		_specialClipStack = new Stack<BaseBullet>(_clipSize);

		_bigBullet = Instantiate(_bigBullet) as BigBullet;
		_bigBullet.Init (_playerController, _playerCollision);
		_bigBullet.gameObject.SetActive(false);
		skeletonAnimation.state.Complete += (state, trackIndex, loopCount) => {
			if (trackIndex == 0 && Random.value < 0.7f)
				_slugs.Play ();
			skeletonAnimation.skeleton.SetBonesToSetupPose();
		};
	}

	public virtual bool Shoot (bool isRevenge)
	{
		if (CanShoot)
		{
			_nextFire = Time.time + _fireRate;
			_muzzleFlashAnim.SetTrigger("shoot");
			skeletonAnimation.state.SetAnimation (0, reload, false);
			if (!_bIsSpecial)
			{
				//_gunAnim.SetTrigger("shoot");
//				_muzzleFlashAnim.SetTrigger("shoot");
				_audioSource.PlayOneShot(_shootSound);
				BaseBullet bullet = _clipQueue.Dequeue();
				bullet.gameObject.SetActive(true);
				bullet.transform.position = _bulletSpawnPos.position;
				bullet.Move(transform.right, _playerController, _playerCollision, isRevenge, transform.rotation.eulerAngles);
				_clipQueue.Enqueue(bullet);
//				skeletonAnimation.state.SetAnimation (0, reload, false);
			}
			else
			{
				// TODO change to gun animation for special bullet
				// TODO shoot sound for special bullet
//				_muzzleFlashAnim.SetTrigger("shoot");
				BaseBullet specialBullet = _specialClipStack.Pop();
				specialBullet.gameObject.SetActive(true);
				specialBullet.transform.position  = _bulletSpawnPos.position;
				specialBullet.Move(transform.right, _playerController, _playerCollision, isRevenge, transform.rotation.eulerAngles);
				_bIsSpecial = false;
//				skeletonAnimation.state.SetAnimation (0, reload, false);
			}
			return true;
		}
		else
		{
			return false;
		}

	}

	public void LoadSpecialBullet ()
	{
		if (!_bIsSpecial && _specialClipStack.Count < 1)
		{
			_bIsSpecial = true;
			_specialClipStack.Push(_bigBullet);
		}
	}

	public virtual void Aim (float x, float y)
	{
		Vector2 dir = new Vector2 (x, y);
		if (dir.sqrMagnitude > _rotationThreshold * _rotationThreshold)
		{
			float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward * Time.deltaTime);
		}
	}

	public void ToWhiteShader ()
	{
		MaterialPropertyBlock mpb = new MaterialPropertyBlock ();
		mpb.SetFloat ("_TextureFade", 0.6f);
		_meshRenderer.SetPropertyBlock (mpb);
	}

	public void ToDefaultShader ()
	{
		MaterialPropertyBlock mpb = new MaterialPropertyBlock ();
		mpb.SetFloat ("_TextureFade", 0);
		_meshRenderer.SetPropertyBlock (mpb);
	}
}
