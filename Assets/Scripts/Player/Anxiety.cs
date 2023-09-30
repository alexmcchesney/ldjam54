using Assets.Scripts.People;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Player
{
    public class Anxiety : MonoBehaviour
    {
        public static event System.Action<float> OnAnxietyChange;


        public float Value {
            get { return _value; }
            set
            {
                float newValue = Mathf.Clamp01(value);
                if(newValue != _value && OnAnxietyChange != null) { OnAnxietyChange(newValue); }

                _value = newValue;
            }
        }
        public bool OnAnxietyCooldown
        {
            get { return Time.time < _timeOfMostRecentHit + cooldownDuration; }
        }


        public float enemyDamage = 0.1f;
        public float cooldownDuration = 0.2f;


        private float _value = 0;
        private float _timeOfMostRecentHit = -1f;


        public void Reset()
        {
            Value = 0;
        }


        private void OnCollisionEnter2D(Collision2D collision)
        {
            Person person = collision.gameObject.GetComponent<Person>();
            if(person == null) { return; }
            if(OnAnxietyCooldown) { return; }

            ApplyAnxiety(enemyDamage);
        }


        private void ApplyAnxiety(float change)
        {
            _timeOfMostRecentHit = Time.time;
            Value += change;
        }
    }
}

