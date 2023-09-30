using Assets.Scripts.Environment;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _roomPrefabs;

    [SerializeField]
    private GameObject _player;

    private int _currentRoomIndex = 0;

    private GameObject _currentRoom;

    public static GameManager Instance { get; private set; }

    public void Awake()
    {
        Instance = this;
    }

    public void OnEnable()
    {
        // There may be a room already loaded in the editor
        var existing = FindObjectOfType<Room>();
        if (existing == null)
        {
            SpawnRoom();
        }
        else
        {
            _currentRoom = existing.gameObject;
        }
    }

    private void SpawnRoom()
    {
        if(_currentRoomIndex > _roomPrefabs.Length-1)
        {
            _currentRoomIndex = 0;
        }

        _currentRoom = Instantiate(_roomPrefabs[_currentRoomIndex], transform);
        _currentRoom.transform.SetParent(null, false);
        _currentRoom.transform.localPosition = Vector3.zero;
        Room room = _currentRoom.GetComponent<Room>();
        _player.transform.position = room.Entrance.transform.position;
    }

    public void NextRoom()
    {
        _currentRoomIndex++;
        _currentRoom.GetComponent<Room>().OnExit();
        Destroy(_currentRoom);
        SpawnRoom();
    }
}
