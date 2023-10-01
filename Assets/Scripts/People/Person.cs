using Assets.Scripts.Environment;
using Assets.Scripts.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

namespace Assets.Scripts.People
{
    public class Person : MonoBehaviour
    {
        [SerializeField]
        private float _rotationSpeed = 1f;

        [SerializeField]
        private float _attractPointCooldownTime = 3f;

        private float _thrust;

        public static HashSet<GameObject> LivePeople { get; private set; } = new();

        private Rigidbody2D _rigidBody;

        private Vector2? _targetPosition;

        private AttractPoint? _currentAttractPoint;

        private float _percentageChanceOfAttractPointSelection;

        private Coroutine _attractPointCooldown;



        public void Awake()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
        }

        public void OnEnable()
        {
            LivePeople.Add(gameObject);
        }

        public void OnDisable()
        {
            LivePeople.Remove(gameObject);

            if (_attractPointCooldown != null)
            {
                _attractPointCooldown = null;
                _thrust *= 2;
            }
        }

        public void OnSpawn(float thrust, float direction, float mass, float percentageChanceOfAttractPointSelection)
        {
            _thrust = thrust;
            _rigidBody.rotation = direction;
            _rigidBody.mass = mass;

            float scale = 1f + ((mass - 1) / 7);
            transform.localScale = new Vector3(scale, scale);

            _percentageChanceOfAttractPointSelection = percentageChanceOfAttractPointSelection;
            SelectAttractPoint();
        }

        private void SelectAttractPoint()
        {
            if (AttractPoint.LiveAttractPoints.Count > 0 && Random.Range(0, 100) < _percentageChanceOfAttractPointSelection && (_currentAttractPoint == null || AttractPoint.LiveAttractPoints.Count > 1))
            {
                var oldAttractPoint = _currentAttractPoint;

                do
                {
                    _currentAttractPoint = AttractPoint.LiveAttractPoints.ElementAt(Random.Range(0, AttractPoint.LiveAttractPoints.Count));
                }
                while(_currentAttractPoint == oldAttractPoint);

                _targetPosition = ((Vector2)_currentAttractPoint.transform.position) + (Random.insideUnitCircle * _currentAttractPoint.Radius);
            }
            else
            {
                _currentAttractPoint = null;
                _targetPosition = null;
            }
        }

        public void FixedUpdate()
        {
            if(_attractPointCooldown == null && _currentAttractPoint != null && _currentAttractPoint.WithinRadius(transform.position))
            {
                // If we have an attract point and we're within its radius, cut speed for a little bit, then potentially pick a new one.
                _attractPointCooldown = StartCoroutine(AttractPointCooldown());
            }
            else if(_targetPosition != null)
            {
                // We have a target position but we're not close to it, so turn towards it.
                float angle = Mathf.Atan2(((Vector2)_targetPosition).y - transform.position.y,  ((Vector2)_targetPosition).x - transform.position.x) * Mathf.Rad2Deg;
                angle -= 90f;
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);

                // If we have a target position but not an attract point, get rid of it when we are within 1 unit
                if(_currentAttractPoint == null && Vector2.Distance(transform.position, (Vector2)_targetPosition) < 1)
                {
                    _targetPosition = null;
                    SelectAttractPoint();
                }
            }

            _rigidBody.AddForce(transform.up * (_thrust * Time.fixedDeltaTime));

        }

        private IEnumerator AttractPointCooldown()
        {
            _currentAttractPoint = null;
            _targetPosition = null;
            _thrust /= 2;

            yield return new WaitForSeconds(_attractPointCooldownTime);

            _thrust *= 2;
            SelectAttractPoint();
            _attractPointCooldown = null;

        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.CompareTag("Scenery"))
            {
                // Turn around
                _rigidBody.rotation = _rigidBody.rotation + Random.Range(150, 210);

                // Maybe pick an attract point?
                SelectAttractPoint();
            }
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Repeller") && _targetPosition == null)
            {
                // Pick a target position away from the repeller
                do
                {
                    _targetPosition = new Vector2(Random.Range(-5, 5), Random.Range(-4, 4));
                }
                while (Vector2.Distance((Vector2)_targetPosition, collision.gameObject.transform.position) < collision.gameObject.GetComponent<CircleCollider2D>().radius);
            }
        }
    }

}
