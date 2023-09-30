using Assets.Scripts.People;
using Assets.Scripts.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Environment
{
    public class Room : MonoBehaviour
    {
        public GameObject Entrance;

        public void OnExit()
        {
            // Clean up all people
            GameObject[] livePeople = Person.LivePeople.ToArray();
            foreach(GameObject person in livePeople) 
            {
                ObjectPool.PoolObject(person);
            }
        }
    }

}
