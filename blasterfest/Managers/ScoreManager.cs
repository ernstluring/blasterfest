using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
	private PlayerScore[] _playersScores;

	public static ScoreManager Instance { get; private set; }

	private void Awake ()
	{
		if (Instance == null)
			Instance = this;
		else {
			Debug.LogError("There is already a ScoreManager in the scene, there can only be one", Instance);
			Destroy(this);
		}
	}

	private void Start ()
	{
		SpinePlayerController[] playerControllers = PlayerManager.Instance.PlayerControllers.ToArray();
		_playersScores = new PlayerScore[playerControllers.Length];
		for (int i = 0; i < playerControllers.Length; i++)
		{
			_playersScores[i] = playerControllers[i].Score;
		}
	}

	private void OnDestroy ()
	{
		Instance = null;
	}

	public void CalculateScore (PlayerScore target, bool isShooterWanted, bool wasWantedKill, bool wasBulletKill,
		bool wasRevengeKill)
	{
		if (isShooterWanted)
		{
			var value = wasBulletKill ? (target.CurrentScore + (2+target.Rank)*2) : 
				(target.CurrentScore + (target.Rank*2));
			target.SetScore(value, isShooterWanted, wasRevengeKill, wasWantedKill);
		}
		// Shooter is not wanted
		else
		{
			if (wasRevengeKill)
			{
				if (wasWantedKill)
				{
					// Revenge wanted bullet kill / revenge wanted splash kill
					var value = wasBulletKill ? (target.CurrentScore + (2+target.Rank)*3) :
						(target.CurrentScore + (target.Rank*4));
					target.SetScore(value, isShooterWanted, wasRevengeKill, wasWantedKill);
				}
				else
				{
					// Revenge normal bullet kill / revenge normal splash kill
					var value = wasBulletKill ? (target.CurrentScore + (4+2)*target.Rank) :
						(target.CurrentScore + (3*target.Rank));
					target.SetScore(value, isShooterWanted, wasRevengeKill, wasWantedKill);
				}
			}
			else
			{
				if (wasWantedKill)
				{
					// Wanted bullet kill / wanted splash kill
					var value = wasBulletKill ? (target.CurrentScore + (4+2)*target.Rank) : 
						(target.CurrentScore + (3*target.Rank));
					target.SetScore(value, isShooterWanted, wasRevengeKill, wasWantedKill);
				}
				else
				{
					// Normal bullet kill / normal splash kill
					var value = wasBulletKill ? (target.CurrentScore + (2+target.Rank)) :
						(target.CurrentScore + target.Rank);
					target.SetScore(value, isShooterWanted, wasRevengeKill, wasWantedKill);
				}
			}
		}

		CalculateRank(target);
	}

	public void SurvivorScore (PlayerScore target)
	{
		int value = 4+2*target.Rank;
		target.SetSurvivor(value);

		CalculateRank(target);
	}

	public void CalculateRank (PlayerScore target)
	{
		int rank = 1;
		for (int i = 0; i < _playersScores.Length; i++)
		{
			if (_playersScores[i] != target)
			{
				if (_playersScores[i].CurrentScore < target.CurrentScore)
				{
					rank--;
				}
				else if (_playersScores[i].CurrentScore > target.CurrentScore)
					rank++;
			}
		}
		if (rank < 1) rank = 1;
		if (rank > 4) rank = 4;
		target.SetRank(rank);
	}
}
