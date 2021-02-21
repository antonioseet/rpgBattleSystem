using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
