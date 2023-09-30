using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    public class Controller : MonoBehaviour
    {
        Rigidbody2D _Rigidbody2D { get { return GetComponent<Rigidbody2D> (); } }


        public float speed = 1f;
        public float rotationSpeed = 1000f;
        public float deadzone = 0.01f;


        private void FixedUpdate()
        {
            Vector2 direction = InputDirection();
            _Rigidbody2D.velocity = speed * direction;

            if (direction == Vector2.zero) { return; }

            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.fixedDeltaTime);
        }


        Vector2 InputDirection()
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            Vector2 direction = new Vector2(x, y);

            if (direction.sqrMagnitude < deadzone) { return Vector2.zero; }
            return direction;
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
