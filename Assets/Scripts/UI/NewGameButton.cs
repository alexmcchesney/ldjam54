using Assets.Scripts.People;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI
{
    public class NewGameButton : MonoBehaviour
    {
        public void NewGame()
        {
            Person.PoolAllPeople();
            SceneManager.UnloadSceneAsync("Title");
            SceneManager.LoadScene("Game", LoadSceneMode.Additive);
        }
    }
}

