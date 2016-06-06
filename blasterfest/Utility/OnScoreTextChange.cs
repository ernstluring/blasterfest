using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using MEC;

public class OnScoreTextChange : MonoBehaviour {

	[System.Serializable]
	private struct ScoreSounds
	{
		public AudioClip scoreSound;
		public AudioClip revengeScoreSound;
		public AudioClip wantedKilledScoreSound;
		public AudioClip firstRankSound;
		public AudioClip survivorSound;
	}

	[Header("Audio")]
	[SerializeField]
	private ScoreSounds _scoreSounds;

	[Header(" ")]
	[SerializeField]
	private PlayerScore _eventHandler;
	[SerializeField]
	private Text _messageText;
	[SerializeField]
	private Image _crownImage;
	private Text _scoreText;

	private AudioSource _audioSource;

	[System.Serializable]
	public struct ScoreColors {
		public Color normalKillColor;
		public Color revengeKillColor;
		public Color wantedKillColor;
		public Color isWantedColor;
	}

	[SerializeField]
	private ScoreColors _scoreColors;

	private void Awake ()
	{
		_scoreText = GetComponent<Text>();
		_audioSource = GetComponent<AudioSource> ();
		_scoreText.text = string.Empty;
		_messageText.text = string.Empty;
		_crownImage.enabled = false;
	}

	private void OnEnable ()
	{
		_eventHandler.onScore += UpdateScoreDisplay;
		_eventHandler.onRank += GiveCrownOnFirstRank;
		_eventHandler.onSurvivor += OnSurvivor;
	}

	private void OnDisable ()
	{
		_eventHandler.onScore -= UpdateScoreDisplay;
		_eventHandler.onRank -= GiveCrownOnFirstRank;
		_eventHandler.onSurvivor -= OnSurvivor;
	}

	private void UpdateScoreDisplay (int score, bool isShooterWanted, bool wasRevengeKill, bool wasWantedKill)
	{
		_scoreText.text = "+" + score;
		Color color = GetScoreColor (isShooterWanted, wasRevengeKill, wasWantedKill);
		_scoreText.color = color;

		if (color == _scoreColors.wantedKillColor && wasWantedKill) {
			_audioSource.PlayOneShot (_scoreSounds.wantedKilledScoreSound);
			_messageText.text = "Wanted";
		} else if (color == _scoreColors.revengeKillColor) {
			_audioSource.PlayOneShot (_scoreSounds.revengeScoreSound);
			_messageText.text = "Revenge";
		}
		_messageText.color = color;
		_audioSource.PlayOneShot (_scoreSounds.scoreSound);

		Timing.RunCoroutine(ScoreTimer(), Segment.Update);
	}

	private void GiveCrownOnFirstRank (int rank)
	{
		if (rank == 1) {
			_crownImage.enabled = true;
			_audioSource.PlayOneShot (_scoreSounds.firstRankSound);
		} else {
			_crownImage.enabled = false;
		}
	}

	private void OnSurvivor (int score)
	{
		_audioSource.PlayOneShot (_scoreSounds.survivorSound);
		_messageText.color = _scoreColors.isWantedColor;
		_messageText.text = "Survivor";
		_scoreText.text = "+" + score;
		_scoreText.color = _scoreColors.isWantedColor;
		Timing.RunCoroutine(ScoreTimer(), Segment.Update);
	}

	private Color GetScoreColor (bool isShooterWanted, bool wasRevengeKill, bool wasWantedKill)
	{
		if (isShooterWanted) {
			return _scoreColors.isWantedColor;
		} else {
			if (wasRevengeKill) {
				return _scoreColors.revengeKillColor;
			}
			else if (wasWantedKill) {
				return _scoreColors.wantedKillColor;
			} else {
				return _scoreColors.normalKillColor;
			}
		}
	}

	private IEnumerator<float> ScoreTimer ()
	{
		yield return Timing.WaitForSeconds(1);
		_scoreText.text = string.Empty;
		_messageText.text = string.Empty;
	}
}
