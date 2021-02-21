using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  The Attack class will encapsulate the properties of normal and special attacks 
 */
public class Attack
{
    public string attackName;
    public int power;
    public int accuracy;
    public int specialUses = 4;
    public bool isSpecial;

    public Attack(string name, int power, int accuracy, bool isSpecial)
    {
        this.attackName = name;
        this.power = power;
        this.accuracy = accuracy;
        this.isSpecial = isSpecial;
        // add type specification for specials
    }
}
