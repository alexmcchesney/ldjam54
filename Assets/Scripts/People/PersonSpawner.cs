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

        [SerializeField]
        private float _minThrust;

        [SerializeField]
        private float _maxThrust;

        [SerializeField]
        private float _minMass;

        [SerializeField]
        private float _maxMass;

        [SerializeField]
        private float _percentageChanceOfAttractPointSelection = 25f;

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
                GameObject personObj = ObjectPool.GetObjectForType("Person", transform, transform.position);
                Person person = personObj.GetComponent<Person>();
                person.OnSpawn(Random.Range(_minThrust, _maxThrust), transform.rotation.eulerAngles.z, Random.Range(_minMass, _maxMass), _percentageChanceOfAttractPointSelection);
                
                spawned++;
                yield return delay;
            }
        }
    }

}
