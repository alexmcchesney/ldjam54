using Assets.Scripts.People;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI
{
    public class QuitGameButton : MonoBehaviour
    {
        public void Awake()
        {
#if UNITY_WEBGL
            gameObject.SetActive(false);
#endif
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
}

