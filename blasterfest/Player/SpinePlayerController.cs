using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Game.Extensions;
using Spine.Unity;
using Game.CharacterController2D;
using MEC;

public enum PlayerNumber
{
	One = 0,
	Two = 1,
	Three = 2,
	Four = 3
}

[System.Serializable]
public struct PlayerSounds
{
	public AudioClip jetpack;
	public AudioClip spawn;
	public AudioClip walk;
	public AudioClip land;
}

/**
 * The PlayerController class
 */
public class SpinePlayerController : MonoBehaviour, IKillable
{
	/**
	 * Debug Variables
	 */
	[SerializeField]
	public bool unlimitedFuel = false;
	[SerializeField]
	public bool bUseKeyboard = false;

	/**
	 * Member Variables
	 */
	private CharacterController2D _controller;
	//private Animator _animator;
	private Vector3 _velocity;
	//private SpriteRenderer _spriteRenderer;
	private MeshRenderer _meshRenderer;
	private AudioSource _audioSource;

//	private float _normalizedHorizontalSpeed = 0;
	private float _currentRunSpeed = 0;
	private Vector2 _rocketJumpDir;

	private bool _bisInvincible = false;
	private bool _bGunHasShot = false;
	private bool _bRocketJump = false;
	private bool _bRespawning = false;
	private bool _bIsKilled = false;
	private bool _isWanted;
	private GameObject _boltsAndGears;
	private bool lastGrounded = false;

	/** 
	 * Inspector Variables
	 */
	[Header("Animation")]
	public SkeletonAnimation skeletonAnimation;
	[SpineAnimation]
	public string idle = "Idle";
	[SpineAnimation]
	public string run = "Walk cycle";
	[SpineAnimation]
	public string fly = "Hovering";
	[SpineAnimation]
	public string fall = "Falling";
	[SpineAnimation]
	public string takeOff = "Take off";
	[SpineAnimation]
	public string land = "Land";
	[SpineAnimation]
	public string deathFlying = "Death flying";
	[SpineAnimation]
	public string deathLand = "Death land";
	[SpineAnimation]
	public string deathLaunch = "Death launch";

	[Header("Audio")]
	[SerializeField]
	private PlayerSounds _playerSounds;
	[SpineEvent]
	public string footstepEvent = "Footstep";
	[SpineEvent]
	public string landingEvent = "Land";
	[SpineEvent]
	public string takeOffEvent = "Take off";
	[SpineEvent]
	public string slugParticleEvent = "Slug particle";

	[Header("Movement")]
	[SerializeField]
	private float _gravity = -15.02f;
	[SerializeField]
	private float _maxRunSpeed = 3.5f;
	[SerializeField]
	private float _acceleration = 0.33f;
	[SerializeField]
	private float _groundDamping = 9;
	[SerializeField]
	private float _inAirDamping = 5;
	[SerializeField]
	private float _flyForce = 0.7f;
	[SerializeField]
	private float _minVelocityY = -7.61f;
	[SerializeField]
	private float _maxVelocityY = 5.7f;
	//	[SerializeField]
	//	private float _maxRocketJumpVelocity = 20;
	[SerializeField]
	private float _rocketJumpForce = 27;
	[SerializeField]
	private float _rocketJumpDuration = 0.3f;
	[SerializeField]
	private float _floatSpeed = 5;
	[SerializeField]
	private PlayerNumber _playerNumber;
	[SerializeField]
	private SpineBaseGun _gun;
	[SerializeField]
	private FuelBar _fuelBar;
	[SerializeField]
	private WantedCircleProgressBar _wantedProgressBar;
	[SerializeField]
	private GameObject _boltsAndGearsPrefab;
	[SerializeField]
	private ParticleSystem _blood;
	[SerializeField]
	private ParticleSystem _jetpackSmoke;
	[SerializeField]
	private ParticleSystem _walkRubble;
	[SerializeField]
	private PlayerScore _score;

	private float jumpEndTime;

	private Shader _whiteShader;
	private Shader _defaultShader;

	private bool _bIsJetpacking = false;

	/**
	 * Properties
	 */
	public PlayerNumber PlayerNumber
	{ 
		get { return _playerNumber; }
		set { _playerNumber = value; }
	}
	public Color PlayerColor { get; set; }
	public int PlayerColorID { get; set; }
	public string PlayerColorName { get; private set; }
	public Transform SpawnPosition { get; set; }
	public WantedCircleProgressBar WantedProgressBar
	{
		get { return _wantedProgressBar; }
	}
	public SpineBaseGun Gun { get { return _gun; } }
	public PlayerScore Score { get { return _score; } }
	public AControlsMapper ControlsMapper { get; private set; }
	public bool active = true;
	public bool IsInvincible { get { return _bisInvincible; } set { _bisInvincible = value; } }

