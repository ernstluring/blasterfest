using UnityEngine;
using System.Collections;

public enum GameMode { DeathMatch, Wanted }

public class GameModeManager : MonoBehaviour {

	public static GameModeManager Instance { get; private set; }

	private GameMode currentGameMode;

	void Awake () {
		Object.DontDestroyOnLoad (this.gameObject);
		if (Instance == null) {
			Instance = this;
		}
	}

	public void SetGameMode (GameMode gameMode)
	{
		currentGameMode = gameMode;
	}

	public GameMode GetGameMode ()
	{
		return currentGameMode;
	}
}
