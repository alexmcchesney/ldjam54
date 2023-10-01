using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class SprintManager : MonoBehaviour
    {
        public static event System.Action<float> OnStaminaChange;
        public static event System.Action<bool> OnExhaustionSet;


        public float Stamina {
            get { return _stamina; }
            set {
                float newValue = Mathf.Clamp01(value);
                if (newValue != _stamina)
                {
                    _stamina = newValue;
                    if (OnStaminaChange != null) { OnStaminaChange(newValue); }
                    if (newValue == 0) { IsExhausted = true; }
                    if (newValue == 1f) { IsExhausted = false; }
                }
            }
        }
        public bool IsExhausted {
            get { return _isExhausted; }
            set
            {
                if (_isExhausted == value) { return; }
                _isExhausted = value;
                if(OnExhaustionSet != null) { OnExhaustionSet(value); }
            }
        }
        public bool SprintRequested => Input.GetButton("Submit");


        public float baseSpeed = 3f;
        public float sprintMultiplier = 2f;
        public float sprintBurnTimer = 1.5f;
        public float sprintRecoveryTimer = 3f;
        public float exhaustionSpeedMultiplier = 0.8f;


        private float _stamina = 1f;
        private bool _isExhausted = false;


        public void Reset()
        {
            Stamina = 1f;
            IsExhausted = false;
        }


        public float CalculateSpeed()
        {
            UpdateStamina();

            if(IsExhausted) { return baseSpeed; }
            if(SprintRequested) { return baseSpeed * sprintMultiplier; }

            return baseSpeed;
        }


        void UpdateStamina()
        {
            if(SprintRequested && !IsExhausted)
            {
                Stamina -= Time.fixedDeltaTime / sprintBurnTimer;
            }
            else
            {
                Stamina += Time.fixedDeltaTime / sprintRecoveryTimer;
            }
        }
    }
}
