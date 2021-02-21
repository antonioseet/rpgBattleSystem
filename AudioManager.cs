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
    // Adding the main battle music that will be randomly chosen at the beginning of each match
    void Start()
    {
        songs.Add(song1);
        songs.Add(song2);
        songs.Add(song5);
        songs.Add(song8);
        songs.Add(song9);
        songs.Add(song13);

        // Choose first song.
        changeSong();
    }

    // ChangeSong will randomly choose a next song and begin to play it.
    public void changeSong()
    {
        float randomListIndex = Random.Range(0F, songs.Count - 1);
        int randomIndex = Mathf.RoundToInt(randomListIndex);
        BGM.clip = songs[randomIndex];
        BGM.Play();
        print(BGM.clip.name);
    }

    // Once victoy is attained, this song will play and continue to loop until exit.
    public void playWin()
    {
        BGM.clip = win;
        BGM.Play();
    }

    // If the player is defeated, this song will play once.
    public void playLoss()
    {
        BGM.clip = loss;
        BGM.loop = false;
        BGM.Play();
    }
}
