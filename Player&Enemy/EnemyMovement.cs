using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : CharacterMovement
{
    //This makes the enemies move in opposite direction from player each time animate() runs
    void Start() => animationDistance *= -1;

}