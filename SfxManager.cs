using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxManager : MonoBehaviour
{

    public AudioSource sfxManager;
    public AudioClip normalAttackAudio;
    public AudioClip specialAttackAudio;
    // Start is called before the first frame update
    void Start()
    {
        sfxManager.clip = normalAttackAudio;
    }

    public void playHit(Attack attack)
    {
        if (attack.isSpecial )
            sfxManager.clip = specialAttackAudio;
        else
            sfxManager.clip = normalAttackAudio;

        sfxManager.Play();
    }
    
}
