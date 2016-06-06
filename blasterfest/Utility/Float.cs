using UnityEngine;
using System.Collections;
using Game.Extensions;

public class Float : MonoBehaviour {

	[SerializeField]
	private bool _float, _scale;

	[SerializeField]
	private float _floatSpeed = 1;
	[SerializeField]
	private float _scaleSpeed = 1;
	[SerializeField]
	private float _floatAmplitude = 0.07f;
	[SerializeField]
	private float _scaleAmplitude = 1;

	private Vector3 startPos;
	private Vector3 startScale;

	private void Awake ()
	{
		startPos = transform.position;
		startScale = transform.localScale;
	}

	private void LateUpdate ()
	{
		if (_float)
			transform.SetPositionY(startPos.y + _floatAmplitude * Mathf.Sin(_floatSpeed*Time.time));
		if (_scale)
			transform.SetScale(startScale.y + _scaleAmplitude * Mathf.Sin(_scaleSpeed*Time.time));
	}

	private void OnDisable ()
	{
		transform.position = startPos;
		transform.localScale = startScale;
	}
}
