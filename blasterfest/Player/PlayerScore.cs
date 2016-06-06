using UnityEngine;
using System.Collections.Generic;

public class PlayerScore : MonoBehaviour {

	public List<int> scoreList = new List<int>();
	public List<int> rankList = new List<int>();

	public delegate void OnScore(int score, bool isShooterWanted, bool wasRevengeKill,
		bool wasWantedKill);
	public event OnScore onScore;

	public delegate void OnRank(int rank);
	public event OnRank onRank;

	public delegate void OnSurvivor(int score);
	public event OnSurvivor onSurvivor;

	public int CurrentScore { get; private set; }
	public int Rank { get; private set; }

	private void Start ()
	{
		CurrentScore = 0;
		Rank = 1;
	}

	public void SetScore (int score, bool isShooterWanted, bool wasRevengeKill, bool wasWantedKill)
	{
		if (onScore != null) {
			onScore (score - CurrentScore, isShooterWanted, wasRevengeKill, wasWantedKill);
		}
		CurrentScore = score;
		scoreList.Add (score);
	}

	public void SetRank (int rank)
	{
		this.Rank = rank;
		rankList.Add (rank);
		if (onRank != null) {
			onRank (rank);
		}
	}

	public void SetSurvivor (int score) {
		CurrentScore += score;
		if (onSurvivor != null)
			onSurvivor (score);
	}
}
