using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

namespace HUD
{
    public class Anxiety : MonoBehaviour
    {
        public ProceduralImage anxietyBar;


        private void OnEnable()
        {
            Player.Anxiety.OnAnxietyChange += Refresh;
        }


        private void OnDisable()
        {
            Player.Anxiety.OnAnxietyChange -= Refresh;
        }


        void Refresh(float anxiety)
        {
            anxietyBar.fillAmount = anxiety;
        }
    }
}

