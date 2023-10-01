using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AppInitializer : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        if (SceneManager.sceneCount == 1)
        {
            SceneManager.LoadSceneAsync("Title", LoadSceneMode.Additive);
        }
    }
}
