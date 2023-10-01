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

        [SerializeField]
        private SpriteRenderer _doorSpriteRenderer;

        [SerializeField]
        private Sprite _doorOpenSprite;

        [SerializeField]
        private Sprite _doorClosedSprite;

        public void OnEnable()
        {
            StartCoroutine(SpawnRoutine());
        }

        private IEnumerator SpawnRoutine()
        {
            _doorSpriteRenderer.sprite = _doorOpenSprite;

            yield return null;

            float spawnRate;
            if(_totalSpawned > 0)
            {
                spawnRate = _timeToSpawn / _totalSpawned;
            }
            else
            {
                spawnRate = 0.2f;
            }

            WaitForSeconds delay = new WaitForSeconds(spawnRate);

            int spawned = 0;
            while(spawned < _totalSpawned || _totalSpawned < 0) 
            {
                GameObject personObj = ObjectPool.GetObjectForType("Person", transform, transform.position);
                Person person = personObj.GetComponent<Person>();
                person.OnSpawn(Random.Range(_minThrust, _maxThrust), transform.rotation.eulerAngles.z, Random.Range(_minMass, _maxMass), _percentageChanceOfAttractPointSelection);
                
                spawned++;
                yield return delay;
            }

            _doorSpriteRenderer.sprite = _doorClosedSprite;

        }
    }

}