	private bool _bEndScreenIsActive = false;

	private void Awake ()
	{
		//_animator = GetComponent<Animator> ();
		_controller = GetComponent<CharacterController2D> ();
		//_spriteRenderer = GetComponent<SpriteRenderer>();
		_meshRenderer = GetComponent<MeshRenderer> ();
		_audioSource = GetComponent<AudioSource>();
		skeletonAnimation = GetComponent<SkeletonAnimation> ();
//		Score = GetComponent<PlayerScore>();
		_score = GetComponent<PlayerScore>();

		if (!bUseKeyboard)
		{
			CheckSystemForControls ();
		}
		else
		{
			ControlsMapper = new KeyboardControlsMapper (_playerNumber);
		}
		_boltsAndGears = Instantiate(_boltsAndGearsPrefab, transform.position, Quaternion.identity) as GameObject;
		_boltsAndGears.SetActive(false);

		GameManager.onEndState += (playerWithHeighestScore) => _bEndScreenIsActive = false;
	}

	private void Start ()
	{
		PlayerColorName = GetSkin (PlayerColorID);
		skeletonAnimation.skeleton.SetSkin (PlayerColorName);
		_gun.skeletonAnimation.skeleton.SetSkin (PlayerColorName);
		_fuelBar.SetColor(_playerNumber);
		_whiteShader = Shader.Find ("Sprites/DefaultColorFlash");
		_defaultShader = _meshRenderer.material.shader;
		skeletonAnimation.state.Event += HandleSpineEvent;
		_jetpackSmoke.SetEmissionRate(0);
		_walkRubble.SetEmissionRate(0);
	}

	private void OnDestroy ()
	{
		skeletonAnimation.state.Event -= HandleSpineEvent;
	}

	private string GetSkin (int id) {
		switch (id) {
		case 0:
			return "Red";;
		case 1:
			return "Blue";
		case 2:
			return "Green";
		case 3:
			return "Yellow";
		default:
			return "Red";
		}
	}

	private void HandleSpineEvent (Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
		if (e.Data.Name == footstepEvent) {
			// play footstep audio
			// TODO change the audiosource to dedicated footstep audiosource so multiple sounds can happen
			_audioSource.Stop ();
			_audioSource.clip = _playerSounds.walk;
			_audioSource.pitch = RandomPitch (0.2f);
			_audioSource.Play ();
		}
	}

	private float RandomPitch (float maxOffset)
	{
		return 1f + Random.Range (-maxOffset, maxOffset);
	}

