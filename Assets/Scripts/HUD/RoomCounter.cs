using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HUD
{
    public class RoomCounter : MonoBehaviour
    {
        public TMP_Text count;


        private void OnEnable()
        {
            GameManager.OnNewRoom += UpdateCount;
        }


        private void OnDisable()
        {
            GameManager.OnNewRoom -= UpdateCount;
        }


        void UpdateCount()
        {
            count.text = (GameManager.Instance.RoomReached).ToString();
        }
    }
}
