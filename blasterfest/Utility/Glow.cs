using UnityEngine;
using System.Collections;
using Game.Extensions;

public class Glow : MonoBehaviour {

	[SerializeField]
	private float _glowSpeed = 1;
	[SerializeField]
	private float _glowAmplitude = 1;

	private SpriteRenderer _spriteRenderer;
	private Color startColor;

	private void Awake ()
	{
		_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		startColor = _spriteRenderer.color;
	}

	private void LateUpdate ()
	{
		_spriteRenderer.SetAlpha(startColor.a + _glowAmplitude * Mathf.Sin(_glowSpeed*Time.time));
	}

	private void OnDisable ()
	{
		_spriteRenderer.color = startColor;
	}
}
