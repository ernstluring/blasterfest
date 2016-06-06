using UnityEngine;
using System.Collections.Generic;

public class Pool {

	private List<GameObject> pooledObjects;

	// Sample of the actual object to store. Used when the list needs to grow
	private GameObject pooledObj;

	private int poolSize;
	private int maxPoolSize;

	private Vector3 poolPosition = new Vector3(-100, -100, 0);

	public Pool (GameObject obj, int poolSize, int maxPoolSize)
	{
		pooledObjects = new List<GameObject>();

		for (int i = 0; i < poolSize; i++)
		{
			GameObject go = GameObject.Instantiate(obj, poolPosition, Quaternion.identity) as GameObject;
			go.SetActive(false);
			pooledObjects.Add(go);
		}

		this.maxPoolSize = maxPoolSize;
		this.pooledObj = obj;
		this.poolSize = poolSize;
	}

	public GameObject GetObject ()
	{
		for (int i = 0; i < pooledObjects.Count; i++)
		{
			if (!pooledObjects[i].activeInHierarchy)
			{
				pooledObjects[i].SetActive(true);
				return pooledObjects[i];
			}
		}
		// When we couldn't find a inactive object we need to check if we can grow the list 
		// beyond our current count.
		if (this.maxPoolSize > this.pooledObjects.Count)
		{
			GameObject obj = GameObject.Instantiate(pooledObj, Vector3.zero, Quaternion.identity) as GameObject;
			obj.SetActive(true);
			pooledObjects.Add(obj);
			return obj;
		}
		// If we couldn't find any inactive objects and the list can't grow any further, return null
		return null;
	}
}
