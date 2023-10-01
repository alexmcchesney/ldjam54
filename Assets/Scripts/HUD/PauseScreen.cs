using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HUD
{
    public class PauseScreen : MonoBehaviour
    {
        public static event System.Action<bool> OnPauseChange;


        public bool IsPaused {
            get { return _isPaused; }
            set
            {
                if (_isPaused == value) { return; }

                _isPaused = value;
                _canvasGroup.alpha = _isPaused ? 1f : 0;
                _canvasGroup.blocksRaycasts = _isPaused;
                Time.timeScale = _isPaused ? 0 : 1f;

                if (OnPauseChange != null) { OnPauseChange(value); }
            }
        }
        

        private bool PauseToggleRequested => Input.GetKeyDown(KeyCode.Escape);


        [SerializeField] private CanvasGroup _canvasGroup;
        private bool _isPaused = false;


        private void Update()
        {
            if(!PauseToggleRequested) { return; }

            IsPaused = !IsPaused;
        }
    }
}
