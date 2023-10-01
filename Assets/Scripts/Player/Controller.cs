using Assets.Scripts.People;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    [RequireComponent(typeof(Anxiety))]
    public class Controller : MonoBehaviour
    {
        public float rotationSpeed = 1000f;

        public float recoilForce = 4f;
        public float deadzone = 0.01f;

        private Anxiety _anxiety;
        private SprintManager _sprintManager;
        private Rigidbody2D _rigidbody2D;


        public void Awake()
        {
            _anxiety = GetComponent<Anxiety> ();
            _sprintManager = GetComponent<SprintManager> ();
            _rigidbody2D = GetComponent<Rigidbody2D> ();
        }


        public void Reset()
        {
            _anxiety.Reset();
            _sprintManager.Reset();
        }


        private void FixedUpdate()
        {
            Vector2 direction = InputDirection();
            if (!_anxiety.IsInvulnerable) { _rigidbody2D.velocity = _sprintManager.CalculateSpeed() * direction; }

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

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Person person = collision.gameObject.GetComponent<Person>();
            if (person == null) { return; }

            float recoilX = transform.position.x - collision.transform.position.x;
            float recoilY = transform.position.y - collision.transform.position.y;
            Vector2 recoil = recoilForce * new Vector2 (recoilX, recoilY);

            _rigidbody2D.AddForce(recoil, ForceMode2D.Impulse);
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
