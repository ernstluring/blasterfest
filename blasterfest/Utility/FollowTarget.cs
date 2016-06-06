using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour {

	private Transform _myTransform;
	[SerializeField] private Transform _target;

	void Start () {
		_myTransform = transform;
	}
	
	void Update () {
		_myTransform.position = _target.position;
	}
}
