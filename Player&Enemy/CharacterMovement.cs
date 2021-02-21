using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public Character character;

    private float lastAnimationTime;
    protected float animationDistance = .14f;
    public bool animating = true;
    private float animationRate = .80f;

    // Update is called once per frame
    void Update()
    {
        float newTime = Time.time;

        // Timing for the animation of characters sidestepping when awaiting instruction
        if (animating & (newTime - lastAnimationTime >= animationRate) & character.getIsMoving())
        {
            lastAnimationTime = newTime;
            animate();
        }
    }

    private void animate()
    {
        float x = this.transform.position.x;
        float y = this.transform.position.y;
        float z = this.transform.position.z;

        this.transform.position = new Vector3(x + animationDistance, y, z);
          
        animationDistance *= -1;
    }
}
