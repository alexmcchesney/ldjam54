using Assets.Scripts.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.People
{
    public class PersonSpawner : MonoBehaviour
    {
        public static int MaxPeopleInRoom => _All.Select(x => x._totalSpawned).Sum();


        private static List<PersonSpawner> _All = new List<PersonSpawner>();


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
            _All.Add(this);
            StartCoroutine(SpawnRoutine());
        }


        public void OnDisable()
        {
            _All.Remove(this);
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
