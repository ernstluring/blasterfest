using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(FpsCounter))]
public class FpsDislay : MonoBehaviour
{
	public Text highestLabel, averageLabel, lowestLabel;
	private FpsCounter _fpsCounter;
	private string[] _counterDigits;


	void Awake ()
	{
		_fpsCounter = GetComponent<FpsCounter> ();
		_counterDigits = new string[100];
	}

	void Start ()
	{
		for (int i = 0; i < 100; i++)
		{
			_counterDigits [i] = i.ToString ();
		}
	}

	void Update ()
	{
		averageLabel.text = _counterDigits [Mathf.Clamp (_fpsCounter.AverageFPS, 0, 99)];
		highestLabel.text = _counterDigits [Mathf.Clamp (_fpsCounter.HighestFPS, 0, 99)];
		lowestLabel.text = _counterDigits [Mathf.Clamp (_fpsCounter.LowestFPS, 0, 99)];
	}
}
