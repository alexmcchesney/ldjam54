using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    public class PoolWhenOffScreen : MonoBehaviour
    {
        private static readonly WaitForSeconds CHECK_DELAY = new WaitForSeconds(0.5f);

        private Renderer _renderer;

        private bool _hasBeenOnScreen;

        public void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        public void OnEnable()
        {
            StartCoroutine(CheckForOffScreen());
        }

        private IEnumerator CheckForOffScreen()
        {
            _hasBeenOnScreen = false;
            while (true)
            {
                if(_hasBeenOnScreen && !_renderer.isVisible) 
                {
                    ObjectPool.PoolObject(gameObject);
                }
                else if(!_hasBeenOnScreen)
                {
                    _hasBeenOnScreen = _renderer.isVisible;
                }

                yield return CHECK_DELAY;
            }
        }
    }
}
