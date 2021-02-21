using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Announcer : MonoBehaviour
{
    public AudioSource announcer;

    public AudioClip begin1, begin2, begin3, begin4, begin5, begin6, begin7, begin8;
    
    public AudioClip bothMiss;
    public AudioClip crit1, crit2, crit3, crit4, crit5;
    public AudioClip critFirstTurn1, critFirstTurn2;
    public AudioClip eitherMiss1, eitherMiss2;
    public AudioClip end1;
    public AudioClip enemyHeal1, enemyHeal2;
    public AudioClip enemyMiss1;
    public AudioClip faintWithPartyAlive;
    public AudioClip bothLowHealth1, bothLowHealth2, bothLowHealth3;
    public AudioClip lowHealth1, lowHealth2, lowHealth3, lowHealth4;
    public AudioClip midBattle;
    public AudioClip lateTurn1, lateTurn2;
    public AudioClip attack1, attack2, attack3, attack4, attack5, attack6;
    public AudioClip waiting1, waiting2;
    public AudioClip win1, win2, win3;

    private List<AudioClip> beginList = new List<AudioClip>();
    private List<AudioClip> critList = new List<AudioClip>();
    private List<AudioClip> critFirstTurnList = new List<AudioClip>();
    private List<AudioClip> eitherMissList = new List<AudioClip>();
    private List<AudioClip> enemyHealList = new List<AudioClip>();
    private List<AudioClip> bothLowHealthList = new List<AudioClip>();
    private List<AudioClip> playerLowHealthList = new List<AudioClip>();
    private List<AudioClip> lateTurnList = new List<AudioClip>();
    private List<AudioClip> attackList = new List<AudioClip>();
    private List<AudioClip> waitingList = new List<AudioClip>();
    private List<AudioClip> winList = new List<AudioClip>();



    // Start is called before the first frame update
    void Start()
    {
        beginList.Add(begin1);
        beginList.Add(begin2);
        beginList.Add(begin3);
        beginList.Add(begin4);
        beginList.Add(begin5);
        beginList.Add(begin6);
        beginList.Add(begin7);
        beginList.Add(begin8);

        critList.Add(crit1);
        critList.Add(crit2);
        critList.Add(crit3);
        critList.Add(crit4);
        critList.Add(crit5);

        critFirstTurnList.Add(critFirstTurn1);
        critFirstTurnList.Add(critFirstTurn2);

        eitherMissList.Add(eitherMiss1);
        eitherMissList.Add(eitherMiss2);

        enemyHealList.Add(enemyHeal1);
        enemyHealList.Add(enemyHeal2);

        bothLowHealthList.Add(bothLowHealth1);
        bothLowHealthList.Add(bothLowHealth2);
        bothLowHealthList.Add(bothLowHealth3);

        playerLowHealthList.Add(lowHealth1);
        playerLowHealthList.Add(lowHealth2);
        playerLowHealthList.Add(lowHealth3);
        playerLowHealthList.Add(lowHealth4);

        lateTurnList.Add(lateTurn1);
        lateTurnList.Add(lateTurn2);

        attackList.Add(attack1);
        attackList.Add(attack2);
        attackList.Add(attack3);
        attackList.Add(attack4);
        attackList.Add(attack5);
        attackList.Add(attack6);

        waitingList.Add(waiting1);
        waitingList.Add(waiting2);

        winList.Add(win1);
        winList.Add(win2);
    
        battleBegins();
    }

    private AudioClip getRandomClip(List<AudioClip> playlist)
    {
        float randomListIndex = Random.Range(0F, playlist.Count-1);
        int randomIndex = Mathf.RoundToInt(randomListIndex);
        return playlist[randomIndex];
    }

    private void battleBegins()
    {
        announcer.clip = getRandomClip(beginList);
        announcer.Play();
    }

    public void announceAttack()
    {
        announcer.clip = getRandomClip(attackList);
        announcer.Play();
    }

    public void announceTurnOneCrit()
    {
        announcer.clip = getRandomClip(critFirstTurnList);
        announcer.Play();
    }

    public void announceCrit()
    {
        announcer.clip = getRandomClip(critList);
        announcer.Play();
    }

    public void announceMiss()
    {
        announcer.clip = getRandomClip(eitherMissList);
        announcer.Play();
    }

    public void announceWaiting()
    {
        announcer.clip = getRandomClip(waitingList);
        announcer.Play();
    }

    public void announceBothMiss()
    {
        announcer.clip = bothMiss;
        announcer.Play();
    }

    public void announceHeal()
    {
        announcer.clip = getRandomClip(enemyHealList);
        announcer.Play();
    }

    public void announceBothLowHealth()
    {
        announcer.clip = getRandomClip(bothLowHealthList);
        announcer.Play();
    }

    public void announcePlayerLowHealth()
    {
        announcer.clip = getRandomClip(playerLowHealthList);
        announcer.Play();
    }

    public void announceLateTurn()
    {
        announcer.clip = getRandomClip(lateTurnList);
        announcer.Play();
    }

    public void announceWin()
    {
        announcer.clip = getRandomClip(winList);
        announcer.Play();
    }

    public void announceFaint()
    {
        announcer.clip = faintWithPartyAlive;
        announcer.Play();
    }

}
