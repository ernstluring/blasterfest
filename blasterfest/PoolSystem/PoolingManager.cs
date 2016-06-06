using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PoolType { Bullet, BigBullet }

public class PoolingManager {

	private static PoolingManager _instance;

	private Dictionary<PoolType, Pool> objectPools;

	private PoolingManager ()
	{
		this.objectPools = new Dictionary<PoolType, Pool>();
	}

	public static PoolingManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new PoolingManager();
			}
			return _instance;
		}
	}

	public bool CreatePool (GameObject objToPool, PoolType poolType, int poolSize, int maxPoolSize)
	{
		if (PoolingManager.Instance.objectPools.ContainsKey(poolType))
		{
			return false;
		}
		else
		{
			Pool p = new Pool(objToPool, poolSize, maxPoolSize);
			PoolingManager.Instance.objectPools.Add(poolType, p);
			return true;
		}
	}

	public GameObject GetObject(PoolType poolType)
	{
		return PoolingManager.Instance.objectPools[poolType].GetObject();
	}

	public void SetObject(GameObject obj)
	{
		obj.SetActive(false);
		obj.transform.position = Vector3.zero;
	}
}
