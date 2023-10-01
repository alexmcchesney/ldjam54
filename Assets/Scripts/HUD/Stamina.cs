using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

namespace HUD
{
    public class Stamina : MonoBehaviour
    {
        public ProceduralImage staminaBar;
        public Animator animator;


        private void OnEnable()
        {
            Player.SprintManager.OnStaminaChange += Refresh;
            Player.SprintManager.OnExhaustionSet += SetExhaustion;
        }


        private void OnDisable()
        {
            Player.SprintManager.OnStaminaChange -= Refresh;
            Player.SprintManager.OnExhaustionSet -= SetExhaustion;
        }


        void Refresh(float stamina)
        {
            staminaBar.fillAmount = stamina;
        }


        void SetExhaustion (bool isExhausted)
        {
            animator.SetBool("isExhausted", isExhausted);
        }
    }
}
