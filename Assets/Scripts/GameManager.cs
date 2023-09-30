using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] RoomPrefabs;

    private int _currentRoom = 0;

    public void OnEnable()
    {
        // There may be a room already loaded in the editor
    }
}
