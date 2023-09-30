using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Player
{
    public class Anxiety : MonoBehaviour
    {
        public static event System.Action<float> OnAnxietyChange;


        public float enemyDamage = 0.1f;


        public float Value {
            get { return _value; }
            set
            {
                float newValue = Mathf.Clamp01(value);
                if(newValue != _value && OnAnxietyChange != null) { OnAnxietyChange(newValue); }

                _value = newValue;
            }
        }


        private float _value = 0;


        public void Reset()
        {
            Value = 0;
        }


        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Check for enemy component
            Value += enemyDamage;
        }
    }
}

