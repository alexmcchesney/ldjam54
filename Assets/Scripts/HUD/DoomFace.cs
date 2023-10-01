using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace HUD
{
    public class DoomFace : MonoBehaviour
    {
        public Image faceImage;
        public DoomFaceOption[] faceOptions;


        private DoomFaceOption[] _sortedOptions;
        private float[] _thresholds;


        private void OnEnable()
        {
            Player.Anxiety.OnAnxietyChange += UpdateFace;
            ResetArrays();
        }


        private void OnDisable()
        {
            Player.Anxiety.OnAnxietyChange -= UpdateFace;
        }


        void UpdateFace(float anxiety)
        {
            float t = Mathf.InverseLerp(0, Player.Anxiety.MAX_ANXIETY, anxiety);

            for (int i = 0; i < _thresholds.Length; i++)
            {
                if (_thresholds[i] <= t && _thresholds[i + 1] > t)
                {
                    faceImage.sprite = _sortedOptions[i].sprite; return;
                }
            }

            faceImage.sprite = _sortedOptions[_sortedOptions.Length - 1].sprite;
        }


        void ResetArrays()
        {
            _sortedOptions = faceOptions.OrderBy<DoomFaceOption, float>(x => x.upperAnxietyBound).ToArray();

            _thresholds = new float[1 + faceOptions.Length];
            _thresholds[0] = 0;
            for (int i = 0; i < _sortedOptions.Length; i++)
            {
                _thresholds[i + 1] = _sortedOptions[i].upperAnxietyBound;
            }
        }
    }
}
