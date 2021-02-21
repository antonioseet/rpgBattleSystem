using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource BGM;
    public AudioClip song1;
    public AudioClip song2;
    public AudioClip song5;
    public AudioClip song8;
    public AudioClip song9;
    public AudioClip song13;

    public AudioClip loss;
    public AudioClip win;
    
    private List<AudioClip> songs = new List<AudioClip>();

    public GameStateManager gameStateManager;

    // Start is called before the first frame update
    void Start()
    {
        songs.Add(song1);
        songs.Add(song2);
        songs.Add(song5);
        songs.Add(song8);
        songs.Add(song9);
        songs.Add(song13);

        changeSong();
    }

    public void changeSong()
    {
        float randomListIndex = Random.Range(0F, songs.Count - 1);
        int randomIndex = Mathf.RoundToInt(randomListIndex);
        BGM.clip = songs[randomIndex];
        BGM.Play();
        print(BGM.clip.name);
    }

    public void playWin()
    {
        BGM.clip = win;
        BGM.Play();
    }

    public void playLoss()
    {
        BGM.clip = loss;
        BGM.loop = false;
        BGM.Play();
    }
}
