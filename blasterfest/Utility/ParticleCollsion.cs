using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleCollsion : MonoBehaviour
{
	private ParticleSystem _ps;

	private ParticleCollisionEvent[] _collisionEvents;

    [SerializeField]
	private SpriteRenderer _bloodPrefab;

    void Start ()
    {
        _ps = GetComponent<ParticleSystem>();
        _collisionEvents = new ParticleCollisionEvent[5];
	}

	void OnParticleCollision(GameObject other)
    {
        int safeLength = _ps.GetSafeCollisionEventSize();
        if (_collisionEvents.Length < safeLength)
            _collisionEvents = new ParticleCollisionEvent[safeLength];

        int numberOfCollisionEvents = _ps.GetCollisionEvents(other, _collisionEvents);

        for (int i = 0; i < numberOfCollisionEvents; i++)
        {
            if (other.transform.childCount < 5)
            {
                Vector3 collisionPos = _collisionEvents[i].intersection;

				SpriteRenderer bloodpre = (SpriteRenderer)Instantiate(_bloodPrefab, collisionPos, Quaternion.identity);
                bloodpre.transform.SetParent(other.transform);
				Destroy(bloodpre.gameObject, 30);
            }
        }
        
    }

}