	private void CheckSystemForControls ()
	{
		if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
		{
			ControlsMapper = new XboxControllerMapperWindows (_playerNumber);
		}
		else if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor)
		{
			ControlsMapper = new XboxControllerMapperOSX (_playerNumber);
		}
	}

	private void Update ()
	{
		GunAim ();
		if (!active) {
			if (!_controller.isGrounded) {
				_velocity.y += _gravity * Time.deltaTime;
			} else {
				_velocity.y = 0;
			}
			_controller.move (_velocity * Time.deltaTime);
			return;
		}


		if (!_bRespawning) {
			Controller ();
		} else if (_bRespawning) {
			RevengeShot ();
		}

		SpriteFlip ();

		if (transform.position.y < -9 && !_bRespawning) {
			Timing.RunCoroutine (Respawn ());
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			if (_playerNumber == PlayerNumber.One)
				Kill (true);
		}
	}

	private void GunAim ()
	{
		if (!_bEndScreenIsActive && Input.GetAxisRaw (ControlsMapper.GetAimHorizontalAxis ()) != 0 || Input.GetAxisRaw (ControlsMapper.GetAimVerticalAxis ()) != 0)
		{
			_gun.Aim (Input.GetAxisRaw (ControlsMapper.GetAimHorizontalAxis ()), Input.GetAxisRaw (ControlsMapper.GetAimVerticalAxis ()));
		}
	}

	private void SpriteFlip ()
	{
		if (!_bEndScreenIsActive && ControlsMapper.AimingRight () && skeletonAnimation.skeleton.FlipX || ControlsMapper.AimingLeft () && !skeletonAnimation.skeleton.FlipX) {
			skeletonAnimation.skeleton.FlipX = !skeletonAnimation.skeleton.FlipX;
		}
	}

	private void RevengeShot ()
	{
		if (ControlsMapper.ShootTriggerPressed () && !_bGunHasShot) {
			_gun.Shoot (_bRespawning);
			_bGunHasShot = true;
		}
		if (!_controller.isGrounded) {
			_velocity.y += _gravity * Time.deltaTime;
		} else {
//			_velocity.y = 0;
			skeletonAnimation.loop = false;
			skeletonAnimation.AnimationName = deathLand;
			_velocity.x = 0;
		}
		_controller.move (_velocity * Time.deltaTime);
	}

	private void Controller ()
	{
		int _normalizedHorizontalSpeed = 0;

		#region Horizontal movement
		if (ControlsMapper.PressedRight ()) {
			_currentRunSpeed += _acceleration;
			_normalizedHorizontalSpeed = 1;
		} else if (ControlsMapper.PressedLeft ()) {
			_currentRunSpeed += _acceleration;
			_normalizedHorizontalSpeed = -1;
		} else {
			_currentRunSpeed = 0;
			_normalizedHorizontalSpeed = 0;
		}

		if (_currentRunSpeed > _maxRunSpeed) {
			_currentRunSpeed = _maxRunSpeed;
		}
		#endregion

		#region Jetpack
		// when jetpack control trigger is pressed
		if (ControlsMapper.JetpackTrigger () && _fuelBar.FuelCount () > 0) {
			_bIsJetpacking = true;

			if (_controller.isGrounded) {
				Debug.Log("Take off");
			}


			if (_audioSource.clip != _playerSounds.jetpack)
				_audioSource.clip = _playerSounds.jetpack;
			if (!_audioSource.isPlaying && _audioSource.clip == _playerSounds.jetpack)
				_audioSource.Play();

			// activate jetpack smoke particle
			_jetpackSmoke.SetEmissionRate(40 *Input.GetAxisRaw(ControlsMapper.GetJetpackTrigger()));
			// add flyforce to velocity vector
			_velocity.y += _flyForce;
			//_velocity.y += Input.GetAxisRaw(ControlsMapper.GetJetpackTrigger()) * _flyForce;
			if (!unlimitedFuel)
				_fuelBar.Burn ();

			_velocity.y = Mathf.Clamp (_velocity.y, _minVelocityY, _maxVelocityY);
		}
		// when jetpack control trigger is released
		else if (!ControlsMapper.JetpackTrigger())
		{
			_bIsJetpacking = false;
			if (_audioSource.clip == _playerSounds.jetpack && _audioSource.isPlaying)
				_audioSource.Stop();
			if (!_controller.isGrounded)
				_velocity.y = Mathf.Clamp (_velocity.y, _minVelocityY, _maxVelocityY);
			// disable jetpack smoke particle
			_jetpackSmoke.SetEmissionRate(0);
		}
		if (_controller.isGrounded && _fuelBar.FuelCount () < 1) {
			_fuelBar.Recharge ();
		}
		#endregion

		#region Shoot
		if (ControlsMapper.ShootTriggerPressed () && !_bGunHasShot)
		{
			if (_gun.Shoot (_bRespawning))
			{
				Vector3 dir = _gun.transform.right.normalized*-1;
				_velocity += dir * _gun.Recoil;
				_bGunHasShot = true;
			}
		}
		else if (ControlsMapper.ShootTriggerReleased () && _bGunHasShot)
		{
			_bGunHasShot = false;
		}
		#endregion

		// apply air or ground damping to movement
		float smoothedMovementFactor = _controller.isGrounded ? _groundDamping : _inAirDamping;
		_velocity.x = Mathf.Lerp (_velocity.x, _normalizedHorizontalSpeed * _currentRunSpeed, Time.deltaTime * smoothedMovementFactor);

		if (_bRocketJump) {
			// calculate the velocity of a rocket jump based of gravity and desired height
			//_velocity.y = Mathf.Sqrt(2 * _rocketJumpForce * -_gravity);
			_velocity.y = _rocketJumpForce * _rocketJumpDuration;
		}

		if (_controller.isGrounded && ControlsMapper.PressedDown ())
		{
			_velocity.y *= -1f;
			_controller.ignoreOneWayPlatformsThisFrame = true;
		}

		_velocity.y += _gravity * Time.deltaTime;

		_controller.move (_velocity * Time.deltaTime);


		// landing
		if (!lastGrounded && _controller.isGrounded)
		{
			skeletonAnimation.state.SetAnimation (1, land, false);
			if (_audioSource.clip != _playerSounds.land)
				_audioSource.clip = _playerSounds.land;
			if (!_audioSource.isPlaying && _audioSource.clip == _playerSounds.land)
				_audioSource.Play();
		}

		#region Graphics
		if (_controller.isGrounded)
		{

			if (_currentRunSpeed == 0) // idle
			{
				skeletonAnimation.AnimationName = idle;
//				skeletonAnimation.state.SetAnimation(0, idle, true);
				if (skeletonAnimation.timeScale != 1)
					skeletonAnimation.timeScale = 1;
				_walkRubble.SetEmissionRate(_currentRunSpeed);
			}
			else // run
			{
				skeletonAnimation.AnimationName = run;
//				skeletonAnimation.state.SetAnimation(0, run, true);
				skeletonAnimation.timeScale = 2;
				_walkRubble.SetEmissionRate(_currentRunSpeed);
			}
		} 
		else
		{
			if (_velocity.y > 0 && _bIsJetpacking) // fly
			{
				skeletonAnimation.AnimationName = fly;
				//skeletonAnimation.state.SetAnimation(0, fly, true);
				if (skeletonAnimation.timeScale != 1)
					skeletonAnimation.timeScale = 1;
				_walkRubble.SetEmissionRate(0);
			}
			else //fall
			{
				skeletonAnimation.AnimationName = fall;
//				skeletonAnimation.state.SetAnimation(0, fall, true);
				if (skeletonAnimation.timeScale != 1)
					skeletonAnimation.timeScale = 1;
			}
			skeletonAnimation.loop = true;
		}
		#endregion

		_velocity = _controller.velocity;

		// store previous state
		lastGrounded = _controller.isGrounded;

		if (!(Time.time < jumpEndTime))
		{
			_bRocketJump = false;
			_rocketJumpDir = Vector2.zero;
		}
	}

	public bool Kill (bool mustDie)
	{
		if (_bisInvincible)
			return false;

		if (GameModeManager.Instance.GetGameMode () == GameMode.Wanted) {
			if (!_isWanted && GameManager.Instance.WantedGame.FirstBulletIsShot && !mustDie)
				return false;
		}

		_boltsAndGears.transform.position = transform.position;
		_boltsAndGears.SetActive(true);
		_boltsAndGears.GetComponent<ParticleSystem>().Play();
		_blood.Play();

		_jetpackSmoke.SetEmissionRate(0);
		_walkRubble.SetEmissionRate(0);

		skeletonAnimation.state.AddAnimation (0, deathLaunch, false, 0);
		skeletonAnimation.state.AddAnimation (0, deathFlying, true, 0.5f);
		Timing.RunCoroutine(Respawn(), Segment.Update);
		return true;
	}

	public bool IsWanted ()
	{
		return _isWanted;
	}
	public void SetIsWanted (bool value)
	{
		_isWanted = value;
		EnvironmentEvents.OnWantedEvent (this);
	}

	private IEnumerator<float> Respawn ()
	{
		// After respawn the player is invincible for 3 seconds
		_bisInvincible = true;
		_bRespawning = true;
		_bGunHasShot = false;
		_fuelBar.Empty ();

		_velocity.y = _floatSpeed;
		_bRespawning = true;

//		while (!_controller.isGrounded)
//			yield return 0;

		// When player is killed when he is wanted, wanted mechanic stops and gets passed to other player
//		if (_isWanted) {
//			GameManager.Instance.WantedGame.DisableTimer();
//		}

		// respawn time is 3 seconds
		yield return Timing.WaitForSeconds(3);



		_blood.Stop ();
		PlayerManager.Instance.SpawnPlayer (this.transform.parent, SpawnPosition);
		_velocity.y = 0f;

		_bRespawning = false;
		_fuelBar.Refill ();

		skeletonAnimation.state.ClearTracks ();
		skeletonAnimation.skeleton.SetBonesToSetupPose ();
		skeletonAnimation.AnimationName = idle;

		ToWhiteShader ();
		// 3 secons of invincibility
		yield return Timing.WaitForSeconds(3);
		_bisInvincible = false;

		ToDefaultShader ();
	}

	public void PlayRespawnSound ()
	{
		// Play the spawn audio clip
		if (_playerSounds.spawn != null)
			_audioSource.PlayOneShot(_playerSounds.spawn);
	}

	public void RocketJump (Vector2 dir) 
	{
		jumpEndTime = Time.time + _rocketJumpDuration;
		_bRocketJump = true;
	}

	public string Name ()
	{
		return transform.parent.name;
	}

	private void ToWhiteShader ()
	{
		MaterialPropertyBlock mpb = new MaterialPropertyBlock ();
		mpb.SetFloat ("_TextureFade", 0.6f);
		_meshRenderer.SetPropertyBlock (mpb);
		_gun.ToWhiteShader ();
	}

	private void ToDefaultShader ()
	{
		MaterialPropertyBlock mpb = new MaterialPropertyBlock ();
		mpb.SetFloat ("_TextureFade", 0);
		_meshRenderer.SetPropertyBlock (mpb);
		_gun.ToDefaultShader ();
	}
}
