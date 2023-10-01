using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

namespace HUD
{
    public class Stamina : MonoBehaviour
    {
        public ProceduralImage staminaBar;


        private void OnEnable()
        {
            Player.SprintManager.OnStaminaChange += Refresh;
        }


        private void OnDisable()
        {
            Player.SprintManager.OnStaminaChange -= Refresh;
        }


        void Refresh(float stamina)
        {
            staminaBar.fillAmount = stamina;
        }
    }
}
