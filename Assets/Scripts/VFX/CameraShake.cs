using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VFX
{
    public class CameraShake : MonoBehaviour
    {
        public float anxietyThreshold = 0.75f;
        public float shakeBaseStrength = 0.075f;


        Vector3 _startingPosition;
        float _anxiety;
        Coroutine _shakeCoroutine;


        private void Awake()
        {
            _startingPosition = transform.position;
        }


        private void OnEnable()
        {
            Player.Anxiety.OnAnxietyChange += UpdateAnxiety;
            _shakeCoroutine = StartCoroutine(ShakeCoroutine());
        }


        private void OnDisable()
        {
            Player.Anxiety.OnAnxietyChange -= UpdateAnxiety;
            if (_shakeCoroutine != null)
            {
                StopCoroutine(ShakeCoroutine());
                _shakeCoroutine = null;
            }
        }


        private void UpdateAnxiety(float anxiety)
        {
            _anxiety = anxiety;
        }


        IEnumerator ShakeCoroutine()
        {
            yield return null;

            while(_shakeCoroutine != null)
            {
                if (_anxiety < anxietyThreshold) {
                    transform.position = _startingPosition;
                }
                else
                {
                    Vector2 direction = Random.insideUnitCircle.normalized;
                    float magnitude = shakeBaseStrength * Mathf.InverseLerp(anxietyThreshold, Player.Anxiety.MAX_ANXIETY, _anxiety);

                    Vector2 displacement = direction * magnitude;
                    transform.position = new Vector3(_startingPosition.x + displacement.x, _startingPosition.y + displacement.y, _startingPosition.z);
                }

                yield return null;
            }
        }
    }
}
