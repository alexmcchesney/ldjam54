using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    public class Controller : MonoBehaviour
    {
        Rigidbody2D _Rigidbody2D { get { return GetComponent<Rigidbody2D> (); } }


        public float speed = 1f;


        private void FixedUpdate()
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            Vector2 dir = new Vector2(x, y).normalized;

            _Rigidbody2D.velocity = dir * speed;
        }
    }
}
