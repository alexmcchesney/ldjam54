using UnityEngine;
using Assets.Scripts.People;

public class VolumeController : MonoBehaviour
{
    public int maxVolumePersons = 100;
    public float maxVolume = 1.0f;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        int numPersons = Person.LivePeople.Count;
        float volume = Mathf.Clamp01((float)numPersons / maxVolumePersons) * maxVolume;
        audioSource.volume = volume;
    }

    /*
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 20), "Number of Persons: " + GameObject.FindGameObjectsWithTag("Person").Length);
        GUI.Label(new Rect(10, 30, 200, 20), "Volume: " + audioSource.volume);
    }
    */
}
