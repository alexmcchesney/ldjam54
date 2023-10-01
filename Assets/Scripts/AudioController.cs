using UnityEngine;
using Assets.Scripts.People;

public class VolumeController : MonoBehaviour
{
    public int MaxPeopleInRoom
    {
        get
        {
            int currentMax = PersonSpawner.MaxPeopleInRoom;
            if (currentMax <= 0) { currentMax = defaultMaxPeopleInRoom; }

            return currentMax;
        }
    }
    public float maxVolume = 1.0f;
    public int defaultMaxPeopleInRoom = 400;
    public float lerpIntensity = 0.12f;


    [SerializeField] AudioSource _normalAudioSource;
    [SerializeField] AudioSource _warpedAudioSource;
    float _anxiety = 0;
    float _normalVolumeTarget = 0;
    float _warpedVolumeTarget = 0;


    private void OnEnable()
    {
        Player.Anxiety.OnAnxietyChange += UpdateAnxiety;
        GameManager.OnNewRoom += SlamVolumeToZero;
        HUD.PauseScreen.OnPauseChange += HandlePause;
    }


    private void OnDisable()
    {
        Player.Anxiety.OnAnxietyChange -= UpdateAnxiety;
        GameManager.OnNewRoom -= SlamVolumeToZero;
        HUD.PauseScreen.OnPauseChange -= HandlePause;
    }


    void Update()
    {
        UpdateVolumeTargets();
        LerpVolumes();
    }


    void SlamVolumeToZero()
    {
        _normalAudioSource.volume = 0;
        _warpedAudioSource.volume = 0;
    }


    void UpdateVolumeTargets()
    {
        int numPersons = Person.LivePeople.Count;
        float combinedVolume = Mathf.Clamp01((float)numPersons / MaxPeopleInRoom) * maxVolume;

        float warpFraction = Mathf.InverseLerp(0, Player.Anxiety.MAX_ANXIETY, _anxiety);

        _warpedVolumeTarget = warpFraction * combinedVolume;
        _normalVolumeTarget = (1 - warpFraction) * combinedVolume;
    }


    void LerpVolumes()
    {
        _normalAudioSource.volume = Mathf.Lerp(_normalAudioSource.volume, _normalVolumeTarget, lerpIntensity);
        _warpedAudioSource.volume = Mathf.Lerp(_warpedAudioSource.volume, _warpedVolumeTarget, lerpIntensity);
    }


    void UpdateAnxiety(float anxiety)
    {
        _anxiety = anxiety;
    }


    void HandlePause(bool isPaused)
    {
        _normalAudioSource.enabled = !isPaused;
        _warpedAudioSource.enabled = !isPaused;
    }

    /*
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 20), "Number of Persons: " + GameObject.FindGameObjectsWithTag("Person").Length);
        GUI.Label(new Rect(10, 30, 200, 20), "Volume: " + audioSource.volume);
    }
    */
}
