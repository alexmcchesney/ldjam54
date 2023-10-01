using Assets.Scripts.Environment;
using Assets.Scripts.People;
using Player;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using VFX;

public class GameManager : MonoBehaviour
{
    public int RoomReached {  get; private set; }


    [SerializeField]
    private GameObject[] _roomPrefabs;

    [SerializeField]
    private GameObject _player;

    [SerializeField]
    private GameObject _gameOverNotice;

    private int _currentRoomIndex = 0;

    private GameObject _currentRoom;

    public static System.Action OnNewRoom;

    public int Iteration { get; private set; } = 0;

    public static GameManager Instance { get; private set; }

    public void Awake()
    {
        Instance = this;
    }

    public void OnEnable()
    {
        Anxiety.OnAnxietyChange += OnAnxietyChange;
        Iteration = 0;

        // There may be a room already loaded in the editor
        var existing = FindObjectOfType<Room>();
        if (existing == null)
        {
            SpawnRoom(true);
        }
        else
        {
            _currentRoom = existing.gameObject;
        }
    }

    private void OnDisable()
    {
        Anxiety.OnAnxietyChange -= OnAnxietyChange;
    }

    private void SpawnRoom(bool instant)
    {
        RoomReached++;

        if(_currentRoomIndex > _roomPrefabs.Length-1)
        {
            _currentRoomIndex = 0;
            Iteration++;
        }

        _currentRoom = Instantiate(_roomPrefabs[_currentRoomIndex], transform);
        _currentRoom.transform.SetParent(null, false);
        Room room = _currentRoom.GetComponent<Room>();
        room.OnEnter(instant, _player);
        if(OnNewRoom != null)
        {
            OnNewRoom();
        }
    }

    public void NextRoom()
    {
        _player.SetActive(false);
        _currentRoomIndex++;
        _currentRoom.GetComponent<Room>().OnExit(false);
        SpawnRoom(false);
    }

    public void StartOver()
    {
        _currentRoomIndex = 0;
        RoomReached = 0;
        _player.GetComponent<Controller>().Reset();
        _currentRoom.GetComponent<Room>().OnExit(true);
        Destroy(_currentRoom);
        _gameOverNotice.SetActive(false);
        SpawnRoom(true);
    }

    public void OnAnxietyChange(float newValue)
    {
        if(newValue == 1)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        _player.SetActive(false);
        _gameOverNotice.SetActive(true);
    }

    public void Quit()
    {
        Person.PoolAllPeople();
        CameraShake.Instance.CancelShake();
        SceneManager.UnloadSceneAsync("Game");
        SceneManager.LoadScene("Title", LoadSceneMode.Additive);
    }
}
