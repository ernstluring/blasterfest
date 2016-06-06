using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using MEC;

public class PlayerManager : MonoBehaviour
{
	[System.Serializable]
	public struct PlayerColors
	{
		public Color[] colors;
	}

	[Header("Colors")]
	[SerializeField]
	public PlayerColors _playerColors;

	[SerializeField]
	private List<GameObject> _players = new List<GameObject> (4);
	[SerializeField] private List<Transform> _spawnPositions = new List<Transform> ();
	[SerializeField] private List<SpinePlayerController> _playerControllers = new List<SpinePlayerController>();
	[SerializeField] private Animator _spawnAnimation;

	private List<Transform> _usedSpawnPoints = new List<Transform> ();
	private int _playerAmount;
	private List<GameObject> _activePlayers = new List<GameObject> (4);
	private List<SpinePlayerController> _activePlayerControllers = new List<SpinePlayerController> (4);

	public static PlayerManager Instance { get; private set; }

	public List<GameObject> Players { get { return _activePlayers; } }
	public List<SpinePlayerController> PlayerControllers { get { return _activePlayerControllers; } }
	public List<Transform> SpawnPositions { get { return _spawnPositions; } }

	private void Awake ()
	{
		if (Instance == null)
			Instance = this;
		
		//Object.DontDestroyOnLoad (gameObject);

		_playerAmount = Input.GetJoystickNames ().Length;
		for (int i = 0; i < _playerAmount; i++) {
			_activePlayers.Add (_players [i]);
			_activePlayerControllers.Add (_playerControllers [i]);
		}
	}

	public Transform GetPlayer (PlayerNumber playerNumber)
	{
		return _players [(int)playerNumber].transform;
	}

	public void SetPlayer (Transform player, PlayerNumber playerNumber)
	{
		//TODO set player ID ?
		_players.Add (player.gameObject);
		_playerControllers.Add (player.GetComponent<SpinePlayerController> ());
	}

	public int CountPlayers ()
	{
		return _players.Count;
	}

	public void SpawnPlayer (Transform player, Transform spawnPos)
	{
		player.gameObject.SetActive (false);
		Timing.RunCoroutine (SpawnAnimationAndPlayer (spawnPos, player));
	}

	private IEnumerator<float> SpawnAnimationAndPlayer (Transform spawnPos, Transform player)
	{
		SpinePlayerController playerController = player.GetChild (0).GetComponent<SpinePlayerController> ();

		if (_spawnAnimation != null) {

			Animator spawnAnim = Instantiate(_spawnAnimation) as Animator;
			spawnAnim.transform.position = new Vector3(spawnPos.position.x, spawnPos.position.y + 1f, spawnPos.position.z);
			Color c = playerController.PlayerColor;
			c.a = 0.7f;
			spawnAnim.GetComponent<SpriteRenderer> ().color = c;
			
			
			Destroy(spawnAnim.gameObject, spawnAnim.GetCurrentAnimatorStateInfo(0).length);
			yield return Timing.WaitForSeconds (0.4f);
		}

		if (!player.gameObject.activeInHierarchy) {
			player.gameObject.SetActive (true);
		}
		playerController.PlayRespawnSound ();


		player.position = spawnPos.position;
		playerController.transform.localPosition = Vector3.zero;

		yield return 0;
	}

	public void SpawnAllPlayers ()
	{
		SpinePlayerController playerController;
		for (int i = 0; i < _playerAmount; i++) {
			playerController = Players [i].GetComponentInChildren<SpinePlayerController> ();
			playerController.PlayerColor = _playerColors.colors [i];
			playerController.PlayerColorID = i;
			playerController.SpawnPosition = _spawnPositions [i];
			SpawnPlayer (Players[i].transform, playerController.SpawnPosition);
		}
	}

}
