using UnityEngine;

interface IDestructable
{
	void Destroy (Collider2D playerCol, SpinePlayerController player, bool wasRevengeBullet);
}
interface IKillable
{
	bool Kill (bool mustDie);
	bool IsWanted ();
}
public interface IPoolableObject
{
	void Spawned();
	void Despawned();
}
