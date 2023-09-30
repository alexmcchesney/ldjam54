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
        public bool IsInvulnerable
        {
            get { return Time.time < _timeOfMostRecentHit + invulnerabilityDuration; }
        }


        public float enemyDamage = 0.1f;
        public float invulnerabilityDuration = 0.2f;

        [SerializeField]
        private float _anxietyCooldown;

        [SerializeField]
        private float _percentageRecoveryOnNewRoom;

        private float _value = 0;
        private float _timeOfMostRecentHit = -1f;

        public void Awake()
        {
            GameManager.OnNewRoom += OnNewRoom;   
        }

        public void Reset()
        {
            Value = 0;
        }

        public void Update()
        {
            if(Value > 0)
            {
                Value -= _anxietyCooldown * Time.deltaTime;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Person person = collision.gameObject.GetComponent<Person>();
            if(person == null) { return; }
            if(IsInvulnerable) { return; }

            ApplyAnxiety(enemyDamage);
        }


        private void ApplyAnxiety(float change)
        {
            _timeOfMostRecentHit = Time.time;
            Value += change;
        }

        private void OnNewRoom()
        {
            Value -= Value * (_percentageRecoveryOnNewRoom / 100f);
        }
    }
}

