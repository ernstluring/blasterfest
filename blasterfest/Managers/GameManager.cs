using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager Instance { get; private set; }

	public enum GameState { Start, Wanted, Deathmatch, End, Wait, Count };
	private GameState _gameState;

	private delegate void RunState();
	private RunState[] _runStateDelegate;

	public delegate void OnEndState (List<SpinePlayerController> allPlayers);
	public static event OnEndState onEndState;
	public delegate void OnStartState ();
	public static event OnStartState onStartState;


	private ScoreManager _scoreManager;
	public WantedGameRound WantedGame {get; private set; }
	public DeathMatch GetDeathMatch { get; private set; }

	[SerializeField]
	private GameMode _gameModeForDebug;

	[SerializeField]
	private int _wantedRounds = 20;

	public void SwitchState (GameState state) {
		_gameState = state;
	}

	private void Awake () {
		//Object.DontDestroyOnLoad(this);
		if (Instance == null) {
			Instance = this;
		} else {
			Debug.LogError("There is already a GameManager in the scene, there can only be one", Instance);
			Destroy(this.gameObject);
		}

		_runStateDelegate = new RunState[(int)GameState.Count];
		_runStateDelegate[(int)GameState.Start] = StartGame;
		_runStateDelegate[(int)GameState.Wanted] = BountyGame;
		_runStateDelegate[(int)GameState.Deathmatch] = DeathMatchGame;
		_runStateDelegate[(int)GameState.End] = EndGame;
		_runStateDelegate[(int)GameState.Wait] = Wait;

	}

	private void Start ()
	{
		//GameModeManager.Instance.SetGameMode (_gameModeForDebug);
		_gameState = GameState.Start;
	}

	private void OnDestroy () {
		if (_runStateDelegate != null) {
			for (int i = 0; i < (int)GameState.Count; i++) {
				_runStateDelegate[i] = null;
			}
			_runStateDelegate = null;
		}
		Instance = null;
	}
	
	private void Update () {
		if (_runStateDelegate[(int)_gameState] != null) {
			_runStateDelegate[(int)_gameState]();
		}
	}

	private void StartGame () {
		PlayerManager.Instance.SpawnAllPlayers ();
		_scoreManager = GetComponent<ScoreManager>();
		if (_scoreManager == null) {
			gameObject.AddComponent<ScoreManager>();
		}
			
		if (GameModeManager.Instance.GetGameMode () == GameMode.Wanted) {
			_gameState = GameState.Wanted;
		}
		if (GameModeManager.Instance.GetGameMode () == GameMode.DeathMatch) {
			_gameState = GameState.Deathmatch;
		}
		onStartState ();
	}

	private void BountyGame () {
		if (WantedGame == null) {
			WantedGame = gameObject.AddComponent<WantedGameRound>();
			WantedGame.maxWantedRounds = _wantedRounds;
		}
	}

	private void DeathMatchGame () {
		if (GetDeathMatch == null) {
			GetDeathMatch = gameObject.AddComponent<DeathMatch> ();
		}
	}

	private void EndGame () {
		float dt = Time.deltaTime;

		List<SpinePlayerController> playerControllers = PlayerManager.Instance.PlayerControllers;
		CameraController.Instance.StopShake ();
		onEndState (playerControllers);

		while (Time.timeScale > 0.1f) {
			Time.timeScale = Mathf.MoveTowards(Time.timeScale, 0, dt * 0.001f);
		}

		Time.timeScale = 0;
		_gameState = GameState.Wait;
	}

	private void Wait () {

	}
}
