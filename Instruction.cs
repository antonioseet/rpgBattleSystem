using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Instruction
{
    public Character attacker;
    public Character target;
    public Attack attack;
    public Announcer announcer;

    public Text blueChunk;
    public Text redChunk;

    private int turnCounter; // For the announcer on first turns
    private DialogueText dialogueText;
    private SfxManager sfx;


    public Instruction(Character attacker, Character target, Attack attack, Announcer announcer, int turnCounter, DialogueText dialogueTextObject, SfxManager sfx
                       , Text blueChunk, Text redChunk)
    {
        this.attacker = attacker;
        this.target = target;
        this.attack = attack;
        this.announcer = announcer;
        this.turnCounter = turnCounter;
        this.dialogueText = dialogueTextObject;
        this.sfx = sfx;
        this.blueChunk = blueChunk;
        this.redChunk = redChunk;
    }

    public Instruction(Character attacker, Announcer announcer, int turnCounter, DialogueText dialogueTextObject, SfxManager sfx
                       , Text blueChunk, Text redChunk)
    {
        this.attacker = attacker;
        this.announcer = announcer;
        this.turnCounter = turnCounter;
        this.dialogueText = dialogueTextObject;
        this.sfx = sfx;
        this.blueChunk = blueChunk;
        this.redChunk = redChunk;
    }

    public bool execute()
    {
        int accuracyRoll = Random.Range(0, 100);
        float damageTaken = 0;
        bool attackHit = true;

        //First check if attack missed
        if (accuracyRoll > attack.accuracy)
        {
            announcer.announceMiss();
            dialogueText.setNewText(attacker.characterName + " used " + attack.attackName + ", but it MISSED!");
            attackHit = false;
        }
        else
        {
            //If we DO NOT miss, check for crit & low HP
            int critRoll = Random.Range(0, 100);
            int extraDamage = Random.Range(-10, 10);

            int attackStat;
            int defenseStat;

            if (attack.isSpecial)
            {
                attackStat = attacker.specialAttackStat;
                defenseStat = target.specialDefenseStat;
            }
            else
            {
                attackStat = attacker.attackStat;
                defenseStat = target.defenseStat;
            }

            if (critRoll < attacker.critChance)
            {
                damageTaken = (((42) * attack.power * ((float)attackStat / (float)defenseStat)) / 20f) + (2 * extraDamage);
                dialogueText.setNewText("(CRITICAL! 2x damage) " + attacker.characterName + " used " + attack.attackName + "!");
                if (turnCounter == 1)
                    announcer.announceTurnOneCrit();
                else
                    announcer.announceCrit();
            }
            else
            {
                damageTaken = (((22) * attack.power * ((float)attackStat / (float)defenseStat)) / 20f) + extraDamage;
                announcer.announceAttack();
                dialogueText.setNewText(attacker.characterName + " used " + attack.attackName + "!");
            }
            sfx.playHit(attack);
        }

        int damage = (int)damageTaken * (-1);

        target.changeHealth(damage);

        if (attackHit)
        {
            if (attacker.isEnemy)
            {
                blueChunk.text = "" + damage;
                blueChunk.color = Color.red;
            }
            else
            {
                redChunk.text = "" + damage;
                redChunk.color = Color.red;
            }
        }
        else
        {
            if (attacker.isEnemy)
            {
                blueChunk.text = "Miss";
                blueChunk.color = Color.white;
            }
            else
            {
                redChunk.text = "Miss";
                redChunk.color = Color.white;
            }
        }

        return attackHit;
    }
}
