using UnityEngine;
using System.Collections;

public class Destruct : MonoBehaviour, IDestructable {
	public void Destroy (Collider2D playerCol, SpinePlayerController player, bool wasRevengeBullet)
	{
		gameObject.SetActive(false);
	}
}
