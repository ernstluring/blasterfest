using UnityEngine;
using System.Collections;
using Game.Extensions;

public class Move : MonoBehaviour {

	[SerializeField]
	private Vector3 moveDirection;
	[SerializeField]
	private float movementSpeed = 1;
	[SerializeField]
	private float _minXposition = 15;
	[SerializeField]
	private float _maxXPosition = 30;
	[SerializeField]
	private float _minYposition = 1;
	[SerializeField]
	private float _maxYposition = 4;

	private bool _bLeftSideOfScreen;

	void Update () {
		transform.Translate(moveDirection * movementSpeed * Time.deltaTime);

		if (transform.position.x > _maxXPosition && moveDirection.x > 0) {
			transform.SetLocalPositionX (-RandomXPos());
			transform.SetLocalPositionY (RandomYPos ());
		} else if (transform.position.x < -_maxXPosition && moveDirection.x < 0) {
			transform.SetLocalPositionX (RandomXPos());
			transform.SetLocalPositionY (RandomYPos());
		}
	}

	private float RandomXPos ()
	{
		return Random.Range (_minXposition, _maxXPosition);
	}

	private float RandomYPos ()
	{
		return Random.Range (_minYposition, _maxYposition);
	}
}
