using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    public class Controller : MonoBehaviour
    {
        Rigidbody2D _Rigidbody2D { get { return GetComponent<Rigidbody2D> (); } }


        public float speed = 1f;
        public float deadzone = 0.1f;


        private void FixedUpdate()
        {
            _Rigidbody2D.velocity = speed * InputDirection();
        }


        Vector2 InputDirection()
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            if ((x * x) + (y * y) < deadzone * deadzone) { return Vector2.zero; }

            return new Vector2(x, y);
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.CompareTag("Exit"))
            {
                GameManager.Instance.NextRoom();
            }
        }
    }
}
