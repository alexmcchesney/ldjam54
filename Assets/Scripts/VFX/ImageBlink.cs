using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.VFX
{
    [RequireComponent(typeof(Image))]
    public class ImageBlink : MonoBehaviour
    {
        [SerializeField]
        private Color[] _colors;

        [SerializeField]
        private float _delay;

        private Image _image;

        public void Awake()
        {
            _image = GetComponent<Image>();
        }

        public void OnEnable()
        {
            StartCoroutine(Blink());
        }

        private IEnumerator Blink() 
        {
            var delay = new WaitForSeconds(_delay);
            int colIndex = 0;
            while(true)
            {
                yield return delay;
                _image.color = _colors[colIndex];
                colIndex++;
                if(colIndex == _colors.Length)
                {
                    colIndex = 0;
                }
            }
        }
    }
}

