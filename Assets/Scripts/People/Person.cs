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

        public Vector2 Direction;

        private Rigidbody2D _rigidBody;

        public void Awake()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
        }

        public void FixedUpdate()
        {
            _rigidBody.AddForce(Direction * (_thrust * Time.fixedDeltaTime));
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            // Todo - pick a new direction if we hit a wall, or have bounced off the same entity > x times.
        }
    }

}
