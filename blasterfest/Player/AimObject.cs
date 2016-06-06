using UnityEngine;
using System.Collections;

public class AimObject : MonoBehaviour {

	[SerializeField] private PlayerController _playerController;
	[Range(0, 1)]
	[SerializeField] private float _threshold = 0.8f;
	[SerializeField] private Camera _mainCam;

	private Transform _playerTransform;
	private Vector3 _dir = Vector3.right;
	private float _angle;
	private AControlsMapper _controlsMapper;

	void Start () {
		_playerTransform = _playerController.transform;
		_controlsMapper = _playerController.ControlsMapper;
		transform.position = _playerTransform.position;
	}

	void Update () {
		transform.position = _playerTransform.position;

		#if UNITY_EDITOR
		if (!_playerController.bUseKeyboard) {
			if (Input.GetAxisRaw(_controlsMapper.GetAimHorizontalAxis()) != 0 || Input.GetAxisRaw(_controlsMapper.GetAimVerticalAxis()) != 0) {
				_dir = new Vector3 (Input.GetAxisRaw(_controlsMapper.GetAimHorizontalAxis()), -Input.GetAxisRaw(_controlsMapper.GetAimVerticalAxis()),0);
				if (_dir.sqrMagnitude > _threshold * _threshold) {
					_angle = Mathf.Atan2(_dir.y, _dir.x)*Mathf.Rad2Deg;
					transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
				}
			}
		} else {
			_dir = (Input.mousePosition - (_mainCam.WorldToScreenPoint(transform.position))).normalized;
			_angle = Mathf.Atan2(_dir.y, _dir.x)*Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
		}
		#endif
	}
}
