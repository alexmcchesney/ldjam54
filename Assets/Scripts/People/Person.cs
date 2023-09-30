using Assets.Scripts.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.People
{
    public class Person : MonoBehaviour
    {
        [SerializeField]
        private float _thrust;

        public float Direction
        {
            set
            {
                _rigidBody.rotation = value;
            }
        }

        private Rigidbody2D _rigidBody;

        public void Awake()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
        }

        public void FixedUpdate()
        {
            _rigidBody.AddForce(transform.up * (_thrust * Time.fixedDeltaTime));
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.CompareTag("Scenery"))
            {
                // Turn around
                _rigidBody.rotation = _rigidBody.rotation + Random.Range(150, 210);
            }
        }
    }

}
