using Assets.Scripts.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.People
{
    public class PersonSpawner : MonoBehaviour
    {
        [System.Serializable]
        public class IterationSettings
        {
            public int TotalSpawned;
            public float TimeToSpawn;
            public float MinThrust;
            public float MaxThrust;
            public float MinMass;
            public float MaxMass;
            public float PercentageChanceOfAttractPointSelection = 25f;
        }

        public static int MaxPeopleInRoom => _All.Select(x => x.GetTotalSpawnedThisIteration()).Sum();


        private static List<PersonSpawner> _All = new List<PersonSpawner>();

        [SerializeField]
        private IterationSettings[] _settings;

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
            yield return null;
            var settings = GetCurrentIterationSettings();
            _doorSpriteRenderer.sprite = _doorOpenSprite;

            yield return null;

            float spawnRate;
            if(settings.TotalSpawned > 0)
            {
                spawnRate = settings.TimeToSpawn / settings.TotalSpawned;
            }
            else
            {
                spawnRate = 0.2f;
            }

            WaitForSeconds delay = new WaitForSeconds(spawnRate);

            int spawned = 0;
            while(spawned < settings.TotalSpawned || settings.TotalSpawned < 0) 
            {
                GameObject personObj = ObjectPool.GetObjectForType("Person", transform, transform.position);
                Person person = personObj.GetComponent<Person>();
                person.OnSpawn(Random.Range(settings.MinThrust, settings.MaxThrust), transform.rotation.eulerAngles.z, Random.Range(settings.MinMass, settings.MaxMass), settings.PercentageChanceOfAttractPointSelection);
                
                spawned++;
                yield return delay;
            }

            _doorSpriteRenderer.sprite = _doorClosedSprite;

        }

        private IterationSettings GetCurrentIterationSettings()
        {
            int iteration = 0;

            if (GameManager.Instance != null)
            {
                iteration = GameManager.Instance.Iteration;
            }

            if(iteration >= _settings.Length) 
            {
                return _settings[_settings.Length - 1];
            }
            else
            {
                return _settings[iteration];
            }
        }

        public int GetTotalSpawnedThisIteration()
        {
            return GetCurrentIterationSettings().TotalSpawned;
        }
    }

}
