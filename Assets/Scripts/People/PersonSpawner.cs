using Assets.Scripts.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.People
{
    public class PersonSpawner : MonoBehaviour
    {
        [SerializeField]
        private int _totalSpawned;

        [SerializeField]
        private float _timeToSpawn;

        public void OnEnable()
        {
            StartCoroutine(SpawnRoutine());
        }

        private IEnumerator SpawnRoutine()
        {
            yield return null;
            float spawnRate = _timeToSpawn / _totalSpawned;
            WaitForSeconds delay = new WaitForSeconds(spawnRate);

            int spawned = 0;
            while(spawned < _totalSpawned) 
            {
                GameObject person = ObjectPool.GetObjectForType("Person", null, transform.position);
                Vector2 direction = transform.up * -1;
                person.GetComponent<Person>().Direction = direction;
                spawned++;
                yield return delay;
            }
        }
    }

}
