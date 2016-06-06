using UnityEngine;
using System.Collections.Generic;

public class OnWantedLights : MonoBehaviour {

	private List<SpriteRenderer> _lights = new List<SpriteRenderer>();

	private void Start ()
	{
		foreach (Transform child in transform) {
			_lights.Add (child.GetComponent<SpriteRenderer>());
		}
		Color[] colors = PlayerManager.Instance._playerColors.colors;
		for (int i = 0; i < _lights.Count; i++) {
			_lights [i].color = colors [Random.Range (0, colors.Length)];
		}
	}

	private void OnEnable ()
	{
		EnvironmentEvents.onWanted += ColorLights;
	}

	private void OnDisable ()
	{
		EnvironmentEvents.onWanted -= ColorLights;
	}

	private void ColorLights (SpinePlayerController playerController)
	{
		for (int i = 0; i < _lights.Count; i++) {
			_lights [i].color = playerController.PlayerColor;
		}
	}
}
