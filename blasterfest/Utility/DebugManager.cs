using UnityEngine;
using System.Collections;

public class DebugManager : MonoBehaviour {

	public GameObject fpsPanel;

	void Update () {
		if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.B))
		{
			fpsPanel.SetActive(!fpsPanel.activeInHierarchy);
		}
	}
}
