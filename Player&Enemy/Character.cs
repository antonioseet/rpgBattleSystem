using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Character : MonoBehaviour
{
    public string characterName;
    public Rigidbody characterRB;
    public GameObject characterOBJ;
    public int maxHealthPoints, manaPoints, attackStat, defenseStat, specialAttackStat, specialDefenseStat, speedStat, critChance;
    private int currentHealthPoints, displayedHealthPoints;
    private Attack normalAttack, specialAttack;
    private bool isMoving = false;
    public bool hasFainted = false;
    public bool isEnemy;
    private float startingX, startingY, startingZ;

    void Start()
    {
        currentHealthPoints = maxHealthPoints;
    }

    void Update()
    {
        if (currentHealthPoints != displayedHealthPoints)
        {
            moveDisplayedHealth();
        }
    }

    public void changeHealth(int healthChange)
    {
        if(healthChange != 0)
        {
            //if we heal more than max health, set current health to max health
            if (healthChange + currentHealthPoints > maxHealthPoints)
                currentHealthPoints = maxHealthPoints;
            //if health drops below 1, set current health to 0 and set to fainted
            else if (healthChange + currentHealthPoints < 1)
            {
                currentHealthPoints = 0;
                hasFainted = true;
            }
            //else move health to correct spot.
            else
                currentHealthPoints += healthChange;

            if(healthChange < 0)
            {
                int multiplier = 2;
                if (isEnemy)
                    multiplier = 10;
                characterRB.AddForce(new Vector3(0, (1000 * multiplier + healthChange), 0));
            }
            
        }
    }

    public void revive()
    {
        currentHealthPoints = maxHealthPoints;
    }

    private void moveDisplayedHealth()
    {
        if (currentHealthPoints > displayedHealthPoints)
        {
            displayedHealthPoints += 1;
        }
        else if(currentHealthPoints < displayedHealthPoints)
        {
            displayedHealthPoints -= 1;
        }
    }

    public void assignAttacks(Attack normAttack, Attack specAtack)
    {
        normalAttack = normAttack;
        specialAttack = specAtack;
    }

    public int getDisplayedHealth()
    {
        return displayedHealthPoints;
    }

    public int getCurrentHealth()
    {
        return currentHealthPoints;
    }

    public int getMaxHealth()
    {
        return maxHealthPoints;
    }

    public bool getIsMoving()
    {
        return isMoving;
    }

    public void setIsMoving(bool move)
    {
        isMoving = move;
    }
    
    public List<string> getAttackNames()
    {
        List<string> attackNames = new List<string>();
        attackNames.Add(normalAttack.attackName);
        attackNames.Add(specialAttack.attackName);
        return attackNames;
    }

    public List<Attack> getAttacks()
    {
        List<Attack> attacks = new List<Attack>();
        attacks.Add(normalAttack);
        attacks.Add(specialAttack);
        return attacks;
    }

    public Attack getAttack()
    {
        return normalAttack;
    }

    public Attack getSpecialAttack()
    {
        return specialAttack;
    }

    public bool isFullHealth()
    {
        return currentHealthPoints == maxHealthPoints ? true : false;
    }
}
