using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class SprintManager : MonoBehaviour
    {
        public static event System.Action<float> OnStaminaChange;


        public float Stamina {
            get { return _stamina; }
            set {
                float newValue = Mathf.Clamp01(value);
                if(newValue != _stamina)
                {
                    _stamina = newValue;
                    if (OnStaminaChange != null) { OnStaminaChange(newValue); }
                    if (newValue == 0) { TriggerExhaustion(); }
                }
            }
        }
        public bool IsExhausted => _exhaustionDurationCoroutine != null;
        public bool SprintRequested => Input.GetButton("Submit");


        public float baseSpeed = 3f;
        public float sprintMultiplier = 2f;
        public float sprintBurnTimer = 1.5f;
        public float sprintRecoveryTimer = 3f;
        public float exhaustionSpeedMultiplier = 0.4f;
        public float exhaustionDuration = 5f;


        private Coroutine _exhaustionDurationCoroutine;
        private float _stamina = 1f;


        public void Reset()
        {
            Stamina = 1f;
            if(_exhaustionDurationCoroutine != null)
            {
                StopCoroutine(_exhaustionDurationCoroutine);
                _exhaustionDurationCoroutine = null;
            }
        }


        public float CalculateSpeed()
        {
            UpdateStamina();

            if(IsExhausted) { return baseSpeed * exhaustionSpeedMultiplier; }
            if(SprintRequested) { return baseSpeed * sprintMultiplier; }

            return baseSpeed;
        }


        void UpdateStamina()
        {
            if (IsExhausted) { return; }

            if(SprintRequested)
            {
                Stamina -= Time.fixedDeltaTime / sprintBurnTimer;
            }
            else
            {
                Stamina += Time.fixedDeltaTime / sprintRecoveryTimer;
            }
        }


        public void TriggerExhaustion ()
        {
            _exhaustionDurationCoroutine = StartCoroutine(ExhaustionDurationCoroutine());
        }


        IEnumerator ExhaustionDurationCoroutine()
        {
            yield return new WaitForSeconds(exhaustionDuration);

            Stamina = 1f;
            _exhaustionDurationCoroutine = null;
        }
    }
}
